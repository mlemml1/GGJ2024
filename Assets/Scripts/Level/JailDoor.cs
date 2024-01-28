using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class JailDoor : MonoBehaviour
{
    public Transform m_hinge;
    public DialogTree m_callDialog;
    public DialogTree m_guardDialog;
    public DialogTree m_pickupDialog;
    public DialogTree m_unlockDialog;
    public DialogTrigger m_trigger;
    public Pacer m_guard;
    private bool m_isWalking = false;
    public AudioClip m_laughErupt;
    private AudioSource m_audio;
    public GameObject m_keys;

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
        m_trigger.m_tree = null;
        StartCoroutine(GuardLaughRoutine());
    }

    private IEnumerator GuardLaughRoutine()
    {
        m_audio.PlayOneShot(m_laughErupt);
        m_guard.m_animator.SetBool("laughing", true);

        yield return new WaitForSeconds(m_laughErupt.length);

        m_keys.SetActive(true);
        m_audio.Play();
    }

    public void PickupKeys()
    {
        m_keys.SetActive(false);
        m_trigger.m_tree = m_unlockDialog;
    }

    public void Unlock()
    {
        m_trigger.m_tree = null;
        StartCoroutine(UnlockRoutine());
    }

    private IEnumerator UnlockRoutine()
    {
        Vector3 angle = m_hinge.transform.rotation.eulerAngles;
        Vector3 targetAngle = angle + new Vector3(0, 90, 0);

        const int numSteps = 100;

        for (int i = 0; i < numSteps; i++)
        {
            float along = ((float)i / (float)numSteps);
            var rot = Vector3.Lerp(angle, targetAngle, along);
            m_hinge.transform.rotation = Quaternion.Euler(rot);
            yield return null;
        }
    }
}
