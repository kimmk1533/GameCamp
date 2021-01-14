using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __GameManager : Singleton<__GameManager>
{
    public int m_CurrentStage = 0;
    public Camera m_Camera;

    protected override void Awake()
    {
        base.Awake();

        __Initialize();
    }

    public override void __Initialize()
    {
        M_Pause = PauseManager.Instance;
        M_Pause.__Initialize();

        M_Direction = DirectionManager.Instance;
        M_Direction.__Initialize();

        M_Mouse = MouseManager.Instance;
        M_Mouse.__Initialize();

        DB_Item = ItemDB.Instance;
        DB_Item.__Initialize();

        M_Inventory = InventoryManager.Instance;
        M_Inventory.__Initialize();

        M_Timer = TimerManager.Instance;
        M_Timer.__Initialize();

        M_Sound = SoundManager.Instance;
        M_Sound.__Initialize();
    }

    DirectionManager M_Direction;
    MouseManager M_Mouse;
    ItemDB DB_Item;
    InventoryManager M_Inventory;

    SoundManager M_Sound;
    PauseManager M_Pause;
    TimerManager M_Timer;

#if UNITY_EDITOR
    [ContextMenu("[ 현재 스테이지의 초기 위치로 카메라 이동 ]")]
    public void MoveCamera()
    {
        m_Camera.transform.position = DirectionManager.Instance.m_StandardPos[m_CurrentStage] + new Vector3(-30, 0, -50);
    }
#endif
}
