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

    Stage2_붓,
    Stage2_빨간__물감이__묻은__붓,
    Stage2_노란__물감이__묻은__붓,
    Stage2_파란__물감이__묻은__붓,
    Stage2_조각용__칼,
    Stage2_종이컵,
    Stage2_부러진__열쇠,
    Stage2_물이__담긴__컵,
    Stage2_석고__반죽,
    Stage2_석고__열쇠,

    Stage3_쇠톱날,
    Stage3_열쇠,
    Stage3_숫자가__적힌__종이,

    Stage4_쪽가위,
    Stage4_단추,
    Stage4_열쇠,
    Stage4_인형의__팔,
    Stage4_식칼,
    Stage4_도끼,

    Stage5_장난감__삽,
    Stage5_식칼,
    Stage5_쪽지,
    Stage5_건전지,
    Stage5_금속탐지기,
    Stage5_열쇠,

    Stage6_열쇠__A,
    Stage6_열쇠__B,
    Stage6_열쇠__C,
    Stage6_열쇠__D,
    Stage6_지도,
    Stage6_출구__열쇠,

    Stage7_곡괭이,
    Stage7_초록보석,
    Stage7_노란보석,
    Stage7_파란보석,
    Stage7_빨간보석,
    Stage7_열쇠,
    Stage7_손전등,


    Stage8_문제종이,
    Stage8_간__모형,
    Stage8_심장__모형,
    Stage8_눈__모형,
    Stage8_U__S__B,

    Stage9_영어종이,
    Stage9_마른손수건,
    Stage9_젖은손수건,
    Stage9_라이터,
    Stage9_칠판지우개,
    
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
