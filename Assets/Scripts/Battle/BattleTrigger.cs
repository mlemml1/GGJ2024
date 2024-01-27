using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    private bool m_bBattleStarted = false;
    public DialogTree m_tree;
    public EnemyDef m_enemy;
    public GameObject m_sprite;
    public GameObject m_fizzler;
    public Texture m_fizzleTex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerController>(out var player))
            return;

        if (m_bBattleStarted)
            return;
        m_bBattleStarted = true;

        if (m_tree != null)
        {
            player.StartDialog(m_tree, this.gameObject, () =>
            {
                player.StartBattle(m_enemy, BattleDone);
            });
        }
        else
        {
            // Begin the battle immediately.
            player.StartBattle(m_enemy, BattleDone);
        }
    }

    public void BattleDone(bool won)
    {
        if (won)
            StartCoroutine(DoFizzle());
    }

    private IEnumerator DoFizzle()
    {
        MeshRenderer[] meshes = null;
        if (m_fizzler != null)
        {
            m_sprite.SetActive(false);

            var fizzler = Instantiate(m_fizzler, transform);
            fizzler.transform.localPosition = Vector3.zero;
            fizzler.transform.localRotation = Quaternion.Euler(0, 180, 0);

            meshes = fizzler.GetComponentsInChildren<MeshRenderer>();
            foreach ( var mesh in meshes )
                mesh.material.SetTexture("_BaseMap", m_fizzleTex);
        }

        yield return new WaitForSeconds(1);

        // begin fade.
        for (int i = 100; i >= 0; --i)
        {
            foreach (var mesh in meshes)
                mesh.material.color = mesh.material.color.WithAlpha((float)i / 100.0f);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
