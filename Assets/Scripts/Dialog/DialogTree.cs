using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogOption
{
    public string text;
    public Object next;
    public string nextParam;
}

[CreateAssetMenu(fileName = "Dialog", menuName = "GGJ/DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    [Multiline]
    public string dialogText;

    public List<DialogOption> dialogResponses;
}
