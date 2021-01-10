using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//오브젝트 클릭이 아닌 UI 클릭과 관련된 함수.
public class InvenSlot : MonoBehaviour, IPointerClickHandler
{
    // 슬롯의 인덱스
    [ShowOnly]
    public int m_SlotIndex;

    // 슬롯의 아이템
    [ShowOnly]
    public Item m_ItemInfo;

    [ShowOnly]
    public Image m_Image;

    MouseManager M_Mouse;

    private void Awake()
    {
        M_Mouse = MouseManager.Instance;

        m_Image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        M_Mouse.m_CurrentSlot = this;
    }
}
