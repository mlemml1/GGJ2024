using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class JokeLine : MonoBehaviour
{
    public GameObject m_wordTemplate;
    public bool m_bVisible = false;

    private string text;
    private LinkedList<(WordType, TextMeshProUGUI)> m_remainingWords = new();
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

    public void BuildLine(string line)
    {
        // Split into words.
        var re = new Regex(@"(?:\(noun\)|\(verb\)|[\w']+|[^\s])");

        // Add a textbox for each word.
        var matches = re.Matches(line);
        foreach (var word in matches)
            AddWord(word.ToString().Trim());
    }

    private void AddWord(string text)
    {
        var box = Instantiate(m_wordTemplate, this.transform).GetComponent<TextMeshProUGUI>();
        m_wordBoxes.Add(box);

        // Setup text.
        if (text == "(noun)")
        {
            box.text = "____ ";
            m_remainingWords.AddLast((WordType.Noun, box));
        }
        else if (text == "(verb)")
        {
            box.text = "____ ";
            m_remainingWords.AddLast((WordType.Verb, box));
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

    public WordType NextWord()
    {
        if (m_remainingWords.Count == 0)
            return (WordType)(-1);

        return m_remainingWords.First.Value.Item1;
    }

    public void FillWord(WordDef word)
    {
        if (m_remainingWords.Count == 0)
            return;

        var (type, box) = m_remainingWords.First.Value;
        m_remainingWords.RemoveFirst();

        Assert.IsTrue(word.type == type);
        box.text = $"{word.name} ";
        box.color = Color.yellow;
    }
}