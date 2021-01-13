using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum E_FadeType
{
    NoneFade,
    Fade,
}

[System.Flags]
public enum E_SelectableObjectConditionType
{
    HasItem = 1 << 0,
    ActiveItem = 1 << 1,
    SameImage = 1 << 2,
}

[System.Flags]
public enum E_SelectableObjectActionType
{
    SetActive = 1 << 0,
    MoveCamera = 1 << 1,
    ChangeImage = 1 << 2,
    AddItem = 1 << 3,
    RemoveItem = 1 << 4,
    PlaySound = 1 << 5,
}

public class SelectableObject : MonoBehaviour, IPointerClickHandler
{
    // 스크립트 활성화 여부
    public bool m_Enable;

    #region 클릭됐을 때 할 액션

    #region Types
    public E_SelectableObjectConditionType m_ConditionType;
    public E_FadeType m_FadeType;
    public E_SelectableObjectActionType m_ActionType;
    #endregion

    #region Conditions

    // HasItem
    public List<E_ItemType> m_RequireItems;

    // ActiveItem
    public E_ItemType m_RequireItem;

    // SameImage
    public Image m_CheckImage;
    public SpriteRenderer m_CheckRenderer;
    public Sprite m_CheckSprite;

    #endregion

    #region Actions

    #region SetActive
    public List<bool> m_Actives;
    public List<GameObject> m_Objects;
    #endregion

    #region MoveCamera
    // 현재 방향으로 이동
    public bool m_DirectionMove;
    // 이전 위치로 이동
    public bool m_MoveToPreviousPos;
    // 이동할 위치
    public Vector3 m_Position;
    // 이전 위치
    static Vector3 m_LastPos;
    #endregion

    #region ChangeSprite
    public bool m_IsOnce;
    public Sprite m_Sprite;
    public Image m_Image;
    public SpriteRenderer m_Renderer;
    #endregion

    #region AddItem
    public List<E_ItemType> m_AddItemTypes;
    #endregion

    #region RemoveItem
    public List<E_ItemType> m_RemoveItemTypes;
    #endregion

    #region PlaySound
    public List<float> m_AudioStartSeconds;
    public List<AudioSource> m_Audios;
    #endregion

    #endregion

    // 화살표 표시 여부
    public bool m_DirActive;

    // 4방향 화살표
    public bool m_LeftActive;
    public bool m_RightActive;
    public bool m_UpActive;
    public bool m_DownActive;

    #endregion

    // 액션 시작 시 호출할 이벤트
    public UnityEvent m_StartEvent;
    // 액션 종료 시 호출할 이벤트
    public UnityEvent m_EndEvent;

    // 카메라가 이동할 위치
    Vector3 m_Pos;
    // 메인 카메라
    Camera m_Camera;

    // 매니져
    DirectionManager M_Direction;
    InventoryManager M_Inventory;

    private void Awake()
    {
        __Initialize();
    }

    public void __Initialize()
    {
        // SetActive
        // 첫 번째 오브젝트가 null값이면 자기 자신을 추가
        if (m_Objects.Count == 0)
            m_Objects.Add(this.gameObject);
        else if (m_Objects[0] == null)
            m_Objects[0] = this.gameObject;

        // ChangeImage
        // 렌더러가 없으면 자기 자신의 렌더러를 가져옴
        if (m_Renderer == null)
            m_Renderer = GetComponent<SpriteRenderer>();

        // PlaySound
        // 첫 번째 오디오소스가 null값이면 자기 자신을 추가
        if (m_Audios.Count == 0)
            m_Audios.Add(this.GetComponent<AudioSource>());
        else if (m_Audios[0] == null)
            m_Audios[0] = this.GetComponent<AudioSource>();

        // 카메라, 카메라가 이동할 위치 설정
        m_Camera = Camera.main;
        m_Pos = m_Position;
        m_Pos.z += m_Camera.transform.position.z;

        // 매니져 가져오기
        M_Direction = DirectionManager.Instance;
        M_Inventory = InventoryManager.Instance;
    }

    // =========================================================================

    // 일반 오브젝트가 클릭됐을 때
    public void OnMouseUp()
    {
        // 마우스 위치에 UI와 오브젝트가 겹쳐있지 않다면
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            DoAction();
        }
    }

    // UI 오브젝트가 클릭됐을 때
    public void OnPointerClick(PointerEventData eventData)
    {
        // 액션 실행
        DoAction();
    }

    // =========================================================================

    // 좌측 화살표용 왼쪽 회전
    public void TurnLeft()
    {
        // 방향 회전
        M_Direction.TurnLeft();

        // 페이드가 있을 경우
        if (m_FadeType == E_FadeType.Fade)
        {
            // 페이드 액션 이벤트에 추가
            Fade.FadeAction += M_Direction.CameraMoveToDir;
        }
        // 페이드가 없을 경우
        else if (m_FadeType == E_FadeType.NoneFade)
        {
            // 액션 실행
            M_Direction.CameraMoveToDir();
        }
    }
    // 우측 화살표용 오른쪽 회전
    public void TurnRight()
    {
        // 방향 회전
        M_Direction.TurnRight();

        // 페이드가 있을 경우
        if (m_FadeType == E_FadeType.Fade)
        {
            // 페이드 액션 이벤트에 추가
            Fade.FadeAction += M_Direction.CameraMoveToDir;
        }
        // 페이드가 없을 경우
        else if (m_FadeType == E_FadeType.NoneFade)
        {
            // 액션 실행
            M_Direction.CameraMoveToDir();
        }
    }
    // 하단 화살표용 이전 위치로 이동
    public void TurnBack()
    {
        // 이동할 위치를 이전 위치로 변경
        m_Pos = m_LastPos;
    }

    // 시작 이벤트 실행 함수
    public void DoStartEvent()
    {
        m_StartEvent?.Invoke();
    }
    // 끝 이벤트 실행 함수
    public void DoEndEvent()
    {
        m_EndEvent?.Invoke();
    }

    // 액션 실행 함수
    public void DoAction()
    {
        // 스크립트가 활성화 되어있을 때
        if (m_Enable)
        {
            //// 조건 없음
            //if (m_ConditionType == E_SelectableObjectConditionType.None)
            //{
            //    // 액션 실행
            //    Action();
            //}
            //// HasItem 조건
            //else if (m_ConditionType == E_SelectableObjectConditionType.HasItem)
            //{
            //    bool flag = true;

            //    for (int i = 0; i < m_RequireItems.Count; ++i)
            //    {
            //        if (!M_Inventory.HasItem(m_RequireItems[i]))
            //        {
            //            flag = false;
            //            break;
            //        }
            //    }

            //    if (flag)
            //    {
            //        // 액션 실행
            //        Action();
            //    }
            //}
            //// ActiveItem 조건
            //else if (m_ConditionType == E_SelectableObjectConditionType.ActiveItem)
            //{
            //    if (M_Inventory.m_ActivedSlot != null &&
            //        m_RequireItem == M_Inventory.m_ActivedSlot.m_ItemInfo.m_Type)
            //    {
            //        M_Inventory.UseItem(m_RequireItem);
            //        // 액션 실행
            //        Action();
            //    }
            //}
            //else if (m_ConditionType == E_SelectableObjectConditionType.SameImage)
            //{
            //    if (m_CheckImage.sprite == m_CheckSprite)
            //    {
            //        // 액션 실행
            //        Action();
            //    }
            //}

            List<bool> Flags = new List<bool>();

            // 조건 없음
            if (m_ConditionType == 0)
            {
                Flags.Add(true);
            }
            else
            {
                // HasItem 조건
                if (m_ConditionType.HasFlag(E_SelectableObjectConditionType.HasItem))
                {
                    bool temp = true;

                    for (int i = 0; i < m_RequireItems.Count; ++i)
                    {
                        if (!M_Inventory.HasItem(m_RequireItems[i]))
                        {
                            temp = false;
                            break;
                        }
                    }

                    Flags.Add(temp);
                }
                // ActiveItem 조건
                if (m_ConditionType.HasFlag(E_SelectableObjectConditionType.ActiveItem))
                {
                    Flags.Add(M_Inventory.m_ActivedSlot != null &&
                              m_RequireItem == M_Inventory.m_ActivedSlot.m_ItemInfo.m_Type);
                }
                // SameImage 조건
                if (m_ConditionType.HasFlag(E_SelectableObjectConditionType.SameImage))
                {
                    if (m_CheckImage != null)
                    {
                        Flags.Add(m_CheckImage.sprite == m_CheckSprite);
                    }
                    if (m_CheckRenderer != null)
                    {
                        Flags.Add(m_CheckRenderer.sprite == m_CheckSprite);
                    }
                }
            }

            bool flag = true;

            for (int i = 0; i < Flags.Count; ++i)
            {
                if (!Flags[i])
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                if (m_ConditionType.HasFlag(E_SelectableObjectConditionType.ActiveItem))
                {
                    M_Inventory.UseItem(m_RequireItem);
                }

                Action();
            }
        }
    }
    // 실제 액션 함수
    void Action()
    {
        // 한 번만 실행 옵션이 켜져있는 경우
        if (m_IsOnce)
        {
            // 스크립트를 끔
            m_Enable = false;
            //gameObject.SetActive(false);
        }

        // 페이드가 있을 경우
        if (m_FadeType == E_FadeType.Fade)
        {
            // 페이드가 가능한 경우
            if (Fade.CanFade())
            {
                // 시작 이벤트 추가
                Fade.FadeStart += DoStartEvent;

                // SetActive
                if (m_ActionType.HasFlag(E_SelectableObjectActionType.SetActive))
                {
                    Fade.FadeAction += SetActive;
                }
                // MoveCamera
                if (m_ActionType.HasFlag(E_SelectableObjectActionType.MoveCamera))
                {
                    // 화살표 끔
                    if (m_DirActive)
                    {
                        DisableDir();
                    }

                    // '현재 바라보는 방향으로 이동'은 이벤트 사용
                    // 따라서 '현재 바라보는 방향으로 이동'이 아닐 경우 카메라 이동
                    if (!m_DirectionMove)
                    {
                        Fade.FadeAction += CameraMove;
                    }
                }
                // ChangeImage
                if (m_ActionType.HasFlag(E_SelectableObjectActionType.ChangeImage))
                {
                    Fade.FadeAction += ChangeImage;
                }
                // AddItem
                if (m_ActionType.HasFlag(E_SelectableObjectActionType.AddItem))
                {
                    Fade.FadeAction += AddItem;
                }
                // RemoveItem
                if (m_ActionType.HasFlag(E_SelectableObjectActionType.RemoveItem))
                {
                    Fade.FadeAction += RemoveItem;
                }
                // PlaySound
                if (m_ActionType.HasFlag(E_SelectableObjectActionType.PlaySound))
                {
                    Fade.FadeAction += PlaySound;
                }

                // 화살표 업데이트
                if (m_DirActive)
                {
                    Fade.FadeAction += UpdateDirEnabled;
                }

                // 종료 이벤트 추가
                Fade.FadeEnd += DoEndEvent;

                // 페이드 실행
                Fade.DoFade();
            }
        }
        // 페이드가 없을 경우
        else if (m_FadeType == E_FadeType.NoneFade)
        {
            // 시작 이벤트 호출
            DoStartEvent();

            // SetActive
            if (m_ActionType.HasFlag(E_SelectableObjectActionType.SetActive))
            {
                SetActive();
            }
            // MoveCamera
            if (m_ActionType.HasFlag(E_SelectableObjectActionType.MoveCamera))
            {
                // '현재 바라보는 방향으로 이동'은 이벤트 사용
                // 따라서 '현재 바라보는 방향으로 이동'이 아닐 경우 카메라 이동
                if (!m_DirectionMove)
                {
                    CameraMove();
                }
            }
            // ChangeImage
            if (m_ActionType.HasFlag(E_SelectableObjectActionType.ChangeImage))
            {
                ChangeImage();
            }
            // AddItem
            if (m_ActionType.HasFlag(E_SelectableObjectActionType.AddItem))
            {
                AddItem();
            }
            // RemoveItem
            if (m_ActionType.HasFlag(E_SelectableObjectActionType.RemoveItem))
            {
                RemoveItem();
            }
            // PlaySound
            if (m_ActionType.HasFlag(E_SelectableObjectActionType.PlaySound))
            {
                PlaySound();
            }

            // 화살표 업데이트
            if (m_DirActive)
            {
                UpdateDirEnabled();
            }

            // 종료 이벤트 호출
            DoEndEvent();
        }
    }

    // 화살표 끄는 함수
    void DisableDir()
    {
        M_Direction.m_LeftImage.gameObject.SetActive(false);
        M_Direction.m_RightImage.gameObject.SetActive(false);
        M_Direction.m_UpImage.gameObject.SetActive(false);
        M_Direction.m_DownImage.gameObject.SetActive(false);
    }
    // 화살표 업데이트 함수
    void UpdateDirEnabled()
    {
        M_Direction.m_LeftImage.gameObject.SetActive(m_LeftActive);
        M_Direction.m_RightImage.gameObject.SetActive(m_RightActive);
        M_Direction.m_UpImage.gameObject.SetActive(m_UpActive);
        M_Direction.m_DownImage.gameObject.SetActive(m_DownActive);
    }

    // 설정한 오브젝트 수 만큼 오브젝트를 키고 끔
    void SetActive()
    {
        for (int i = 0; i < m_Objects.Count; ++i)
        {
            m_Objects[i].SetActive(m_Actives[i]);
        }
    }
    // 설정한 위치로 카메라를 이동
    void CameraMove()
    {
        // 이전 위치로 이동일 경우
        if (m_MoveToPreviousPos)
        {
            // 이동할 위치를 이전 위치로 변경
            m_Pos = m_LastPos;
        }
        // 일반적인 이동일 경우
        else
        {
            // 이전 위치 기억
            m_LastPos = m_Camera.transform.position;
        }

        // 카메라 이동
        m_Camera.transform.position = m_Pos;
    }
    // 설정한 렌더러의 이미지를 바꿈
    void ChangeImage()
    {
        if (m_Image != null)
        {
            m_Image.sprite = m_Sprite;
        }
        else if (m_Renderer != null)
        {
            m_Renderer.sprite = m_Sprite;
        }
    }
    // 아이템 추가
    void AddItem()
    {
        for (int i = 0; i < m_AddItemTypes.Count; ++i)
        {
            M_Inventory.PushSlotItem(m_AddItemTypes[i]);
        }
    }
    // 아이템 제거
    void RemoveItem()
    {
        for (int i = 0; i < m_RemoveItemTypes.Count; ++i)
        {
            M_Inventory.UseItem(m_RemoveItemTypes[i]);
        }
    }
    // 오디오 재생
    void PlaySound()
    {
        for (int i = 0; i < m_Audios.Count; ++i)
        {
            StartCoroutine(Play(i));
        }
    }
    IEnumerator Play(int index)
    {
        m_Audios[index].Stop();

        yield return new WaitForSeconds(m_AudioStartSeconds[index]);

        m_Audios[index].Play();
    }

    // 다음 스테이지로 넘어가는 함수
    public void NextStage()
    {
        ++__GameManager.Instance.m_CurrentStage;
        M_Inventory.ClearInventory();
    }
}
