using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController m_controller;
    private Vector3 m_velocity;
    private Vector3 m_faceDir = Vector3.forward;
    private const float maxSpeed = 15.0f;

    void Start()
    {
        m_controller = GetComponent<CharacterController>();


    }

    void Update()
    {
        InteractCheck();
    }

    void FixedUpdate()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical") * 1.5f;

        m_velocity = new Vector3(horz, 0, vert);
        float speed = m_velocity.magnitude * maxSpeed;
        m_velocity.Normalize();

        if (speed > maxSpeed)
            speed = maxSpeed;

        //Debug.Log($"speed {speed}");
        m_controller.SimpleMove(m_velocity * speed);

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
        // Look for something interactable.


        // Look for talkative NPCs.
        var npc = FindUsable<DialogTrigger>();
        if (npc != null)
        {
            // interact hint.
        }
    }
}
