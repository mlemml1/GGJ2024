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
    private int m_selectionIndex;

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

    private void UpdateResponses(string pretext, List<DialogOption> options)
    {
        if (m_selectionIndex < 0)
            m_selectionIndex = options.Count - 1;

        if (m_selectionIndex >= options.Count)
            m_selectionIndex = 0;

        StringBuilder sb = new();

        if (!string.IsNullOrEmpty(pretext))
            sb.AppendLine(pretext);

        for (int i = 0; i < options.Count; i++)
        {
            if (i == m_selectionIndex)
                sb.Append("-> ");
            else
                sb.Append("   ");

            sb.AppendLine(options[i].text);
        }

        m_text.text = sb.ToString();
    }

    private bool FindFlag(string flag, ref string str)
    {
        bool hasAny = str.Contains(flag);
        str = str.Replace(flag, "");
        return hasAny;
    }

    public IEnumerator ShowDialog(DialogTree tree, GameObject target)
    {
        if (Active)
            yield break;

        m_text.text = "";
        m_text.verticalAlignment = VerticalAlignmentOptions.Top;
        m_selectionIndex = 0;
        Active = true;

        // Wait a frame for hysteresis.
        yield return null;

        // Split into dialogs.
        if (!string.IsNullOrEmpty(tree.dialogText))
        {
            var dialogs = tree.dialogText.Split("\n\n");

            float lastSpeechTime = 0;
            //float speechRate = tree.vox?.voxSfx?.length ?? 0;
            float speechRate = 0.1f;

            foreach (var str in dialogs)
            {
                var finalStr = str.Replace("\\n", "\n");

                // Hack. Too bad!
                bool isNarration = FindFlag("\\t", ref finalStr);

                m_text.verticalAlignment = isNarration ? VerticalAlignmentOptions.Middle : VerticalAlignmentOptions.Top;

                // Draw the text.
                StringBuilder builder = new();
                for (int i = 0; i < finalStr.Length; i++)
                {
                    builder.Append(finalStr[i]);
                    m_text.text = builder.ToString();

                    // play clip.
                    if (!isNarration && tree.vox != null && (Time.time - lastSpeechTime) > speechRate)
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
        }

        DialogOption? selectedOption = null;

        var responses = tree.dialogResponses;
        if (responses != null && responses.Count != 0)
        {
            // Wait a frame for hysteresis.
            yield return null;

            // Setup UI options if we have them.
            UpdateResponses(tree.responseText, responses);

            // Hacky loop, runs once per frame to check input.
            while (true)
            {
                if (Input.GetButtonDown("MenuPrev"))
                {
                    // Select previous.
                    m_selectionIndex--;
                    UpdateResponses(tree.responseText, responses);
                }
                else if (Input.GetButtonDown("MenuNext"))
                {
                    // Select next.
                    m_selectionIndex++;
                    UpdateResponses(tree.responseText, responses);
                }
                else if (Input.GetButtonDown("Interact"))
                {
                    selectedOption = responses[m_selectionIndex];
                    break;
                }

                yield return null;
            }
        }
        Active = false;

        // Chain the next option.
        if (selectedOption is DialogOption option)
            yield return SelectOption(option, target);
    }

    private IEnumerator SelectOption( DialogOption option, GameObject target )
    {
        if (option.next is DialogTree tree)
        {
            yield return ShowDialog(tree, target);
        }
        else if (!string.IsNullOrEmpty( option.message ))
        {
            // Action target.
            Debug.Log($"send message {option.message} = {option.messageParam}");
            target.SendMessage(option.message, option.messageParam);
        }

        yield return null;
    }
}
