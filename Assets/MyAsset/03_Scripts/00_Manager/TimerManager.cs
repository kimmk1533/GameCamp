﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    [ShowOnly, SerializeField]
    int LimitTimer_mm;  //분 단위로 환산 및 저장(초 단위 내림값).
    [ShowOnly, SerializeField]
    int LimitTimer_ss;  //초 단위로 환산 및 저장(초 밑 점 단위 내림값).
    [ShowOnly, SerializeField]
    float LimitTimer;   //초단위로 저장.
    [ShowOnly, SerializeField]
    bool timerStop = false;  //타이머 작동 여부.

    public override void __Initialize()
    {
        SetLimitTimer(10, 0);   //일단 어디서 설정할 곳이 없으니 생성 시 타이머 10분 설정.
    }

    void Update()
    {
        if (!timerStop)
        {
            LimitTimer -= Time.deltaTime;
            RenewalMMSS();
            if (IsOverTime(0, 0))   //게임 오버 예시.
            {
                timerStop = true;
                Debug.LogError("GameOver");
            }
        }
    }

    public void ResetTimer(int _new_mm, int _new_ss)
    {
        SetLimitTimer(_new_mm, _new_ss);
    }
    public bool IsOverTime(int _check_mm, int _check_ss)   //시간이 지났는지 체크(원하는 분, 원하는 초 입력), 시간이 지났을 시 true 반환.
    {
        if (LimitTimer_mm <= _check_mm)
        {
            if (LimitTimer_ss < _check_ss)  //분과 초 계산을 소수점 내림으로 계산하기 때문에 <= 하면 안됨.
            {
                Debug.Log(LimitTimer_mm + "분 " + LimitTimer_ss + 1 + " 초 경과");
                return true;
            }
        }
        return false;
    }

    //get set
    public float GetLimitTimer()
    {
        return LimitTimer;
    }
    public void SetLimitTimer(int _new_mm, int _new_ss)  //타이머 세팅(분, 초).
    {
        LimitTimer_mm = _new_mm;
        LimitTimer_ss = _new_ss;
        LimitTimer = (_new_mm * 60) + _new_ss;
    }
    public int GetLimitTimerMM()  //분 단위 get
    {
        SetLimitTimerMM();
        return LimitTimer_mm;
    }
    void SetLimitTimerMM()
    {
        LimitTimer_mm = Mathf.FloorToInt(GetLimitTimer() / 60);
    }
    public int GetLimitTimerSS()  //초 단위 get
    {
        SetLimitTimerSS();
        return LimitTimer_ss;
    }
    void SetLimitTimerSS()
    {
        LimitTimer_ss = Mathf.FloorToInt(GetLimitTimer() % 60);
    }
    public bool GetTimerStop()
    {
        return timerStop;
    }
    public void SetTimerStop(bool _isstop)
    {
        timerStop = _isstop;
    }

    public void RenewalMMSS()  //분과 초 단위 갱신.
    {
        SetLimitTimerMM();
        SetLimitTimerSS();
    }
}
