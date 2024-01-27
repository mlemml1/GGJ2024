using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RoomVolume : MonoBehaviour
{
    public bool m_isStartingRoom;

    private bool m_bVisible;
    private float m_opacity;
    private float m_opacityVel;

    void Start()
    {
        m_bVisible = m_isStartingRoom;
        m_opacity = m_isStartingRoom ? 1 : 0;
        SetOpacityRecursive(m_opacity);
    }

    void Update()
    {
        float targetOpacity = m_bVisible ? 1 : 0;

        m_opacity = Mathf.SmoothDamp(m_opacity, targetOpacity, ref m_opacityVel, 0.1f);
        SetOpacityRecursive(m_opacity);
    }

    private void SetOpacityRecursive(float opacity)
    {
        foreach (var obj in this.GetComponentsInChildren<MeshRenderer>())
        {
            obj.enabled = opacity > 0.5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerController>(out var player))
            return;

        m_bVisible = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent<PlayerController>(out var player))
            return;

        m_bVisible = false;
    }
}
