using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public enum E_Type
    {
        None,
        SetActive,
        MoveCamera,
    }

    public E_Type m_Type;

    // SetActive
    public GameObject m_Object;
    public bool m_Active;

    // MoveCamera
    public Vector3 m_Position;

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

        m_Camera = Camera.main;
        m_Pos = m_Position;
        m_Pos.z += m_Camera.transform.position.z;
    }

    private void OnMouseUp()
    {
        if (Fade.CanFade())
        {
            switch (m_Type)
            {
                case E_Type.None:
                    return;
                case E_Type.SetActive:
                    Fade.FadeAction += SetActive;
                    Fade.DoFade();
                    break;
                case E_Type.MoveCamera:
                    Fade.FadeAction += CameraMove;
                    Fade.DoFade();
                    break;
            }

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
}
