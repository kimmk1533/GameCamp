using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_FadeType
{
    NoneFade,
    Fade,
}

[System.Flags]
public enum E_SelectableObjectActionType
{
    SetActive = 1 << 0,
    MoveCamera = 1 << 1,
    ChangeImage = 1 << 2,
}

public class SelectableObject : MonoBehaviour, IPointerClickHandler
{
    // 스크립트 활성화 여부
    public bool m_Enable;

    #region 클릭됐을 때 할 행동

    // Type
    public E_FadeType m_FadeType;
    public E_SelectableObjectActionType m_Type;

    #region SetActive
    public List<bool> m_Actives;
    public List<GameObject> m_Objects;
    #endregion

    #region MoveCamera
    // 화살표 표시 여부
    public bool m_LeftActive;
    public bool m_RightActive;
    public bool m_UpActive;
    public bool m_DownActive;

    // 현재 방향으로 이동
    public bool m_DirectionMove;
    // 이전 위치로 이동
    public bool m_LastActive;
    // 이동할 위치
    public Vector3 m_Position;
    // 이전 위치
    static Vector3 m_LastPos;
    #endregion

    #region ChangeSprite
    public bool m_IsOnce;
    public Sprite m_Image;
    public SpriteRenderer m_Renderer;
    #endregion

    #endregion

    public UnityEvent m_StartEvent;
    public UnityEvent m_EndEvent;

    Vector3 m_Pos;
    Camera m_Camera;

    DirectionManager M_Direction;

    private void Awake()
    {
        __Initialize();
    }

    public void __Initialize()
    {
        if (m_Objects.Count == 0)
            m_Objects.Add(this.gameObject);
        else if (m_Objects[0] == null)
            m_Objects[0] = this.gameObject;

        if (m_Renderer == null)
            m_Renderer = GetComponent<SpriteRenderer>();

        m_Enable = true;

        m_Camera = Camera.main;
        m_Pos = m_Position;
        m_Pos.z += m_Camera.transform.position.z;

        M_Direction = DirectionManager.Instance;
    }

    // 일반 오브젝트
    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            DoAction();
        }
    }
    // UI 오브젝트
    public void OnPointerClick(PointerEventData eventData)
    {
        DoAction();
    }

    public void TurnLeft()
    {
        M_Direction.TurnLeft();

        if (m_FadeType == E_FadeType.Fade)
        {
            Fade.FadeAction += M_Direction.CameraMoveToDir;
        }
        else if (m_FadeType == E_FadeType.NoneFade)
        {
            M_Direction.CameraMoveToDir();
        }
    }
    public void TurnRight()
    {
        M_Direction.TurnRight();

        if (m_FadeType == E_FadeType.Fade)
        {
            Fade.FadeAction += M_Direction.CameraMoveToDir;
        }
        else if (m_FadeType == E_FadeType.NoneFade)
        {
            M_Direction.CameraMoveToDir();
        }
    }
    public void TurnBack()
    {
        m_Pos = m_LastPos;
    }

    public void DoAction()
    {
        if (m_Enable)
        {
            Action();

            if (m_IsOnce)
            {
                gameObject.SetActive(false);
            }

            //if (m_IsOnce)
            //{
            //    if (!m_Toggle)
            //    {
            //        m_Toggle = true;
            //        Action();
            //    }
            //}
            //else
            //{
            //    Action();
            //}
        }
    }
    void Action()
    {
        if (m_FadeType == E_FadeType.NoneFade)
        {
            m_StartEvent?.Invoke();

            if (m_Type.HasFlag(E_SelectableObjectActionType.SetActive))
            {
                SetActive();
            }
            if (m_Type.HasFlag(E_SelectableObjectActionType.MoveCamera))
            {
                if (!m_DirectionMove)
                {
                    CameraMove();
                }
            }
            if (m_Type.HasFlag(E_SelectableObjectActionType.ChangeImage))
            {
                ChangeImage();
            }

            UpdateActive();
        }
        else if (m_FadeType == E_FadeType.Fade)
        {
            if (Fade.CanFade())
            {
                m_StartEvent?.Invoke();

                if (m_Type.HasFlag(E_SelectableObjectActionType.SetActive))
                {
                    Fade.FadeAction += SetActive;
                }
                if (m_Type.HasFlag(E_SelectableObjectActionType.MoveCamera))
                {
                    TurnOffImage();
                    if (!m_DirectionMove)
                    {
                        Fade.FadeAction += CameraMove;
                    }
                }
                if (m_Type.HasFlag(E_SelectableObjectActionType.ChangeImage))
                {
                    Fade.FadeAction += ChangeImage;
                }

                Fade.FadeAction += UpdateActive;

                Fade.DoFade();
            }
        }
    }

    void TurnOffImage()
    {
        M_Direction.m_LeftImage.gameObject.SetActive(false);
        M_Direction.m_RightImage.gameObject.SetActive(false);
        M_Direction.m_UpImage.gameObject.SetActive(false);
        M_Direction.m_DownImage.gameObject.SetActive(false);
    }
    void UpdateActive()
    {
        M_Direction.m_LeftImage.gameObject.SetActive(m_LeftActive);
        M_Direction.m_RightImage.gameObject.SetActive(m_RightActive);
        M_Direction.m_UpImage.gameObject.SetActive(m_UpActive);
        M_Direction.m_DownImage.gameObject.SetActive(m_DownActive);
    }

    void SetActive()
    {
        for (int i = 0; i < m_Objects.Count; ++i)
        {
            m_Objects[i].SetActive(m_Actives[i]);
        }
        //m_Objects.SetActive(m_Active);
    }
    void CameraMove()
    {
        if (m_LastActive)
        {
            m_Pos = m_LastPos;
        }
        else
        {
            m_LastPos = m_Camera.transform.position;
        }

        m_Camera.transform.position = m_Pos;
    }
    void ChangeImage()
    {
        m_Renderer.sprite = m_Image;
    }

    // 다음 스테이지로 넘어가는 함수
    public void NextStage()
    {
        ++__GameManager.Instance.m_CurrentStage;
    }
}
