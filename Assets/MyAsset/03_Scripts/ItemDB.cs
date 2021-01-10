using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : Singleton<ItemDB>
{
    public List<Item> m_DataBase = new List<Item>();

    public override void __Initialize()
    {

    }

    public Item ReturnItemToIndex(E_ItemType e_type)
    {
        return m_DataBase[(int)e_type];
    }
}
