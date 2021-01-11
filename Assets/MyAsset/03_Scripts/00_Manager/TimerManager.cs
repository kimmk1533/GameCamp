using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    //ReadOnly로 쓰고싶은데 인스펙터에 안써서 일단 저걸로 썼어요 바꿔주면 감사~.
    [SerializeField]
    int LimitTimer_mm;  //분 단위로 환산 및 저장(초 단위 내림값).
    [SerializeField]
    int LimitTimer_ss;  //초 단위로 환산 및 저장(초 밑 점 단위 내림값).
    [SerializeField]
    float LimitTimer;   //초단위로 저장.
    [SerializeField]
    bool timerStop = false;  //타이머 작동 여부.

    public override void __Initialize()
    {
        SetLimitTimer(10, 0);
    }

    void Update()
    {
        if (!timerStop)
        {
            LimitTimer -= Time.deltaTime;
            RenewalMMSS();
            if (IsOverTime(0, 0))
            {
                timerStop = true;
                Debug.LogError("GameOver");
            }
        }
    }

    void ResetTimer(int _new_mm, int _new_ss)
    {
        SetLimitTimer(_new_mm, _new_ss);
    }
    bool IsOverTime(int _check_mm, int _check_ss)   //시간이 지났는지 체크(원하는 분, 원하는 초 입력), 시간이 지났을 시 true 반환.
    {
        if (GetLimitTimerMM() <= _check_mm)
        {
            if (GetLimitTimerSS() < _check_ss)  //분과 초 계산을 소수점 내림으로 계산하기 때문에 <= 하면 안됨.
            {
                Debug.Log(GetLimitTimerMM() + "분 " + GetLimitTimerSS() + 1 + " 초 경과");
                return true;
            }
        }
        return false;
    }

    //get set
    float GetLimitTimer()
    {
        return LimitTimer;
    }
    void SetLimitTimer(int _new_mm, int _new_ss)  //타이머 세팅(분, 초).
    {
        LimitTimer_mm = _new_mm;
        LimitTimer_ss = _new_ss;
        LimitTimer = (_new_mm * 60) + _new_ss;
    }
    int GetLimitTimerMM()  //분 단위 get
    {
        SetLimitTimerMM();
        return LimitTimer_mm;
    }
    void SetLimitTimerMM()
    {
        LimitTimer_mm = Mathf.FloorToInt(GetLimitTimer() / 60);
    }
    int GetLimitTimerSS()  //초 단위 get
    {
        SetLimitTimerSS();
        return LimitTimer_ss;
    }
    void SetLimitTimerSS()
    {
        LimitTimer_ss = Mathf.FloorToInt(GetLimitTimer() % 60);
    }
    bool GetTimerStop()
    {
        return timerStop;
    }
    void SetTimerStop(bool _isstop)
    {
        timerStop = _isstop;
    }

    void RenewalMMSS()  //분과 초 단위 갱신.
    {
        SetLimitTimerMM();
        SetLimitTimerSS();
    }
}
