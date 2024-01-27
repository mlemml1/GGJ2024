using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;
    public JokeManifest m_jokeManifest;
    public WordManifest m_wordManifest;
    // public List<WordDef> m_startingWords = new();

    private List<ItemDef> m_inventory = new();
    private List<WordDef> m_words = new();

    private HashSet<WordDef> m_verbs;
    private HashSet<WordDef> m_nouns;
    private HashSet<WordDef> m_adjs;
    private HashSet<WordDef> m_exts;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // m_words = new(m_startingWords);

        // Choose several random words.
        var verbs = m_wordManifest.words.Where(x => x.type == WordType.Verb).ToList();
        var nouns = m_wordManifest.words.Where(x => x.type == WordType.Noun).ToList();
        var adjs = m_wordManifest.words.Where(x => x.type == WordType.Adj).ToList();
        var exts = m_wordManifest.words.Where(x => x.type == WordType.Ext).ToList();

        Shuffle(verbs);
        Shuffle(nouns);
        Shuffle(adjs);
        Shuffle(exts);

        m_verbs = new(verbs);
        m_nouns = new(nouns);
        m_adjs = new(adjs);
        m_exts = new(exts);

        // Add 4 of each to the starting deck.
        m_words.AddRange(ChooseWords(m_verbs, 49));
        m_words.AddRange(ChooseWords(m_nouns, 49));
        m_words.AddRange(ChooseWords(m_adjs, 49));
        m_words.AddRange(ChooseWords(m_exts, 49));
    }

    public List<WordDef> ChooseWords(HashSet<WordDef> list, int nWords)
    {
        var o = new List<WordDef>();
        while (nWords > 0 && list.Count != 0)
        {
            --nWords;

            var word = list.First();
            list.Remove(word);
            o.Add(word);
        }
        return o;
    }

    // https://stackoverflow.com/questions/273313/randomize-a-listt
    private static System.Random rng = new();
    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
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

    public List<WordDef> GetWords(WordType type, WordCategory? cat)
    {
        if (cat is WordCategory category)
        {
            var found = m_words
                .Where(x => x.type == type && x.category == category)
                .ToList();

            Debug.Log($"found words of type {category}!");
            if (found.Count != 0)
                return found;
        }

        // Fallback if no words of that category.
        return m_words
            .Where(x => x.type == type)
            .ToList();
    }

    public List<JokeDef> GetJokes()
    {
        return new List<JokeDef>(m_jokeManifest.jokes);
    }
}
