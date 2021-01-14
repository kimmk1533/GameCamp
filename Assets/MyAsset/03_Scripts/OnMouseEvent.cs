using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnMouseEvent : MonoBehaviour
{
    public E_ItemType m_RequireItem;

    public UnityEvent m_OnMouseEnterEvent;

    InventoryManager M_Inven;

    private void Awake()
    {
        M_Inven = InventoryManager.Instance;
    }

    private void OnMouseEnter()
    {
        if (M_Inven.m_ActivedSlot != null &&
            M_Inven.m_ActivedSlot.m_ItemInfo.m_Type == m_RequireItem)
        {
            m_OnMouseEnterEvent?.Invoke();
        }
    }
}