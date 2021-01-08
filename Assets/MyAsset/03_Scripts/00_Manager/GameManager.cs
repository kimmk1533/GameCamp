﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
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
        M_Direction = DirectionManager.Instance;
        M_Direction.__Initialize();

        M_Mouse = MouseManager.Instance;
        M_Mouse.__Initialize();

        M_Inventory = InventoryManager.Instance;
        M_Inventory.__Initialize();
    }

    DirectionManager M_Direction;
    MouseManager M_Mouse;
    InventoryManager M_Inventory;

    ////접근 가능 매니저 스크립트.
    //public MouseManager mouseM;

    //전역 변수.
    public Camera maincamera;
}
