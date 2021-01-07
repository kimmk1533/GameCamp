using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum E_SelectableObjectType
{
    SetActive = 1 << 0,
    MoveCamera = 1 << 1,
    ChangeImage = 1 << 2,
}

[RequireComponent(typeof(Collider2D))]
public class SelectableObject : MonoBehaviour
{
    public bool m_Enable;

    public E_SelectableObjectType m_Type;

    // SetActive
    public GameObject m_Object;
    public bool m_Active;

    // MoveCamera
    public Vector3 m_Position;

    // ChangeSprite
    public bool m_IsOnce;
    private bool m_Toggle;
    public Sprite m_Image;
    public SpriteRenderer m_Renderer;

    Vector3 m_Pos;
    Camera m_Camera;

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
    }

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

    void DoAction()
    {
        if (Fade.CanFade())
        {
            switch (m_Type)
            {
                default:
                    return;

                case E_SelectableObjectType.SetActive:
                    Fade.FadeAction += SetActive;
                    break;
                case E_SelectableObjectType.MoveCamera:
                    Fade.FadeAction += CameraMove;
                    break;
                case E_SelectableObjectType.ChangeImage:
                    Fade.FadeAction += ChangeImage;
                    break;
            }

            Fade.DoFade();
        }
    }

    void SetActive()
    {
        m_Object.SetActive(m_Active);
    }
    void CameraMove()
    {
        m_Camera.transform.position = m_Pos;
    }
    void ChangeImage()
    {
        m_Renderer.sprite = m_Image;
    }
}
