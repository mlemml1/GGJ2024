using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioClip m_oneShot;
    public AudioSource m_source;

    public void TriggerOneShot()
    {
        m_source.PlayOneShot(m_oneShot);
    }

    public void Trigger()
    {
        m_source.Play();
    }
}
