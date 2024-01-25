using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;


public class DialogBox : MonoBehaviour
{
    public GameObject m_box;
    public TextMeshProUGUI m_text;

    public bool Active
    {
        get => m_box.activeInHierarchy;
        set => m_box.SetActive(value);
    }

    void Start()
    {
        Active = false;
    }

    void Update()
    {
        
    }

    public IEnumerator ShowDialog(DialogTree tree)
    {
        Debug.Log("pre show");
        if (Active)
            yield break;

        Debug.Log("start show");
        m_text.text = "";
        Active = true;

        // Wait a frame for hysteresis.
        yield return null;

        // Split into dialogs.
        var dialogs = tree.dialogText.Split("\n\n");

        foreach (var str in dialogs)
        {
            // Draw the text.
            StringBuilder builder = new();
            for (int i = 0; i < str.Length; i++)
            {
                builder.Append(str[i]);
                m_text.text = builder.ToString();
                yield return new WaitForSeconds(0.05f);
            }


            // Text drawing is done.
            // Hacky loop, wait for user continue.
            while (true)
            {
                if (Input.GetButtonDown("Interact"))
                    break;

                yield return null;
            }
        }

        /*
        var responses = tree.dialogResponses;
        if (responses != null && responses.Count != 0)
        {
            // Setup UI options if we have them.

            // Hacky loop, runs once per frame to check input.
            while (true)
            {
                float horz = Input.GetAxis("Vertical");

                if (horz < -0.5f)
                {
                    // Select previous.
                }
                else if (horz > 0.5f)
                {
                    // Select next.
                }


                yield return null;
            }
        }
        */

        Active = false;
    }
}
