using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailDoor : MonoBehaviour
{
    public DialogTree m_callDialog;
    public DialogTree m_guardDialog;
    public DialogTree m_pickupDialog;
    public DialogTrigger m_trigger;

    // Start is called before the first frame update
    void Start()
    {
        m_trigger.m_tree = m_callDialog;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallGuard()
    {

    }
}
