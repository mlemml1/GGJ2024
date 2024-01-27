using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SaveGroup
{
    public int id;
    public string saved;
    public delegate void OnSet(string value);

    public OnSet onSet;

    public SaveGroup( int id )
    {
        this.id = id;
    }
}

public class JokeLine : MonoBehaviour
{
    public GameObject m_wordTemplate;
    public bool m_bVisible = false;

    private string text;
    private LinkedList<(WordType, WordCategory?, TextMeshProUGUI, SaveGroup)> m_remainingWords = new();
    private List<TextMeshProUGUI> m_wordBoxes = new();

    public IEnumerator Display()
    {
        yield return null;

        // Rebuild the layout to get us in the right place.
        var group = GetComponentInParent<VerticalLayoutGroup>();
        LayoutRebuilder.MarkLayoutForRebuild(group.transform as RectTransform);

        var image = GetComponent<Image>();
        for (int i = 0; i <= 100; i++)
        {
            // Update transparency.
            float opacity = (float)i / 100.0f;

            image.color = image.color.WithAlpha(opacity);

            foreach (var word in m_wordBoxes)
                word.color = word.color.WithAlpha(opacity);

            // transform.position = new Vector3(i, transform.position.y, transform.position.z);
            yield return null;
        }

        // Let it sink in.
        yield return new WaitForSeconds(1);

        m_bVisible = true;
    }

    public void BuildLine(string line, SaveGroup[] saveGroups)
    {
        // Split into words.
        var re = new Regex(@"(?:\([^)]+\)|[\w']+|[^\s])");

        // Add a textbox for each word.
        var matches = re.Matches(line);
        foreach (var word in matches)
            AddWord(word.ToString().Trim(), saveGroups);
    }

    private void AddWord(string text, SaveGroup[] saveGroups)
    {
        var box = Instantiate(m_wordTemplate, this.transform).GetComponent<TextMeshProUGUI>();
        m_wordBoxes.Add(box);

        Debug.Log($"add word '{text}'");
        // Setup text.
        if (text.StartsWith("(noun"))
        {
            WordCategory? requireCat = null;
            SaveGroup saveGroup = null;
            int tagIdx = text.IndexOf(":");
            if (tagIdx > 0)
            {
                var tags = text.Substring(tagIdx + 1).TrimEnd(")").Split(",");

                foreach (var tag in tags)
                {
                    // Is it a save group?
                    if (tag.Length == 2 && tag[0] == 's' && tag[1] >= '0' && tag[1] <= '9')
                    {
                        int index = tag[1] - '0';
                        Debug.Log($"setting save group {index}");
                        saveGroup = saveGroups[index];
                        continue;
                    }

                    // Is it a category?
                    foreach (var cat in Enum.GetNames(typeof(WordCategory)))
                    {
                        if (tag.StartsWith(cat, StringComparison.OrdinalIgnoreCase))
                        {
                            Debug.Log($"found tag category {cat}!");
                            requireCat = Enum.Parse<WordCategory>(cat);
                            break;
                        }
                    }
                }
            }

            box.text = "____";
            m_remainingWords.AddLast((WordType.Noun, requireCat, box, saveGroup));
        }
        else if (text == "(verb)")
        {
            box.text = "____";
            m_remainingWords.AddLast((WordType.Verb, null, box, null));
        }
        else if (text == "(adj)")
        {
            box.text = "____";
            m_remainingWords.AddLast((WordType.Adj, null, box, null));
        }
        else if (text == "(ext)")
        {
            box.text = "____";
            m_remainingWords.AddLast((WordType.Ext, null, box, null));
        }
        else if (text.StartsWith("(s"))
        {
            int groupId = int.Parse(text.Substring(2, 1));
            // Debug.Log($"loading save group {groupId} ({text.Substring(2, 1)})");
            // box.text = saveGroups[groupId].saved;
            saveGroups[groupId].onSet += (name) => { box.text = name; };
        }
        else
        {
            box.text = $"{text} ";
        }
    }

    public bool HasWordsUnfilled()
    {
        return m_remainingWords.Count != 0;
    }

    public (WordType, WordCategory?) NextWord()
    {
        if (m_remainingWords.Count == 0)
            return ((WordType)(-1), null);

        var (type, cat, box, group) = m_remainingWords.First.Value;
        return (type, cat);
    }

    public void FillWord(WordDef word)
    {
        if (m_remainingWords.Count == 0)
            return;

        var (type, cat, box, group) = m_remainingWords.First.Value;
        m_remainingWords.RemoveFirst();

        Assert.IsTrue(word.type == type);
        box.text = $"{word.name}";
        if (group != null)
        {
            group.saved = word.name;
            group.onSet(word.name);
            Debug.Log($"Saved {word.name} to group {group.id}");
        }

        box.color = Color.yellow;
    }
}