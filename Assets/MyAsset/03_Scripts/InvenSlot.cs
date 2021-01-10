using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//오브젝트 클릭이 아닌 UI 클릭과 관련된 함수.
public class InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // 슬롯의 인덱스
    [ShowOnly]
    public int m_SlotIndex;

    // 슬롯의 아이템
    [ShowOnly]
    public Item m_ItemInfo;

    [ShowOnly]
    public Image m_BGImage;
    [ShowOnly]
    public Image m_ItemImage;

    MouseManager M_Mouse;
    InventoryManager M_Inventory;

    private void Awake()
    {
        M_Mouse = MouseManager.Instance;
        M_Inventory = InventoryManager.Instance;

        m_BGImage = transform.FindChildren("BGImage").GetComponent<Image>();
        m_ItemImage = transform.FindChildren("ItemImage").GetComponent<Image>();

        OffItemImage();
    }

    public void OnItemImage()
    {
        m_ItemImage.ChangeAlpha(1);
    }
    public void OffItemImage()
    {
        m_ItemImage.ChangeAlpha(0);
    }

    public void OnPointerEnter(PointerEventData eventData)  //마우스가 위에 있을 시(실시간).
    {
        if (M_Inventory.m_ActivedSlot != this)
        {
            m_BGImage.color = M_Inventory.m_HighlightColor;
        }

        if (M_Mouse.GetState() == E_MouseState.NONE)
            M_Inventory.MouseOverSlot(this.gameObject);
    }
    public void OnPointerExit(PointerEventData eventData)   //마우스가 위에서 벗어날 시.
    {
        if (M_Inventory.m_ActivedSlot != this)
        {
            m_BGImage.color = M_Inventory.m_DefaultColor;
        }

        if (M_Mouse.GetState() == E_MouseState.NONE)
            M_Inventory.MouseExitSlot();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (M_Inventory.m_ActivedSlot != null)
            {
                // 아이템 끼리의 상호작용
                TestFunc();

                M_Inventory.m_ActivedSlot.m_BGImage.color = M_Inventory.m_DefaultColor;
                M_Inventory.m_ActivedSlot = null;
            }

            if (m_ItemInfo.m_Type != E_ItemType.None)
            {
                m_BGImage.color = M_Inventory.m_ActivedColor;

                M_Inventory.m_ActivedSlot = this;
            }
        }
        // 우클릭
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (m_ItemInfo.m_CanZoomIn)
            {
                for (int i = 0; i < M_Inventory.m_Canvas.transform.childCount; ++i)
                {
                    M_Inventory.m_Canvas.transform.GetChild(i).gameObject.SetActive(false);
                }

                m_BGImage.color = M_Inventory.m_DefaultColor;
                M_Inventory.m_ItemZoomIn.SetActive(true);

                SpriteRenderer itemImage = M_Inventory.m_ItemZoomIn.transform.FindChildren("ItemImage").GetComponent<SpriteRenderer>();
                itemImage.sprite = m_ItemImage.sprite;
            }
        }
    }

    public void TestFunc()
    {
        Item item1 = m_ItemInfo;
        Item item2 = M_Inventory.m_ActivedSlot.m_ItemInfo;

        if (M_Inventory.m_ActivedSlot == null ||
            item1.m_Type == E_ItemType.None ||
            item2.m_Type == E_ItemType.None)
            return;

        if (item1.m_Type != item2.m_Type)
        {
            if ((item1.m_Type == E_ItemType.Stage0_편지 ||
                item2.m_Type == E_ItemType.Stage0_편지) &&
                (item1.m_Type == E_ItemType.Stage0_성냥 ||
                item2.m_Type == E_ItemType.Stage0_성냥))
            {
                M_Inventory.PullSlotItem(m_SlotIndex);
                M_Inventory.PullSlotItem(M_Inventory.m_ActivedSlot.m_SlotIndex);

                M_Inventory.PushSlotItem(E_ItemType.Stage0_편지성냥사용);
            }
        }
    }
}
