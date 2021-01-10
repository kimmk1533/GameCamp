using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    static Animator m_Animator;
    static bool m_IsFade;

    public void Awake()
    {
        __Initialize();
    }
    public void __Initialize()
    {
        m_Animator = GetComponent<Animator>();
        m_IsFade = false;

        FadeStart += StartFade;
        FadeEnd += EndFade;
    }

    static void StartFade()
    {
        m_IsFade = true;
    }
    static void EndFade()
    {
        m_IsFade = false;
    }

    #region 애니메이션 이벤트
    public delegate void Default();

    public static Default FadeStart;
    public static Default FadeAction;
    public static Default FadeEnd;

    void DoFadeStart()
    {
        FadeStart?.Invoke();
    }
    void DoFadeAction()
    {
        FadeAction?.Invoke();
    }
    void DoFadeEnd()
    {
        FadeEnd?.Invoke();
    }

    void ClearEvent()
    {
        FadeStart = null;
        FadeStart += StartFade;

        FadeAction = null;

        FadeEnd = null;
        FadeEnd += EndFade;
    }
    #endregion

    public static bool CanFade()
    {
        return !m_IsFade;
    }
    public static void DoFade()
    {
        m_Animator.SetTrigger("Fade");
    }
}
