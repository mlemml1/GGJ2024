using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform m_hinge;
    private Plane m_doorPlane;
    private Vector3 m_closedRot;
    private Vector3 m_doorRot;
    private Vector3 m_targetRot;

    public bool m_bOpenUp;
    public DialogTree m_openTree;
    public DialogTree m_closeTree;
    public DialogTrigger m_dialogTrigger;

    private bool m_bIsOpen = false;

    void Start()
    {
        m_doorPlane = new Plane(m_hinge.forward, m_hinge.position);

        m_doorRot = m_hinge.localRotation.eulerAngles;

        m_closedRot = m_doorRot;
        m_targetRot = m_doorRot;

        m_dialogTrigger.m_tree = m_openTree;
    }

    // Update is called once per frame
    private Vector3 m_doorVel;
    void Update()
    {
        // Smoothly move to the target.
        m_doorRot = Vector3.SmoothDamp(m_doorRot, m_targetRot, ref m_doorVel, 0.1f);
        m_hinge.localRotation = Quaternion.Euler(m_doorRot);
    }

    public void Open()
    {
        if (m_bIsOpen) return;
        m_bIsOpen = true;

        // What side of the door is the player on?
        var player = PlayerController.Current;
        if (player == null)
            return;

        bool side = m_doorPlane.GetSide(player.transform.position);

        var axis = m_bOpenUp ? new Vector3(0, 0, 90) : new Vector3(0, 90, 0);

        if (side)
        {
            m_targetRot = m_closedRot + axis;
        }
        else
        {
            m_targetRot = m_closedRot - axis;
        }
        m_dialogTrigger.m_tree = m_closeTree;
    }

    public void Close()
    {
        if (!m_bIsOpen) return;
        m_bIsOpen = false;

        m_targetRot = m_closedRot;
        m_dialogTrigger.m_tree = m_openTree;
    }

    public void Toggle()
    {
        Debug.Log("toggle door");
        if (m_bIsOpen)
            Close();
        else
            Open();

    }
}
