using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    static Animator m_Animator;
    static bool m_IsFade;

    DirectionManager M_Direction;

    public void Awake()
    {
        __Initialize();
    }
    public void __Initialize()
    {
        M_Direction = DirectionManager.Instance;
        m_Animator = GetComponent<Animator>();
        m_IsFade = false;

        LeftButtonClick += TurnLeft;
        RightButtonClick += TurnRight;
    }

    public delegate void Default();

    public static Default LeftButtonClick;
    public static Default RightButtonClick;
    public static Default FadeAction;

    void FadeStart()
    {
        m_IsFade = true;
        Debug.Log("FadeStart");
    }
    void FadeEnd()
    {
        m_IsFade = false;
        FadeAction = null;
        Debug.Log("FadeEnd");
    }

    void TurnLeft()
    {
        M_Direction.TurnLeft();
        DoFade();
    }
    void TurnRight()
    {
        M_Direction.TurnRight();
        DoFade();
    }
    void DirectionCameraMove()
    {
        M_Direction.CameraMoveToDir();
    }

    public void DoLeftButtonClick()
    {
        if (CanFade())
        {
            FadeAction += DirectionCameraMove;
            LeftButtonClick?.Invoke();
        }
    }
    public void DoRightButtonClick()
    {
        if (CanFade())
        {
            FadeAction += DirectionCameraMove;
            RightButtonClick?.Invoke();
        }
    }
    public void DoCameraMove()
    {
        FadeAction?.Invoke();
    }

    public static bool CanFade()
    {
        return !m_IsFade;
    }
    public static void DoFade()
    {
        m_Animator.SetTrigger("Fade");
    }
}
