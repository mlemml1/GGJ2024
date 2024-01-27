using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacer : MonoBehaviour
{
    public Animator m_animator;
    public Transform m_patrolPos1;
    public Transform m_patrolPos2;
    private int m_patrolTarget;

    [Range(0, 4)]
    public float m_paceTime = 0.8f;

    private void Start()
    {
    }

    public void SetPatrolTarget(int target)
    {
        m_patrolTarget = target;
    }

    private Vector3 m_moveVel;
    private void Update()
    {
        // Move to the target point.
        var targetPos = (m_patrolTarget == 0) ? m_patrolPos1.position : m_patrolPos2.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_moveVel, m_paceTime);

        m_animator?.SetBool("walking", !IsAtPatrolTarget(m_patrolTarget));
    }

    public bool IsAtPatrolTarget( int target )
    {
        var targetPos = (target == 0) ? m_patrolPos1.position : m_patrolPos2.position;

        return (targetPos - transform.position).sqrMagnitude < 1.0f;
    }
}
