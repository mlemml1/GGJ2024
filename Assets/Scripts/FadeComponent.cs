using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Created automatically by RoomVolume.
/// No more instancing/static batching. Too bad!
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class FadeComponent : MonoBehaviour
{
    private MeshRenderer m_mesh;
    private Color[] m_matColor;

    //private Material[] m_cloneMaterial;

    // Start is called before the first frame update
    void Start()
    {
        m_mesh = GetComponent<MeshRenderer>();
        if (m_mesh == null)
            return;

        // m_origMaterial = m_mesh.materials;

        m_matColor = new Color[m_mesh.materials.Length];
        for (int i = 0; i < m_mesh.materials.Length; i++)
        {
            m_matColor[i] = m_mesh.materials[i].color;
        }
    }

    public void SetOpacity(float opacity)
    {
        if (m_mesh == null)
            return;


        var materials = m_mesh.materials;

        for (int i = 0; i < m_matColor.Length; i++)
        {
            materials[i].color = m_matColor[i] * opacity;
        }
    }
}
