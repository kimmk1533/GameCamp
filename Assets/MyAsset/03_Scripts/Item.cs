using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int index;
    public string name;
    public Sprite Image;
    public string add;

    public void LoadingItemToIndex()    //아이템DB에서 아이템을 대입 시 사용 함수.
    {
        if (ItemDB.inst != null)
        {
            Item tmp = ItemDB.inst.ReturnItemToIndex(index);
            index = tmp.index;
            name = tmp.name;
            Image = tmp.Image;
            add = tmp.add;
        }
    }
}
