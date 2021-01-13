using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StageN_아이템이름 순서대로 해주세요
// _ 는 스테이지와 아이템이름 중간에 하나만 있어야 합니다.
// __ 는 띄어쓰기로 바뀝니다.

public enum E_ItemType
{
    None,

    Stage0_편지,
    Stage0_그을린__편지,
    Stage0_열쇠,
    Stage0_성냥,

    Stage1_포스트잇,
    Stage1_휴지,
    Stage1_물__묻은__휴지,
    Stage1_종이,

    Stage6_열쇠__A,
    Stage6_열쇠__B,
    Stage6_열쇠__C,
    Stage6_열쇠__D,
    Stage6_지도,
    Stage6_병원출구열쇠,

    Stage8_문제종이,
    Stage8_간모형,
    Stage8_심장모형,
    Stage8_눈모형,
    Stage8_USB,
    
    Stage10_숫자가__적힌__휴지,

    Max
}

public class ItemDB : Singleton<ItemDB>
{
    public MyDictionary<E_ItemType, Item> m_DataBase = new MyDictionary<E_ItemType, Item>();
    //public List<Item> m_DataBase = new List<Item>();

    public override void __Initialize()
    {

    }

    public Item ReturnItemToIndex(E_ItemType e_type)
    {
        return m_DataBase[e_type];
    }
}
