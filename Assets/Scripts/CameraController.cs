using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera m_camera;
    private PlayerController m_player;
    public float m_pitch = 45.0f;
    private Vector3 m_cameraVel;
    void Start()
    {
        m_camera = GetComponent<Camera>();
        m_player = GameObject.FindFirstObjectByType<PlayerController>();

        // Low FOV, near orthographic projection.
        m_camera.fieldOfView = 10;

        UpdateCamera(true);
    }

    void FixedUpdate()
    {
        UpdateCamera(false);
    }

    void UpdateCamera(bool snap)
    {
        var rot = Quaternion.Euler(m_pitch, 0, 0);
        m_camera.transform.rotation = rot;

        var idealPos = m_player.transform.position + (Vector3.up * 1.0f) + (rot * new Vector3(0, 0, -65));

        if (snap)
        {
            transform.position = idealPos;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, idealPos, ref m_cameraVel, 0.2f);
        }
    }
}
