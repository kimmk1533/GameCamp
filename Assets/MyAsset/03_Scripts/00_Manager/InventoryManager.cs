using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    [ReadOnly(true)]
    public Transform m_InventoryPanal;
    [ReadOnly(true)]
    public Transform m_RightDirectionUI;
    [ReadOnly(true)]
    public GameObject m_ItemZoomIn;
    [SerializeField] E_InventoryState m_InventoryState;
    public Transform m_ItemInfoWindow; //아이템 설명 창.
    [Space(10f)]
    [Tooltip("인벤토리 슬롯")]
    [SerializeField] List<InvenSlot> m_SlotList;

    [ShowOnly]
    public InvenSlot m_ActivedSlot;

    [ReadOnly(true)]
    public Color m_DefaultColor;
    [ReadOnly(true)]
    public Color m_HighlightColor;
    [ReadOnly(true)]
    public Color m_ActivedColor;

    int m_SlotMaxSize = 0;
    int m_SlotSize = 0;

    public override void __Initialize()
    {        
        m_SlotList = new List<InvenSlot>();

        m_InventoryState = E_InventoryState.Close;

        InitializeSettingSlot();

        if (__GameManager.Instance.m_CurrentStage == 0)
        {
            PushSlotItem(E_ItemType.Stage0_편지);
        }

    }

    enum E_InventoryState
    {
        Open,
        Moving,
        Close
    }

    IEnumerator InventoryMove() //인벤토리 좌우 이동.
    {
        Vector3 InvenPos = m_InventoryPanal.parent.localPosition;  //인벤토리 UI 이동 용.
        Vector3 RightPos = m_RightDirectionUI.localPosition;   //right 방향 시점 변환 버튼 UI 이동 용.

        int direction = (m_InventoryState == E_InventoryState.Open ? -1 : 1);  //좌우 이동의 값 음양 변화.
        E_InventoryState before_state = m_InventoryState;
        m_InventoryState = E_InventoryState.Moving;

        float moveSize = m_InventoryPanal.parent.GetComponent<RectTransform>().sizeDelta.x / 2;   //인벤토리 UI 캔버스의 크기.
        float movement = moveSize / 10;

        for (int i = 0; i < movement; i++)
        {
            //인벤토리 UI 이동.
            InvenPos.x += direction * moveSize / movement;
            m_InventoryPanal.parent.localPosition = InvenPos;
            //right 방향 시점 변환 버튼 UI 이동.
            RightPos.x += direction * moveSize / movement;
            m_RightDirectionUI.localPosition = RightPos;
            yield return null;
        }

        m_InventoryState = before_state; //Moving State 해제.
    }

    public void MouseOverSlot(GameObject _hit)
    {
        int hit_index = _hit.transform.GetComponent<InvenSlot>().m_SlotIndex;

        if (GetSlotItem(hit_index).m_Type != E_ItemType.None)
        {
            m_ItemInfoWindow.position = _hit.transform.position;
            TextMeshProUGUI hit_nametmp = m_ItemInfoWindow.FindChildren("NameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI hit_addtmp = m_ItemInfoWindow.FindChildren("DescriptText").GetComponent<TextMeshProUGUI>();
            hit_nametmp.text = GetSlotItem(hit_index).m_Type.ToString().Replace("__", " ").Split('_')[1];
            hit_addtmp.text = GetSlotItem(hit_index).m_Description;
            m_ItemInfoWindow.gameObject.SetActive(true);
        }
    }
    public void MouseExitSlot()
    {
        m_ItemInfoWindow.position = Vector2.one * -9999;  //TextMeshPro가 Raycast Target 대상이라 마우스 조작에 방해되는데 켜고 끌 수가 없어서 좌표 값을 멀리 보내버림.
        m_ItemInfoWindow.gameObject.SetActive(false);
    }

    public void InventoryOpenButton()   // 인벤토리 열기/닫기 버튼 클릭 시.
    {
        // 이동 중 중복 클릭 방지.
        if (m_InventoryState != E_InventoryState.Moving)
        {
            // 동작할 state 값으로 변환 후 인벤토리 UI 이동 코루틴 실행.
            if (m_InventoryState == E_InventoryState.Close)
            {
                m_InventoryState = E_InventoryState.Open;
            }
            else if (m_InventoryState == E_InventoryState.Open)
            {
                m_InventoryState = E_InventoryState.Close;
            }

            Vector3 tmp = m_InventoryPanal.parent.FindChildren("InventoryButton").localScale;
            tmp.x *= -1;
            m_InventoryPanal.parent.FindChildren("InventoryButton").localScale = tmp;

            StartCoroutine("InventoryMove");
        }

    }

    //get set
    public Item GetSlotItem(int _index)
    {
        return m_SlotList[_index].m_ItemInfo;
    }
    public void SetSlotItem(int _slotindex, E_ItemType _ItemType)
    {
        Item item = ItemDB.Instance.ReturnItemToIndex(_ItemType);

        m_SlotList[_slotindex].m_ItemInfo = item;

        m_SlotList[_slotindex].m_ItemImage.sprite = item.m_Image;
    }

    void InitializeSettingSlot()
    {
        m_SlotList.Clear();

        for (int i = 0; i < m_InventoryPanal.childCount; i++)
        {
            InvenSlot temp = m_InventoryPanal.GetChild(i).GetComponent<InvenSlot>();
            temp.m_SlotIndex = i;
            m_SlotList.Add(temp);
            m_SlotMaxSize++;
        }
    }

    // 유니티 이벤트 용
    public void PushSlotItem(int _ItemIndex)
    {
        PushSlotItem((E_ItemType)_ItemIndex);
    }
    public void PushSlotItem(E_ItemType _ItemType)
    {
        for (int i = 0; i < m_SlotMaxSize; ++i)
        {
            if (m_SlotList[i].m_ItemInfo.m_Type == E_ItemType.None)
            {
                SetSlotItem(i, _ItemType);
                m_SlotList[i].OnItemImage();

                break;
            }
        }
    }
    public void PullSlotItem(int _slotindex)
    {
        //m_SlotList[_slotindex].OffItemImage();
        //SetSlotItem(_slotindex, E_ItemType.None);
        //--m_SlotSize;

        m_SlotList[_slotindex].m_BGImage.color = m_DefaultColor;
        m_SlotList[_slotindex].m_ItemImage.sprite = null;
        SetSlotItem(_slotindex, E_ItemType.None);
        m_SlotList[_slotindex].OffItemImage();

        for (int i = _slotindex; i < m_SlotMaxSize - 1; ++i)
        {
            m_SlotList[i].m_BGImage.color = m_DefaultColor;
            SetSlotItem(i, m_SlotList[i + 1].m_ItemInfo.m_Type);

            if (m_SlotList[i].m_ItemInfo.m_Type == E_ItemType.None)
            {
                m_SlotList[i].OffItemImage();
            }
            else
            {
                m_SlotList[i].OnItemImage();
            }
        }

        m_SlotList[m_SlotMaxSize - 1].m_BGImage.color = m_DefaultColor;
        SetSlotItem(m_SlotMaxSize - 1, E_ItemType.None);
        m_SlotList[m_SlotMaxSize - 1].OffItemImage();

        --m_SlotSize;
    }
    public void UseItem(E_ItemType e_item)
    {
        for (int i = 0; i < m_SlotMaxSize; ++i)
        {
            if (m_SlotList[i].m_ItemInfo.m_Type == e_item)
            {
                if (m_ActivedSlot != null)
                {
                    m_ActivedSlot.m_BGImage.color = m_DefaultColor;
                    m_ActivedSlot = null;
                }

                PullSlotItem(i);
                break;
            }
        }
    }
    public bool HasItem(E_ItemType e_Item)
    {
        for (int i = 0; i < m_SlotMaxSize; ++i)
        {
            if (m_SlotList[i].m_ItemInfo.m_Type == e_Item)
            {
                return true;
            }
        }

        return false;
    }

    public void ClearInventory()
    {
        for (int i = 0; i < m_SlotMaxSize; ++i)
        {
            m_SlotList[i].m_BGImage.color = m_DefaultColor;
            m_SlotList[i].m_ItemImage.sprite = null;
            SetSlotItem(i, E_ItemType.None);
            m_SlotList[i].OffItemImage();
        }
    }

    void SortItemLst()   //인벤토리 아이템 순서 재정렬.
    {
        m_SlotList.Sort(delegate (InvenSlot a, InvenSlot b)
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
}
