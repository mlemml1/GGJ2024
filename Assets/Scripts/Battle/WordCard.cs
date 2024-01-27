using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WordCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TextMeshProUGUI m_text;
    public WordDef m_word;

    private RectTransform m_transform;
    private bool m_bHasHover;

    private void Start()
    {
        m_bHasHover = false;
        m_transform = GetComponent<RectTransform>();
    }

    private Vector3 m_scaleVel;
    private void Update()
    {
        float targetScale = m_bHasHover ? 1.1f : 1.0f;

        m_transform.localScale = Vector3.SmoothDamp(
            m_transform.localScale,
            Vector3.one * targetScale,
            ref m_scaleVel,
            0.1f);

        var pos = m_transform.localPosition;
        pos.z = m_bHasHover ? 100 : 0;
        m_transform.localPosition = pos;
    }

    public void InitWord(WordDef word)
    {
        m_word = word;
        m_text.text = word.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log($"mouse enter {m_text.text}");

        m_bHasHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_bHasHover = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerController.Current.m_battleHud.UseCard(this);
    }
}
