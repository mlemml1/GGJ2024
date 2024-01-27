using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JokeManifest", menuName = "GGJ/JokeManifest", order = 7)]
public class JokeManifest : ScriptableObject
{
    public List<JokeDef> jokes;
}
