using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerGrate : MonoBehaviour
{
    public GameObject m_trigger;
    public Animator m_controller;

    private void Start()
    {
        m_trigger.SetActive(false);
    }
    public void OpenGrate()
    {
        m_controller.SetTrigger("open");
        m_trigger.SetActive(true);
    }
}
