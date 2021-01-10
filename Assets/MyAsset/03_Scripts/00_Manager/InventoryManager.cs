using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum E_ItemType
{
    None,

    // Stage 0
    편지,
    열쇠,
    성냥곽,

    // Stage 1
    // Stage 2
    // Stage 3
    // Stage 4
    // Stage 5
    // Stage 6
    // Stage 7
    // Stage 8
    // Stage 9
    // Stage 10

    Max
}

public class InventoryManager : Singleton<InventoryManager>
{
    public Transform InventoryPanal;
    public Transform RightDirectionUI;  //맵 이동 오른쪽 버튼 트랜스폼(인벤토리 이동 시 함께 이동하기 위함).
    [SerializeField] Inventory_State inventory_state = Inventory_State.Close;
    [Space(10f)]
    [Tooltip("인벤토리 슬롯")]
    [SerializeField] List<InvenSlot> Slot_lst = new List<InvenSlot>();
    //[SerializeField] List<Item> Slot_item = new List<Item>();
    int slot_max = 0;
    int slot_size = 0;

    public Transform ItemAddWindow; //아이템 설명 창.

    public override void __Initialize()
    {
        InitializeSettingSlot();
        PushSlotItem(E_ItemType.편지);
    }

    enum Inventory_State
    {
        Open,
        Moving,
        Close
    }

    IEnumerator InventoryMove() //인벤토리 좌우 이동.
    {
        Vector3 InvenPos = InventoryPanal.parent.localPosition;  //인벤토리 UI 이동 용.
        Vector3 RightPos = RightDirectionUI.localPosition;   //right 방향 시점 변환 버튼 UI 이동 용.

        int direction = (inventory_state == Inventory_State.Open ? -1 : 1);  //좌우 이동의 값 음양 변화.
        Inventory_State before_state = inventory_state;
        inventory_state = Inventory_State.Moving;

        float moveSize = InventoryPanal.parent.GetComponent<RectTransform>().sizeDelta.x / 2;   //인벤토리 UI 캔버스의 크기.
        float movement = moveSize / 10;

        for (int i = 0; i < movement; i++)
        {
            //인벤토리 UI 이동.
            InvenPos.x += direction * moveSize / movement;
            InventoryPanal.parent.localPosition = InvenPos;
            //right 방향 시점 변환 버튼 UI 이동.
            RightPos.x += direction * moveSize / movement;
            RightDirectionUI.localPosition = RightPos;
            yield return null;
        }

        inventory_state = before_state; //Moving State 해제.
    }

    public void MouseOverSlot(GameObject _hit)
    {
        int hit_index = _hit.transform.GetChild(0).GetComponent<InvenSlot>().m_SlotIndex;
        
        if (GetSlotItem(hit_index).m_Type != E_ItemType.None)
        {
            RenewalItemLst();
            ItemAddWindow.position = _hit.transform.GetChild(1).position;
            TextMeshProUGUI hit_nametmp = Instance.ItemAddWindow.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI hit_addtmp = Instance.ItemAddWindow.GetChild(1).GetComponent<TextMeshProUGUI>();
            hit_nametmp.text = GetSlotItem(hit_index).m_Type.ToString();
            hit_addtmp.text = GetSlotItem(hit_index).m_Description;
            ItemAddWindow.gameObject.SetActive(true);
        }
    }
    public void MouseExitSlot()
    {
        ItemAddWindow.position = Vector2.one * -9999;  //TextMeshPro가 Raycast Target 대상이라 마우스 조작에 방해되는데 켜고 끌 수가 없어서 좌표 값을 멀리 보내버림.
        ItemAddWindow.gameObject.SetActive(false);
    }

    public void InventoryOpenButton()   //인벤토리 열기/닫기 버튼 클릭 시.
    {
        if (inventory_state != Inventory_State.Moving)  //이동 중 중복 클릭 방지.
        {
            //동작할 state 값으로 변환 후 인벤토리 UI 이동 코루틴 실행.
            if (inventory_state == Inventory_State.Close)
            {
                inventory_state = Inventory_State.Open;
            }
            else if (inventory_state == Inventory_State.Open)
            {
                inventory_state = Inventory_State.Close;
            }
            Vector3 tmp = InventoryPanal.parent.GetChild(2).localScale;
            tmp.x *= -1;
            InventoryPanal.parent.GetChild(2).localScale = tmp;
            StartCoroutine("InventoryMove");
        }
        
    }

    //get set
    public Item GetSlotItem(int _index)
    {
        return Slot_lst[_index].m_ItemInfo;
    }
    public void SetSlotItem(int _slotindex, E_ItemType _ItemType)
    {
        Slot_lst[_slotindex].m_ItemInfo = ItemDB.Instance.ReturnItemToIndex(_ItemType);

        //Slot_item[_slotindex].LoadingItemToIndex();
    }

    void InitializeSettingSlot()
    {
        Slot_lst.Clear();

        for (int i = 0; i < InventoryPanal.childCount; i++)
        {
            InvenSlot temp = InventoryPanal.GetChild(i).GetChild(0).GetComponent<InvenSlot>();
            temp.m_SlotIndex = i;
            Slot_lst.Add(temp);
            slot_max++;
        }
    }

    public void PushSlotItem(int _ItemIndex)
    {
        PushSlotItem((E_ItemType)_ItemIndex);
    }
    void PushSlotItem(E_ItemType _ItemType)
    {
        if (slot_size < slot_max)
        {
            SetSlotItem(slot_size++, _ItemType);
        }

        RenewalItemLst();
    }
    public void PullSlotItem(int _slotindex)
    {
        SetSlotItem(_slotindex, E_ItemType.None);
        slot_size--;

        RenewalItemLst();
    }
    void SortItemLst()   //인벤토리 아이템 순서 재정렬.
    {
        Slot_lst.Sort(delegate (InvenSlot a, InvenSlot b)
        {
            int a1 = (int)a.m_ItemInfo.m_Type;
            int b1 = (int)b.m_ItemInfo.m_Type;

            if (a1 == 0)
                a1 = 99999;
            if (b1 == 0)
                b1 = 99999;

            return a1.CompareTo(b1);
        });
    }

    void RenewalItemLst()   //인벤토리 재정렬 설정.
    {
        SortItemLst();

        for (int i = 0; i < slot_max; i++)
        {
            Slot_lst[i].m_ItemInfo.LoadingItemToIndex();
            Slot_lst[i].m_Image.sprite = ItemDB.Instance.ReturnItemToIndex(Slot_lst[i].m_ItemInfo.m_Type).m_Image;
        }
    }
}
