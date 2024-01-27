using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    public RectTransform m_wordDeck;
    public GameObject m_cardTemplate;
    public JokeFormula m_jokeBox;

    private List<JokeDef> m_unusedJokes;
    private PlayerController m_player;
    private EnemyDef m_enemy;
    private List<WordCard> m_cards = new();

    void Start()
    {
        m_cardTemplate.SetActive(false);
    }

    void Update()
    {
        
    }

    public void StartBattle(PlayerController player, EnemyDef enemy)
    {
        m_player = player;
        m_enemy = enemy;

        m_unusedJokes = GameState.Instance.GetJokes();
        ChooseJoke();
    }

    private void BattleOver()
    {
        m_player?.EndBattle();
        m_player = null;
        m_enemy = null;
    }

    private void ChooseJoke()
    {
        if (m_unusedJokes.Count == 0)
        {
            // No jokes left. Oops!
            BattleOver();
            return;
        }

        int nJoke = Random.Range(0, m_unusedJokes.Count);
        var joke = m_unusedJokes[nJoke];
        m_unusedJokes.RemoveAt(nJoke);

        m_jokeBox.SetJoke(joke);
        UpdateDeck();
    }

    private void ClearCards()
    {
        foreach (var card in m_cards)
            Destroy(card.gameObject);
        m_cards.Clear();
    }

    private void UpdateDeck()
    {
        ClearCards();

        // What's next up?
        if (!m_jokeBox.HasWordsUnfilled())
        {
            // Joke finished. Do damage.
            return;
        }
        
        WordType nextWord = m_jokeBox.NextWord();
        BuildDeck(nextWord);
    }

    private void BuildDeck( WordType type, int nOptions = 3 )
    {
        // Get cards for this word.
        var allWords = GameState.Instance.GetWords( type );

        List<WordDef> foundWords = new();
        for (int i = 0; i < nOptions && allWords.Count != 0; i++)
        {
            // Pick a random word from the set.
            int nWord = Random.Range(0, allWords.Count);
            foundWords.Add(allWords[nWord]);
            allWords.RemoveAt(nWord); // meh
        }


        Debug.Log($"building deck with {foundWords.Count} words");

        // Use our words to build the card deck.
        for (int i = 0; i < foundWords.Count; i++)
        {
            float along = ((float)(i + 0.5f) / (float)foundWords.Count);
            float x = along * m_wordDeck.rect.width;

            var card = Instantiate(m_cardTemplate, m_wordDeck).GetComponent<WordCard>();
            m_cards.Add(card);

            if (card.TryGetComponent<RectTransform>(out var tx))
                tx.position = new Vector3(x, tx.position.y, tx.position.z);

            card.InitWord(foundWords[i]);
            card.gameObject.SetActive(true);
        }
        // m_wordDeck
    }

    public void UseCard(WordCard card)
    {
        // The card was used. Fill the word.
        m_jokeBox.FillWord(card.m_word);

        // Rebuild the deck.
        UpdateDeck();
    }
}
