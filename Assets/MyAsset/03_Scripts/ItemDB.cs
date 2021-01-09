using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : Singleton<ItemDB>
{
    public List<Item> itemDB = new List<Item>();

    public override void __Initialize()
    {

    }

    public Item ReturnItemToIndex(E_ItemType e_type)
    {
        return itemDB[(int)e_type];
    }
}
