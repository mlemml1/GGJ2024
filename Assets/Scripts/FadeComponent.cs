using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created automatically by RoomVolume.
/// No more instancing/static batching. Too bad!
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class FadeComponent : MonoBehaviour
{
    private MeshRenderer m_mesh;
    private Material[] m_origMaterial;
    private Material[] m_cloneMaterial;

    // Start is called before the first frame update
    void Start()
    {
        m_mesh = GetComponent<MeshRenderer>();
        m_origMaterial = m_mesh.materials;

        m_cloneMaterial = new Material[m_origMaterial.Length];
        for (int i = 0; i < m_origMaterial.Length; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}