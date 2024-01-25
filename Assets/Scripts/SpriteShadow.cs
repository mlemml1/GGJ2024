using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper tool to support giving sprites shadow effects in the 3d world.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteShadow : MonoBehaviour
{
    private SpriteRenderer m_renderer;
    public Material m_shadowMaterial;

    private void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();

        if (m_renderer != null)
        {
            m_renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            m_renderer.receiveShadows = true;
            m_renderer.allowOcclusionWhenDynamic = true;

            m_renderer.material = m_shadowMaterial;
        }
    }
}