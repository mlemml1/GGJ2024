using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Current { get; private set; }

    public GameObject m_hud;
    public BattleHUD m_battleHud;
    public DialogBox m_dialog;

    private CharacterController m_controller;
    private Vector3 m_velocity;
    public Vector3 Velocity => m_velocity;

    private Vector3 m_faceDir = Vector3.forward;
    private bool m_bInBattle;
    public const float maxSpeed = 10.0f;

    void Start()
    {
        Current = this;
        m_controller = GetComponent<CharacterController>();
        m_bInBattle = false;
    }

    void Update()
    {
        if (m_bInBattle)
            return;

        InteractCheck();
    }

    void FixedUpdate()
    {
        // Disable all movement while in dialog.
        if (m_dialog.Active || m_bInBattle)
        {
            return;
        }

        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical") * 1.5f;

        m_velocity = new Vector3(horz, 0, vert);
        float speed = m_velocity.magnitude * maxSpeed;
        m_velocity.Normalize();

        if (speed > maxSpeed)
            speed = maxSpeed;

        m_velocity *= speed;

        //Debug.Log($"speed {speed}");
        m_controller.SimpleMove(m_velocity);

        if (speed > 0.5f)
            m_faceDir = m_velocity;
    }

    T FindUsable<T>() where T : MonoBehaviour
    {
        const float searchDist = 3.0f;

        Vector3 searchPos = transform.position + Vector3.up * 0.5f;

        // Simple raycast.
        RaycastHit hit;
        if (Physics.Raycast(searchPos, m_faceDir, out hit, searchDist))
        {
            if (hit.transform.TryGetComponent<T>(out T obj))
                return obj;
        }

        // Search left/right slightly.
        Vector3 right = Vector3.Cross(Vector3.up, m_faceDir).normalized;
        // Debug.DrawLine(searchPos, searchPos + right * 5.0f, Color.yellow);
        //Debug.DrawLine(searchPos, searchPos + (m_faceDir - right * 0.2f) * searchDist, Color.yellow);
        //Debug.DrawLine(searchPos, searchPos + (m_faceDir + right * 0.2f) * searchDist, Color.yellow);
        //Debug.DrawLine(searchPos, searchPos + (m_faceDir - right * 0.8f) * searchDist, Color.yellow);
        //Debug.DrawLine(searchPos, searchPos + (m_faceDir + right * 0.8f) * searchDist, Color.yellow);

        if (Physics.Raycast(searchPos, m_faceDir - right * 0.2f, out hit, searchDist))
        {
            if (hit.transform.TryGetComponent<T>(out T obj))
                return obj;
        }

        if (Physics.Raycast(searchPos, m_faceDir + right * 0.2f, out hit, searchDist))
        {
            if (hit.transform.TryGetComponent<T>(out T obj))
                return obj;
        }

        // Final search, farther left/right.

        if (Physics.Raycast(searchPos, m_faceDir - right * 0.8f, out hit, searchDist))
        {
            if (hit.transform.TryGetComponent<T>(out T obj))
                return obj;
        }

        if (Physics.Raycast(searchPos, m_faceDir + right * 0.8f, out hit, searchDist))
        {
            if (hit.transform.TryGetComponent<T>(out T obj))
                return obj;
        }

        return null;
    }

    void InteractCheck()
    {
        bool interact = Input.GetButtonDown("Interact");

        // Look for grabbable items.

        // Look for something interactable.


        // Look for talkative dialog/NPCs.
        var dialog = FindUsable<DialogTrigger>();
        if (dialog != null && dialog.m_tree != null)
        {
            // interact hint.
            if (interact)
            {
                StartDialog(dialog.m_tree, dialog.m_target);
            }
        }
    }

    public void StartDialog(DialogTree tree, GameObject target, Action callback = null)
    {
        if (m_dialog.Active)
            return;

        StartCoroutine(m_dialog.ShowDialog(tree, target, callback));
    }

    private Action<bool> m_battleCallback;
    public void StartBattle(EnemyDef enemy, Action<bool> callback = null)
    {
        if (m_bInBattle)
            return;
        m_bInBattle = true;

        m_battleCallback = callback;

        m_hud.SetActive(false);
        m_battleHud.gameObject.SetActive(true);

        m_battleHud.StartBattle(this, enemy);
    }

    public void EndBattle(bool won)
    {
        if (!m_bInBattle)
            return;
        m_bInBattle = false;

        if (!won)
        {
            SceneManager.LoadScene("GameOver");
            return;
        }

        if (m_battleCallback != null)
            m_battleCallback(won);

        m_hud.SetActive(true);
        m_battleHud.gameObject.SetActive(false);
    }
}
