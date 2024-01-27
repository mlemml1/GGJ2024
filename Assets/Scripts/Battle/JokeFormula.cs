using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class JokeFormula : MonoBehaviour
{
    public GameObject m_rowTemplate;
    private JokeDef m_joke;
    private List<JokeLine> m_jokeLines = new();
    private SaveGroup[] m_saveGroups;

    public JokeFormula()
    {
        m_saveGroups = new SaveGroup[9];
        for (int i = 0; i < m_saveGroups.Length; i++)
            m_saveGroups[i] = new SaveGroup(i);
    }

    private void Start()
    {
    }

    private void Update()
    {
        UpdateLineVisibility();
    }

    public void ClearJoke()
    {
        m_joke = null;
        foreach (var line in m_jokeLines)
            Destroy(line.gameObject);
        m_jokeLines.Clear();
    }

    public void SetJoke(JokeDef joke)
    {
        Assert.IsNotNull(m_saveGroups[0]);

        ClearJoke();
        m_joke = joke;

        var lines = m_joke.text.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            var box = Instantiate(m_rowTemplate, transform).GetComponent<JokeLine>();
            m_jokeLines.Add(box);
            box.gameObject.SetActive(false);
            box.BuildLine(line, m_saveGroups);
        }
    }

    public bool HasWordsUnfilled()
    {
        foreach (var line in m_jokeLines)
        {
            if (line.HasWordsUnfilled())
                return true;
        }

        return false;
    }

    public (WordType, WordCategory?) NextWord()
    {
        foreach (var line in m_jokeLines)
        {
            if (line.HasWordsUnfilled())
                return line.NextWord();
        }

        return ((WordType)(-1), null);
    }

    public void UpdateLineVisibility()
    {
        foreach (var line in m_jokeLines)
        {
            if (!line.gameObject.activeInHierarchy)
            {
                line.gameObject.SetActive(true);
                StartCoroutine(line.Display());
            }

            // Wait for the line to finish sliding in.
            if (!line.m_bVisible)
                break;

            // Last unfilled line is shown.
            if (line.HasWordsUnfilled())
                break;
        }
    }

    public bool DeckReady()
    {
        foreach (var line in m_jokeLines)
        {
            // Wait for the line to finish sliding in.
            if (!line.m_bVisible)
                return false;

            // Last unfilled line is shown.
            if (line.HasWordsUnfilled())
                break;
        }
        return true;
    }

    public void FillWord(WordDef word)
    {
        foreach (var line in m_jokeLines)
        {
            if (line.HasWordsUnfilled())
            {
                line.FillWord(word);
                break;
            }
        }

        UpdateLineVisibility();
    }
}
