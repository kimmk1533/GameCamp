using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __GameManager : Singleton<__GameManager>
{
    public int m_CurrentStage = 0;

    protected override void Awake()
    {
        base.Awake();

        maincamera = Camera.main;

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

        M_Stage = StageManager.Instance;
        M_Stage.__Initialize();

        M_Sound = SoundManager.Instance;
        M_Sound.__Initialize();
    }

    DirectionManager M_Direction;
    MouseManager M_Mouse;
    ItemDB DB_Item;
    InventoryManager M_Inventory;
    StageManager M_Stage;
    SoundManager M_Sound;
    PauseManager M_Pause;

    ////접근 가능 매니저 스크립트.
    //public MouseManager mouseM;

    //전역 변수.
    public Camera maincamera;
}
