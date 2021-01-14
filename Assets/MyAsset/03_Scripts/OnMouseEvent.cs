using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnMouseEvent : MonoBehaviour
{
    public UnityEvent m_OnMouseEnterEvent;
    public UnityEvent m_OnMouseExitEvent;

    private void OnMouseEnter()
    {
        m_OnMouseEnterEvent?.Invoke();
    }
    private void OnMouseExit()
    {
        m_OnMouseExitEvent?.Invoke();
    }
}
