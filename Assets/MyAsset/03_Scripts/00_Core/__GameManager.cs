using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __GameManager : Singleton<__GameManager>
{
    DirectionManager m_Direction;

    protected override void Awake()
    {
        base.Awake();

        __Initialize();
    }

    public override void __Initialize()
    {
        m_Direction = DirectionManager.Instance;
        m_Direction.__Initialize();
    }
}
