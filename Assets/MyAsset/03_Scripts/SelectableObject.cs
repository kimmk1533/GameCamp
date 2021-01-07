using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public Vector3 m_Position;

    Vector3 m_Pos;
    Camera m_Camera;

    private void Awake()
    {
        __Initialize();
    }

    public void __Initialize()
    {
        m_Camera = Camera.main;
        m_Pos = m_Position;
        m_Pos.z += m_Camera.transform.position.z;
    }

    private void OnMouseUp()
    {
        if (Fade.CanFade())
        {
            Fade.MoveCamera += CameraMove;
            Fade.DoFade();
        }
    }

    void CameraMove()
    {
        m_Camera.transform.position = m_Pos;
    }
}
