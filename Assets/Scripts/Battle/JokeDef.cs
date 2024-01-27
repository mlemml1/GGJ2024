using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Joke", menuName = "GGJ/JokeDef", order = 6)]
public class JokeDef : ScriptableObject
{
    [Multiline]
    public string text;
}
