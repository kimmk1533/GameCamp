using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Mouse_State //마우스 동작상태 State.
{
    NULL,
    MOUSE_DOWN,
    MOUSE_DRAG,
    MOUSE_UP
}

public class MouseManager : Singleton<MouseManager>
{
    public Canvas canvas;
    public GameObject CopyDragSlot_obj; //선택한 아이템의 아이콘을 복사해 드래그에 사용하는 오브젝트.

    [Space(10f)]
    [Tooltip("마우스 동작 관련 변수")]
    [SerializeField] Mouse_State state; //마우스 동작 상태.
    [SerializeField] GameObject beingHit_obj;   //현재 동작 연결 상태인 오브젝트.
    [SerializeField] GameObject beingHitUI_obj;   //(UI 선택 시) 실제 동작 연결 상태인 오브젝트 저장.

    public Color32 defalut_color = new Color32(255, 255, 255, 255);
    public Color32 noAlpha_color = new Color32(255, 255, 255, 0);

    __GameManager M_Game;
    InventoryManager M_Inventory;
    StageManager M_Stage;

    public override void __Initialize()
    {
        M_Game = __GameManager.Instance;
        M_Inventory = InventoryManager.Instance;
        M_Stage = StageManager.Instance;
    }

    //get set
    public Mouse_State GetState()
    {
        return state;
    }
    public void SetState(Mouse_State _state)
    {
        state = _state;
    }
    public GameObject GetBeingHitObj()
    {
        return beingHit_obj;
    }
    public void SetBeingHitObj(GameObject _hit)
    {
        beingHit_obj = _hit;
    }
    public GameObject GetBeingHitUIObj()
    {
        return beingHitUI_obj;
    }
    public void SetBeingHitUIObj(GameObject _hit)
    {
        beingHitUI_obj = _hit;
    }

    private void Update()
    {
        switch (state)
        {
            case Mouse_State.NULL:
                ISMouseDown();
                break;
            case Mouse_State.MOUSE_DOWN:
                ISMouseUP();
                break;
            case Mouse_State.MOUSE_UP:
                MouseUpEvent();
                break;
        }
    }

    //오브젝트 클릭 관련 함수(UI클릭 관련은 ClickNDrag 스크립트 참조.
    public RaycastHit2D Raycast2DHitObj()  //마우스 충돌 오브젝트 레이캐스트 체크(return RaycastHit2D hit).
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        Vector3 cameraPos = M_Game.maincamera.transform.position; //카메라 좌표 변환.
        if (hit.collider != null)   //콜라이더 충돌 시.
        {
            beingHit_obj = hit.collider.gameObject;
        }
        return hit;
    }

    void ISMouseDown()
    {
        //pc 마우스 기준.
        if (Input.GetMouseButtonDown(0))
        {
            M_Inventory.MouseExitSlot();
            if (Raycast2DHitObj().collider != null)
            {
                SetState(Mouse_State.MOUSE_DOWN);
                if (beingHit_obj.tag == "InventorySlot")
                {
                    ClickNDrag tmp = beingHit_obj.GetComponent<ClickNDrag>();
                    if (M_Inventory.GetSlotItem(tmp.GetThisSlotIndex()).m_Type != E_ItemType.None)
                    {
                        //Image 연결.
                        tmp.SetThisImage(tmp.gameObject.GetComponent<Image>());
                        tmp.SetCopyDragSlotImage(Instance.CopyDragSlot_obj.GetComponent<Image>());
                        //스프라이트 값 설정.
                        tmp.GetThisImage().color = noAlpha_color;
                        tmp.GetThisImage().sprite = M_Inventory.GetSlotItem(tmp.GetThisSlotIndex()).m_Image;
                        tmp.GetCopyDragSlotImage().sprite = tmp.GetThisImage().sprite;
                        tmp.GetCopyDragSlotImage().color = defalut_color;

                        SetState(Mouse_State.MOUSE_DOWN);
                        SetBeingHitUIObj(tmp.gameObject);
                        SetBeingHitObj(Instance.CopyDragSlot_obj);
                        tmp.FollowDragSlot();
                    }
                    else
                    {
                        SetState(Mouse_State.NULL);
                    }
                }
            }
        }
    }
    void ISMouseUP()
    {
        //pc 마우스 기준.
        if (Input.GetMouseButtonUp(0))
        {
            if (beingHit_obj != null)
            {
                if (beingHitUI_obj != null)    //UI 클릭.
                {
                    if (beingHitUI_obj.tag == "InventorySlot") //인벤토리 아이템 클릭.
                    {
                        UIClickUp();
                    }
                }
                else
                {
                    ObjectClickUp();
                }

                state = Mouse_State.MOUSE_UP;
            }
        }
    }
    void MouseUpEvent() //UI든 오브젝트든 상관없이 동일하게 실행 시 이용하는 함수.
    {
        //여기서 조진서가 만든 박스콜라이더 2d충돌 이벤트를 체크해 실행할 예정.

        Debug.Log("이벤트");
        beingHit_obj = null;
        beingHitUI_obj = null;
        state = Mouse_State.NULL;
    }


    //클릭 이후 이벤트 함수.
    void ObjectClickUp()  //오브젝트 마우스 업의 경우 실행되는 이벤트 관련은 여기 작성.
    {

        if (beingHit_obj != null)
        {
            if (beingHit_obj.tag == "only sound")
            {
                AudioSource audio = beingHit_obj.GetComponent<AudioSource>();
                audio.Play();
            }

            //else if (beingHit_obj.tag == "Item")
            //{
            //    if (beingHit_obj.name == "Selectable(Matches)")
            //    {
            //        M_Inventory.PushSlotItem((int)E_ItemType.열쇠);
            //    }
            //    else if (beingHit_obj.name == "Selectable(Key)")
            //    {
            //        M_Inventory.PushSlotItem((int)E_ItemType.성냥곽);
            //        M_Stage.stage_zero.havekey = true;                  
            //    }
            //}

            else if (beingHit_obj.tag == "term sound")
            {
                switch (beingHit_obj.name)
                {
                    case "Selectable":
                        beingHit_obj.GetComponent<SelectableObject>().m_Enable = M_Stage.stage_zero.havekey;

                        if (beingHit_obj.GetComponent<SelectableObject>().m_Enable == true)
                        {
                            M_Inventory.PullSlotItem(3);
                            AudioSource audio = beingHit_obj.GetComponent<AudioSource>();
                            audio.Play();
                            SelectableObject fade = beingHit_obj.GetComponent<SelectableObject>();
                            fade.DoAction();
                        }
                        break;

                    default:
                        break;
                }

            }
        }

        Debug.Log(beingHit_obj.name);
    }
    public void UIClickUp()
    {
        DragSlotItemGoBack();
        Debug.Log("UI 클릭 업");
    }
    public void UIDragUp()
    {
        DragSlotItemGoBack();
        //Instance.Raycast2DHitObj()로 충돌한 콜라이더를 판별해 이벤트 실행.
        Debug.Log("UI 드래그 업");
    }

    void DragSlotItemGoBack()
    {
        ClickNDrag tmp = beingHitUI_obj.GetComponent<ClickNDrag>();
        //Image 연결.
        tmp.SetThisImage(tmp.gameObject.GetComponent<Image>());
        tmp.SetCopyDragSlotImage(CopyDragSlot_obj.GetComponent<Image>());
        //스프라이트 값 설정.
        tmp.GetThisImage().color = defalut_color;
        tmp.GetCopyDragSlotImage().sprite = null;
        tmp.GetCopyDragSlotImage().color = noAlpha_color;
        CopyDragSlot_obj.transform.localPosition = Vector3.zero;
    }
}
