using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    DirectionManager m_Direction;
    Animator m_Animator;
    bool m_IsFade;

    public void Awake()
    {
        __Initialize();
    }
    public void __Initialize()
    {
        m_Direction = DirectionManager.Instance;
        m_Animator = GetComponent<Animator>();
        m_IsFade = false;

        LeftButtonClick += TurnLeft;
        RightButtonClick += TurnRight;
    }

    public void CameraMove()
    {
        m_Direction.CameraMove();
    }

    public delegate void ButtonClick();

    public ButtonClick LeftButtonClick;
    public ButtonClick RightButtonClick;

    void FadeStart()
    {
        m_IsFade = true;
    }
    void FadeEnd()
    {
        m_IsFade = false;
    }
    void TurnLeft()
    {
        m_Direction.TurnLeft();
        m_Animator.SetTrigger("Fade");
    }
    void TurnRight()
    {
        m_Direction.TurnRight();
        m_Animator.SetTrigger("Fade");
    }
    public void OnLeftButtonClick()
    {
        if (!m_IsFade)
        {
            LeftButtonClick?.Invoke();
        }
    }
    public void OnRightButtonClick()
    {
        if (!m_IsFade)
        {
            RightButtonClick?.Invoke();
        }
    }
}
