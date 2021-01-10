using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum E_MouseState //마우스 동작상태 State.
{
    NONE,
    MOUSE_DOWN,
    MOUSE_UP
}

public class MouseManager : Singleton<MouseManager>
{
    [ReadOnly(true)]
    public Canvas m_Canvas;

    [ReadOnly, Tooltip("마우스 동작 관련 변수")]
    public E_MouseState m_State; //마우스 동작 상태.

    [ShowOnly]
    public GameObject m_SelectedObj;

    GraphicRaycaster m_GraphicRaycaster;

    public override void __Initialize()
    {
        m_GraphicRaycaster = m_Canvas.GetComponent<GraphicRaycaster>();
    }

    public E_MouseState GetState()
    {
        return m_State;
    }

    private void Update()
    {
        switch (m_State)
        {
            case E_MouseState.NONE:
                ISMouseDown();
                break;
            case E_MouseState.MOUSE_DOWN:
                ISMouseUP();
                break;
            case E_MouseState.MOUSE_UP:
                MouseUpEvent();
                break;
        }
    }

    // UI 클릭 확인 함수
    public bool UIRaycastWithTag(string tag)
    {
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> hits = new List<RaycastResult>();
        m_GraphicRaycaster.Raycast(ped, hits);

        foreach (var item in hits)
        {
            if (item.gameObject.CompareTag(tag))
            {
                m_SelectedObj = item.gameObject;
                return true;
            }
        }

        return false;
    }

    void ISMouseDown()
    {
        //pc 마우스 기준.
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("None -> Down");

            if (UIRaycastWithTag("InventorySlot"))
            {
                Debug.Log("인벤 슬롯");
            }

            m_State = E_MouseState.MOUSE_DOWN;
        }
    }
    void ISMouseUP()
    {
        //pc 마우스 기준.
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Down -> Up");

            m_State = E_MouseState.MOUSE_UP;
        }
    }
    void MouseUpEvent()
    {
        Debug.Log("Up -> None");

        m_State = E_MouseState.NONE;
    }

}
