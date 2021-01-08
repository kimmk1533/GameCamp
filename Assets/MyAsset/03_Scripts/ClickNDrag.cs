using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//오브젝트 클릭이 아닌 UI 클릭과 관련된 함수.
public class ClickNDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] int thisIndex; //슬롯의 고유 인덱스 번호(아이템 인덱스와는 별개).
    Image this_img;
    Image copydragslot_img;

    //get set
    public int GetThisSlotIndex()
    {
        return thisIndex;
    }
    public void SetThisSlotIndex(int _index)
    {
        thisIndex = _index;
    }
    public Image GetThisImage()
    {
        return this_img;
    }
    public void SetThisImage(Image _img)
    {
        this_img = _img;
    }
    public Image GetCopyDragSlotImage()
    {
        return copydragslot_img;
    }
    public void SetCopyDragSlotImage(Image _img)
    {
        copydragslot_img = _img;
    }

    //public void OnMouseEnter()  //마우스가 위에 있을 시.
    //{
    //    InventoryManager.Instance.MouseEnterSlot(this.transform.parent.gameObject);
    //}

    //public void OnMouseOver()   //마우스가 위에서 벗어날 시.
    //{
    //    InventoryManager.Instance.MouseOverSlot(this.transform.parent.gameObject);
    //}

    public void OnMouseDown()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData) //드래그 직전 함수.
    {
        if (MouseManager.Instance.GetState() == Mouse_State.MOUSE_DOWN)
        {
            MouseManager.Instance.SetState(Mouse_State.MOUSE_DRAG);
        }
    }

    public void FollowDragSlot()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(MouseManager.Instance.canvas.transform as RectTransform, Input.mousePosition, MouseManager.Instance.canvas.worldCamera, out pos);
        MouseManager.Instance.CopyDragSlot_obj.transform.position = MouseManager.Instance.canvas.transform.TransformPoint(pos);
    }
    public void OnDrag(PointerEventData eventData)  //드래그.
    {
        if (MouseManager.Instance.GetState() == Mouse_State.MOUSE_DRAG)
        {
            FollowDragSlot();
        }
    }

    public void OnEndDrag(PointerEventData eventData)   //드래그 해제.
    {
        if (MouseManager.Instance.GetBeingHitUIObj() != null)
        {
            //UI 마우스 드래그 업의 경우 실행되는 이벤트 관련은 여기 작성.
            if (MouseManager.Instance.GetState() == Mouse_State.MOUSE_DRAG)
            {
                if (MouseManager.Instance.Raycast2DHitObj() == MouseManager.Instance.GetBeingHitUIObj().transform.parent)
                {
                    MouseManager.Instance.UIClickUp();
                }
                else
                {
                    MouseManager.Instance.UIDragUp();
                }
            }

            MouseManager.Instance.SetState(Mouse_State.MOUSE_UP);
            MouseManager.Instance.SetBeingHitObj(null);
            MouseManager.Instance.SetBeingHitUIObj(null);
        }
    }
}
