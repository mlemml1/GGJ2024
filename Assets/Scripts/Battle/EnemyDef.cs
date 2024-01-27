using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "GGJ/EnemyDef", order = 5)]
public class EnemyDef : ScriptableObject
{
    public AnimatorController controller;
    public int maxHealth = 100;
    public int minDamage = 5;
    public int maxDamage = 10;
}
