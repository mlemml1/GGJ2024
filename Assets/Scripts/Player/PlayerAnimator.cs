using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    public PlayerController m_controller;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var velocity = m_controller.Velocity;
        float speed = velocity.magnitude / PlayerController.maxSpeed;

        bool walking = speed > 0.1f;
        m_animator.SetBool("walking", walking);

        if (walking)
        {
            m_animator.speed = speed;

            m_animator.SetFloat("Move_X", velocity.x);
            m_animator.SetFloat("Move_Y", velocity.z);

            // Flip animations.
            //m_spriteRenderer.flipX = velocity.x <= -0.5f;
            m_spriteRenderer.transform.localScale = new(velocity.x > -0.5f ? 0.5f : -0.5f, 0.5f, 1);
        }
    }
}
