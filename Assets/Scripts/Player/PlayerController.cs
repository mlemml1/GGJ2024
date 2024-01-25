using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController m_controller;
    private Vector3 m_velocity;
    private const float maxSpeed = 15.0f;

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        
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
    }
}
