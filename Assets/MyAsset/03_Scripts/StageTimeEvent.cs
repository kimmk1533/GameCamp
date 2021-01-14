using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeEvent
{
    public List<SelectableObject> event_lst = new List<SelectableObject>(); //실행할 이벤트 스크립트.
    public int doAction_mm; //의 분.
    public int doAction_ss; //의 초.
    [ShowOnly]
    public bool doAction_isplay;  //의 실행 여부(한번만 실행).
}

public class StageTimeEvent : MonoBehaviour
{
    public int stage_id;    //스테이지 id(게임매니저와 동일).
    public int mm, ss;  //스테이지 타이머 시간.
    public bool isTimerInOp = true;    //스테이지 실행 시 타이머 온 오프 여부.
    bool timerStart = false;    //스테이지 내 타이머 시작 여부.

    public Color globalLight_color = Color.black;
    public Color pointLight_color = Color.black;
    bool lightchange = false;

    public List<TimeEvent> timeEvent_lst = new List<TimeEvent>(); //실행할 이벤트 스크립트.

    private void Awake()
    {
        if (timeEvent_lst.Count > 0)
            for (int i = 0; i < timeEvent_lst.Count; i++)
            {
                timeEvent_lst[i].doAction_isplay = false;
            }
    }

    private void Update()
    {
        //if (TimerManager.Instance.GetTimerOn())
            if (stage_id == __GameManager.Instance.m_CurrentStage)
            {
                if (!lightchange)
                {
                    LightManager.Instance.SetTmpLight(0);
                    LightManager.Instance.ChangeLightColor(globalLight_color);
                    LightManager.Instance.SetTmpLight(1);
                    LightManager.Instance.ChangeLightColor(pointLight_color);
                    LightManager.Instance.StartUpdateSetting(3f);
                    lightchange = true;
                }
                if (timerStart)
                {
                    if (timeEvent_lst.Count > 0)
                        for (int i = 0; i < timeEvent_lst.Count; i++)
                            if (!timeEvent_lst[i].doAction_isplay)
                                if (TimerManager.Instance.IsOverTime(timeEvent_lst[i].doAction_mm, timeEvent_lst[i].doAction_ss))
                                {
                                    if (timeEvent_lst[i].event_lst.Count > 0)
                                    {
                                        for (int j = 0; j < timeEvent_lst[i].event_lst.Count; j++)
                                        {
                                            timeEvent_lst[i].event_lst[j].DoAction();
                                        }
                                        timeEvent_lst[i].doAction_isplay = true;
                                    }
                                }
                }
                else
                {
                    if (isTimerInOp)
                    {
                        StartStageTime();
                    }
                }
            }
    }


    public void StartStageTime()    //외부 함수로 타이머 실행.
    {
        TimerManager.Instance.SetLimitTimer(mm, ss);
        timerStart = true;
        TimerManager.Instance.SetTimerOn(timerStart);
    }
}
