﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StageN_아이템이름 순서대로 해주세요
// _ 는 스테이지와 아이템이름 중간에 하나만 있어야 합니다.
// __ 는 띄어쓰기로 바뀝니다.

public enum E_ItemType
{
    None,

    Stage0_편지,
    Stage0_열쇠,
    Stage0_성냥,
    Stage0_편지성냥사용,

    Stage1_포스트잇,
    Stage1_휴지,
    Stage1_물__묻은__휴지,
    Stage1_종이,

    Stage6_엑스레이열쇠A,
    Stage6_엑스레이열쇠B,
    Stage6_엑스레이열쇠C,
    Stage6_엑스레이열쇠D,
    Stage6_지도,
    Stage6_병원출구열쇠,

    Stage8_문제종이,
    Stage8_간모형,

    Max
}

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
