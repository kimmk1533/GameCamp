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
    Color32 defalut_color = new Color32(255, 255, 255, 255);
    Color32 noAlpha_color = new Color32(255, 255, 255, 0);

    //get set
    public int GetThisSlotIndex()
    {
        return thisIndex;
    }
    public void SetThisSlotIndex(int _index)
    {
        thisIndex = _index;
    }

    //public void OnMouseEnter()  //마우스가 위에 있을 시.
    //{
    //    InventoryManager.Instance.MouseEnterSlot(this.transform.parent.gameObject);
    //}

    //public void OnMouseOver()   //마우스가 위에서 벗어날 시.
    //{
    //    InventoryManager.Instance.MouseOverSlot(this.transform.parent.gameObject);
    //}

    public void OnBeginDrag(PointerEventData eventData) //클릭 다운.
    {
        if (InventoryManager.Instance.GetSlotItem(thisIndex).index != 0)
        {
            MouseManager.Instance.SetState(Mouse_State.MOUSE_DOWN);
            MouseManager.Instance.SetBeingHitUIObj(gameObject);
            MouseManager.Instance.SetBeingHitObj(MouseManager.Instance.CopyDragSlot_obj);
            //Image 연결.
            this_img = gameObject.GetComponent<Image>();
            copydragslot_img = MouseManager.Instance.CopyDragSlot_obj.GetComponent<Image>();
            //스프라이트 값 설정.
            this_img.color = noAlpha_color;
            this_img.sprite = InventoryManager.Instance.GetSlotItem(thisIndex).Image;
            copydragslot_img.sprite = this_img.sprite;
            copydragslot_img.color = defalut_color;
        }
    }

    public void OnDrag(PointerEventData eventData)  //드래그.
    {
        if (MouseManager.Instance.GetState() == Mouse_State.MOUSE_DOWN)
        {
            MouseManager.Instance.SetState(Mouse_State.MOUSE_DRAG);
        }
        if (MouseManager.Instance.GetState() == Mouse_State.MOUSE_DRAG)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MouseManager.Instance.canvas.transform as RectTransform, Input.mousePosition, MouseManager.Instance.canvas.worldCamera, out pos);
            MouseManager.Instance.CopyDragSlot_obj.transform.position = MouseManager.Instance.canvas.transform.TransformPoint(pos);
        }
    }

    public void OnEndDrag(PointerEventData eventData)   //드래그 해제.
    {
        if (MouseManager.Instance.GetState() == Mouse_State.MOUSE_DRAG)
        {
            MouseManager.Instance.SetState(Mouse_State.MOUSE_UP);
            MouseManager.Instance.SetBeingHitObj(null);
            MouseManager.Instance.SetBeingHitUIObj(null);

            //스프라이트 값 설정.
            this_img.color = defalut_color;
            copydragslot_img.sprite = null;
            copydragslot_img.color = noAlpha_color;
            MouseManager.Instance.CopyDragSlot_obj.transform.localPosition = Vector3.zero;

            //UI 마우스 업의 경우 실행되는 이벤트 관련은 여기 작성.
            //인벤토리 클릭과 드래그 구분은 충돌 체크를 해서 구별하면 될듯 함(어차피 오브젝트랑 상호작용을 하려면 기능을 만들어야하니).
            Debug.Log("UI 클릭");
        }
    }
}
