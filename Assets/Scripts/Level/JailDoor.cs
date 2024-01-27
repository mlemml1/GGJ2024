using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class JailDoor : MonoBehaviour
{
    public DialogTree m_callDialog;
    public DialogTree m_guardDialog;
    public DialogTree m_pickupDialog;
    public DialogTrigger m_trigger;
    public Pacer m_guard;
    private bool m_isWalking = false;
    public AudioClip m_laughErupt;
    private AudioSource m_audio;

    // Start is called before the first frame update
    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        LeaveGuard();
    }

    // Update is called once per frame
    void Update()
    {
        // Hack. Too bad!
        if (m_isWalking && m_guard.IsAtPatrolTarget(1))
        {
            m_isWalking = false;
            m_trigger.m_tree = m_guardDialog;
        }
    }

    public void CallGuard()
    {
        m_isWalking = true;
        m_guard.SetPatrolTarget(1);

        // disable interaction.
        m_trigger.m_tree = null;
    }

    public void LeaveGuard()
    {
        m_guard.SetPatrolTarget(0);
        m_trigger.m_tree = m_callDialog;
    }

    public void GuardLaugh()
    {
        StartCoroutine(GuardLaughRoutine());
    }

    private IEnumerator GuardLaughRoutine()
    {
        m_audio.PlayOneShot(m_laughErupt);
        m_guard.m_animator.SetBool("laughing", true);

        yield return new WaitForSeconds(m_laughErupt.length);

        m_audio.Play();
    }
}
