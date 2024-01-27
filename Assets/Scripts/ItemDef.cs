using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "GGJ/ItemDef", order = 4)]
public class ItemDef : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
}
