using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WordManifest", menuName = "GGJ/WordManifest", order = 8)]
public class WordManifest : ScriptableObject
{ 
    public List<WordDef> words = new();

#if UNITY_EDITOR
    public bool regenManifest;

    void BuildManifest()
    {
        words.Clear();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Text/Words" });
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var word = AssetDatabase.LoadAssetAtPath<WordDef>(SOpath);
            if (word != null)
                words.Add(word);
        }
    }

    private void OnValidate()
    {
        if (regenManifest)
        {
            regenManifest = false;
            BuildManifest();
        }
    }
#endif // UNITY_EDITOR
}
