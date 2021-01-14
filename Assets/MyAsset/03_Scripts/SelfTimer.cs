using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfTimer : MonoBehaviour
{
    int sum_mm, sum_ss; //시작 시간과 wantTime을 이용해 타이머가 종료되는 타이밍을 계산(SumTime 함수).
    public int wantTime_mm, wantTime_ss;    //원하는 시간(1분 10초동안 기다리면 작동하고 싶다=>1,10) 설정.
    public SpriteRenderer changeObjRender;  //스프라이트를 바꿀 오브젝트.
    public Sprite change_spr;   //바꿀 스프라이트.
    bool isSuccess = false; //셀프 스위치(타이머가 성공적으로 역할을 다하고 종료되면 true).
    bool isOn = false;   //Active 체크.

    public void StartSelfTimer()    //타이머 시작 함수.
    {
        if (!isOn)
            if (!isSuccess)
            {
                isOn = true;
                SumTime();
            }
    }

    private void Update()
    {
        if (isOn)
            EndSelfTimer();
    }

    public void EndSelfTimer()  //타이머 종료 함수.
    {
        if (!isSuccess)
        {
            if (TimerManager.Instance.IsOverTime(sum_mm, sum_ss))
            {
                Debug.Log("셀프 타이머 성공적 마무리");
                changeObjRender.sprite = change_spr;
                isSuccess = true;
            }
            isOn = false;
        }
    }
    void SumTime() //시작 시간과 wantTime을 이용해 타이머가 종료되는 타이밍을 계산.
    {  
        sum_mm = TimerManager.Instance.GetLimitTimerMM() - wantTime_mm + ((TimerManager.Instance.GetLimitTimerSS() - wantTime_ss > 0) ? 0 : -1);
        sum_ss = TimerManager.Instance.GetLimitTimerSS() - wantTime_ss + ((TimerManager.Instance.GetLimitTimerSS() - wantTime_ss > 0) ? 0 : +60);
        Debug.Log(sum_mm + "분 " + sum_ss + "초 설정, 현재 " + TimerManager.Instance.GetLimitTimerMM() + "분 " + TimerManager.Instance.GetLimitTimerSS() + "초");
    }
}
