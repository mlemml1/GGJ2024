using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public AudioSource m_battleMusic;
    public RectTransform m_wordDeck;
    public GameObject m_cardTemplate;
    public JokeFormula m_jokeBox;
    public Animator m_playerAnim;
    public Animator m_enemyAnim;
    public Slider m_myHealthBar;
    public Slider m_enemyHealthBar;

    private bool m_bInBattle = false;
    private bool m_bMyTurn = false;

    private List<JokeDef> m_unusedJokes;
    private PlayerController m_player;
    private EnemyDef m_enemy;
    private int m_enemyHealth;
    private int m_myHealth;

    WordDef m_lastWord = null;
    private int m_damageScore = 0;

    private List<WordCard> m_cards = new();

    void Start()
    {
    }

    void Update()
    {
        if (!m_bInBattle)
            return;

        UpdateHealth();

        m_jokeBox.gameObject.SetActive(m_bMyTurn);
        m_wordDeck.gameObject.SetActive(m_bMyTurn && m_jokeBox.DeckReady());
    }

    public void StartBattle(PlayerController player, EnemyDef enemy)
    {
        if (m_bInBattle)
            return;
        m_bInBattle = true;
        m_bMyTurn = true;

        m_player = player;
        m_enemy = enemy;

        m_myHealth = 100;
        m_enemyHealth = enemy.maxHealth;
        UpdateHealth(true);

        m_enemyAnim.runtimeAnimatorController = enemy.controller;

        m_unusedJokes = GameState.Instance.GetJokes();
        ChooseJoke();

        m_battleMusic.Play();
    }

    private void Win()
    {
        BattleOver(true);
    }

    private void Lose()
    {
        BattleOver(false);
    }

    private float m_myHealthVel;
    private float m_enemyHealthVel;
    private void UpdateHealth(bool snap = false)
    {
        float myTargetHealth = Mathf.Clamp((float)m_myHealth / 100.0f, 0, 1);
        float enemyTargetHealth = Mathf.Clamp((float)m_enemyHealth / (float)m_enemy.maxHealth, 0, 1);

        if (!snap)
        {
            m_myHealthBar.value = Mathf.SmoothDamp(m_myHealthBar.value, myTargetHealth, ref m_myHealthVel, 1.0f);
            m_enemyHealthBar.value = Mathf.SmoothDamp(m_enemyHealthBar.value, enemyTargetHealth, ref m_enemyHealthVel, 1.0f);
        }
        else
        {
            m_myHealthBar.value = myTargetHealth;
            m_enemyHealthBar.value = enemyTargetHealth;
            m_myHealthVel = 0;
            m_enemyHealthVel = 0;
        }
    }

    private void BattleOver(bool won)
    {
        if (!m_bInBattle)
            return;
        m_bInBattle = false;

        m_battleMusic.Stop();
        m_player?.EndBattle(won);
        m_player = null;
        m_enemy = null;
    }

    private void ChooseJoke()
    {
        if (m_unusedJokes.Count == 0)
        {
            // No jokes left. Oops!
            Win();
            return;
        }

        int nJoke = Random.Range(0, m_unusedJokes.Count);
        var joke = m_unusedJokes[nJoke];
        m_unusedJokes.RemoveAt(nJoke);

        // Don't reset the last word. That way we can chain nouns from different jokes.
        // m_lastWord = null;
        m_damageScore = 0;
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
            StartCoroutine(DoDamage());
            return;
        }
        
        var (nextWord, nextCat) = m_jokeBox.NextWord();
        BuildDeck(nextWord, nextCat);
    }

    private void BuildDeck( WordType type, WordCategory? cat, int nOptions = 3 )
    {
        // Get cards for this word.
        var allWords = GameState.Instance.GetWords( type, cat );

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
            // float along = ((float)(i + 0.5f) / (float)foundWords.Count);
            // float x = along * m_wordDeck.rect.width;

            var card = Instantiate(m_cardTemplate, m_wordDeck).GetComponent<WordCard>();
            m_cards.Add(card);

            //if (card.TryGetComponent<RectTransform>(out var tx))
            //    tx.position = new Vector3(x, tx.position.y, tx.position.z);

            card.InitWord(foundWords[i]);
            card.gameObject.SetActive(true);
        }
        // m_wordDeck
    }

    public void UseCard(WordCard card)
    {
        // The card was used. Fill the word.
        m_jokeBox.FillWord(card.m_word);

        if (card.m_word.type == WordType.Noun)
        {
            if (m_lastWord != null)
            {
                m_damageScore += WordDef.CompareWordScore(m_lastWord.category, card.m_word.category);
            }
            m_lastWord = card.m_word;
        }

        // Rebuild the deck.
        UpdateDeck();
    }

    private bool m_bDoingDamage = false;

    private IEnumerator DoDamage()
    {
        if (m_bDoingDamage)
            yield break;
        m_bDoingDamage = true;

        m_playerAnim.SetTrigger("attack");
        yield return new WaitForSeconds(1);
        m_enemyAnim.SetTrigger("damaged");

        m_enemyHealth -= m_damageScore;
        m_damageScore = 0;

        yield return new WaitForSeconds(1);

        if (m_enemyHealth < 0)
        {
            Win();
        }
        else
        {
            StartCoroutine(TheirTurn());
        }
        m_bDoingDamage = false;
    }

    private IEnumerator TheirTurn()
    {
        if (!m_bMyTurn)
            yield break;
        m_bMyTurn = false;

        m_enemyAnim.SetTrigger("attack");
        yield return new WaitForSeconds(1);
        m_playerAnim.SetTrigger("damaged");

        int damage = Random.Range(m_enemy.minDamage, m_enemy.maxDamage);
        m_myHealth -= damage;

        yield return new WaitForSeconds(1);

        if (m_myHealth < 0)
            Lose();
        else
        {
            ChooseJoke();
            m_bMyTurn = true;
        }
    }

}
