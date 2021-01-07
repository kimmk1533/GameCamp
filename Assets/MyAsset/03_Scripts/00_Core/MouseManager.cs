using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void __Initialize()
    {
        
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
        switch(state)
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
    RaycastHit2D Raycast2DHitObj()  //마우스 충돌 오브젝트 레이캐스트 체크(return RaycastHit2D hit).
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        Vector3 cameraPos = Camera.main.transform.position; //카메라 좌표 변환.
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
            if (Raycast2DHitObj().collider != null)
            {
                state = Mouse_State.MOUSE_DOWN;
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
                beingHit_obj = null;
                state = Mouse_State.MOUSE_UP;

                //오브젝트 마우스 업의 경우 실행되는 이벤트 관련은 여기 작성.
                Debug.Log("오브젝트 클릭");
            }
        }
    }
    void MouseUpEvent() //UI든 오브젝트든 상관없이 동일하게 실행 시 이용하는 함수.
    {
        //여기서 조진서가 만든 박스콜라이더 2d충돌 이벤트를 체크해 실행할 예정.
        Debug.Log("UI나 오브젝트 클릭");
        state = Mouse_State.NULL;
    }
}
