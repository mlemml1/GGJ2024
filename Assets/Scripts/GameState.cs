using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;
    public JokeManifest m_jokeManifest;
    public List<WordDef> m_startingWords = new();

    private List<ItemDef> m_inventory = new();
    private List<WordDef> m_words = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        m_words = new(m_startingWords);
    }

    public void PickupItem( ItemDef item )
    {
        m_inventory.Add(item);
    }

    public void PickupWord( WordDef word )
    {
        if (m_words.Contains(word))
            return;

        m_words.Add( word );
    }

    public List<WordDef> GetWords(WordType type)
    {
        return m_words
            .Where(x => x.type == type)
            .ToList();
    }

    public List<JokeDef> GetJokes()
    {
        return new List<JokeDef>(m_jokeManifest.jokes);
    }
}
