using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public bool m_Active;
    public GameObject m_Object;
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
    private bool m_Toggle;
    public Sprite m_Image;
    public SpriteRenderer m_Renderer;
    #endregion

    #endregion

    Vector3 m_Pos;
    Camera m_Camera;

    DirectionManager M_Direction;

    private void Awake()
    {
        __Initialize();
    }

    public void __Initialize()
    {
        if (m_Object == null)
            m_Object = this.gameObject;
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
        if (m_Enable)
        {
            if (m_IsOnce)
            {
                if (!m_Toggle)
                {
                    m_Toggle = true;
                    DoAction();
                }
            }
            else
            {
                DoAction();
            }
        }
    }
    // UI 오브젝트
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_Enable)
        {
            if (m_IsOnce)
            {
                if (!m_Toggle)
                {
                    m_Toggle = true;
                    DoAction();
                }
            }
            else
            {
                DoAction();
            }
        }
    }

    public void TurnLeft()
    {
        M_Direction.TurnLeft();
        Fade.FadeAction += M_Direction.CameraMoveToDir;
    }
    public void TurnRight()
    {
        M_Direction.TurnRight();
        Fade.FadeAction += M_Direction.CameraMoveToDir;
    }
    public void TurnBack()
    {
        m_Pos = m_LastPos;
    }

    void DoAction()
    {
        if (m_FadeType == E_FadeType.NoneFade)
        {
            switch (m_Type)
            {
                default:
                    return;

                case E_SelectableObjectActionType.SetActive:
                    SetActive();
                    break;
                case E_SelectableObjectActionType.MoveCamera:
                    CameraMove();
                    UpdateActive();
                    break;
                case E_SelectableObjectActionType.ChangeImage:
                    ChangeImage();
                    break;
            }
        }
        else if (m_FadeType == E_FadeType.Fade)
        {
            if (Fade.CanFade())
            {
                switch (m_Type)
                {
                    default:
                        return;

                    case E_SelectableObjectActionType.SetActive:
                        Fade.FadeAction += SetActive;
                        break;
                    case E_SelectableObjectActionType.MoveCamera:
                        Fade.FadeAction += CameraMove;
                        UpdateActive();
                        break;
                    case E_SelectableObjectActionType.ChangeImage:
                        Fade.FadeAction += ChangeImage;
                        break;
                }

                Fade.DoFade();
            }
        }
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
        m_Object.SetActive(m_Active);
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
}
