using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dialog", menuName = "GGJ/DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    [Multiline]
    public string dialogText;
}
