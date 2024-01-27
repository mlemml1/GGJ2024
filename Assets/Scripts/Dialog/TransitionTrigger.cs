using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionTrigger : MonoBehaviour
{
    public string m_nextLevel;

    public void Transition()
    {
        SceneManager.LoadScene(m_nextLevel);
    }
}
