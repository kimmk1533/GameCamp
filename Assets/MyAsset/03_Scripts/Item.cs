using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ItemType
{
    None,
    편지,
    열쇠,
    성냥곽,

    Max
}

[System.Serializable]
public class Item
{
    // 아이템 타입
    public E_ItemType m_Type;
    // 아이템 이미지
    public Sprite m_Image;
    // 아이템 설명
    public string m_Description;

    public void LoadingItemToIndex()    //아이템DB에서 아이템을 대입 시 사용 함수.
    {
        if (ItemDB.Instance != null)
        {
            Item temp = ItemDB.Instance.ReturnItemToIndex(m_Type);

            m_Type = temp.m_Type;
            m_Image = temp.m_Image;
            m_Description = temp.m_Description;
        }
    }
}
