using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speech", menuName = "GGJ/SpeechDef", order = 2)]
public class SpeechDef : ScriptableObject
{
    public AudioClip voxSfx;

    [Range(0, 0.25f)]
    public float warbleScale = 0.0f;
}
