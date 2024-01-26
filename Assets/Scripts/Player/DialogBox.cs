using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogBox : MonoBehaviour
{
    public GameObject m_box;
    public TextMeshProUGUI m_text;
    private AudioSource m_audio;

    public bool Active
    {
        get => m_box.activeInHierarchy;
        set => m_box.SetActive(value);
    }

    void Start()
    {
        Active = false;
        m_audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public IEnumerator ShowDialog(DialogTree tree)
    {
        if (Active)
            yield break;

        m_text.text = "";
        Active = true;

        // Wait a frame for hysteresis.
        yield return null;

        // Split into dialogs.
        var dialogs = tree.dialogText.Split("\n\n");

        float lastSpeechTime = 0;
        //float speechRate = tree.vox?.voxSfx?.length ?? 0;
        float speechRate = 0.1f;

        foreach (var str in dialogs)
        {
            // Draw the text.
            StringBuilder builder = new();
            for (int i = 0; i < str.Length; i++)
            {
                builder.Append(str[i]);
                m_text.text = builder.ToString();

                // play clip.
                if (tree.vox != null && (Time.time - lastSpeechTime) > speechRate)
                {
                    m_audio.pitch = Random.Range(1.0f - tree.vox.warbleScale, 1.0f + tree.vox.warbleScale);
                    m_audio.PlayOneShot(tree.vox.voxSfx);
                    lastSpeechTime = Time.time;
                }

                // speed things up if the user is spamming.
                yield return new WaitForSeconds(Input.GetButton("Interact") ? 0.01f : 0.03f);
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
