using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogOption
{
    public string text;
    public DialogTree next;
    public string message;
    public string messageParam;
}

[CreateAssetMenu(fileName = "Dialog", menuName = "GGJ/DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    [Multiline]
    public string dialogText;

    public SpeechDef vox;
    public string responseText;
    public List<DialogOption> dialogResponses;
}
