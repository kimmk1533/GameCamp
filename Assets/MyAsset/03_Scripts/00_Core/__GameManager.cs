using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __GameManager : Singleton<__GameManager>
{
    DirectionManager M_Direction;
    MouseManager M_Mouse;
    InventoryManager M_Inventory;

    protected override void Awake()
    {
        base.Awake();

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
}
