using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : Singleton<GameManager>
{
    public static ItemDB inst;
    public List<Item> itemDB = new List<Item>();

    protected override void Awake()
    {
        base.Awake();

        __Initialize();
    }

    public override void __Initialize()
    {
        inst = this;    //싱글톤으로 처리할 필요는 없어서 별도 inst(그래서 GameManager 오브젝트의 자식이 아님).
    }

    public Item ReturnItemToIndex(int _index)
    {
        return itemDB[_index];
    }
}
