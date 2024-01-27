using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WordType
{
    Noun,
    Verb
}

public enum WordCategory
{
    // These are numbered based on relevance to each other.
    // Items further away give a better score.

    // Cat 1: things you'd find in a house
    HouseholdItem       = 1,
    Furniture           = 2,

    Lighting            = 5,

    // Cat 2: things you'd find on the street
    Transportation      = 10,
    Infrastructure      = 11,
    Building            = 12,
    Restaurant          = 15,

    // Cat 3: things you'd find in buildings on the street
    Groceries           = 20,


    // Cat 4: Technology
    ElectronicDevice    = 30,
    Appliance           = 31,

    // Cat 5: People
    Politics            = 40,
    Business            = 41,
    GameCharacter       = 42,

    // Cat 6: Entertainment
    Movies              = 50,
    Television          = 51,

    // Cat 7: Science
    Science             = 60,

    // Cat 8: Memes (high scores)
    Meme                = 100
}

[CreateAssetMenu(fileName = "Word", menuName = "GGJ/WordDef", order = 3)]
public class WordDef : ScriptableObject
{
    public WordType type;
    public WordCategory category;
    public string word;


    public static int CompareWordScore(WordCategory a, WordCategory b)
    {
        // Nothing new!
        if (a == b)
            return 0;

        // Critical hit!
        if (a == WordCategory.Meme || b == WordCategory.Meme)
            return 100;

        // Most buildings are unrelated.
        if (a == WordCategory.Building && b == WordCategory.Building)
            return 20;

        int score = Mathf.Abs((int)a - (int)b);

        if (score > 20)
            score = 20;

        return score;
    }
}
