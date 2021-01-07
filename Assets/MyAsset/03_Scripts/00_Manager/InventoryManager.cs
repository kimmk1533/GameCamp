using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public Transform InventoryPanal;
    [SerializeField] bool inventory_open = false;
    [Space(10f)]
    [Tooltip("인벤토리 슬롯")]
    [SerializeField] List<ClickNDrag> Slot_lst = new List<ClickNDrag>();
    [SerializeField] List<Item> Slot_item = new List<Item>();
    int slot_max = 0;

    public override void __Initialize()
    {
        SettingSlot();
    }

    IEnumerator InventoryMove() //인벤토리 좌우 이동.
    {
        Vector3 tmp = InventoryPanal.parent.localPosition;
        float moveSize = InventoryPanal.parent.GetComponent<RectTransform>().sizeDelta.x / 2;
        float movement = moveSize / 10;
        for (int i = 0; i < movement; i++)
        {
            tmp.x += (inventory_open == true ? -1 : 1) * moveSize / movement;
            InventoryPanal.parent.localPosition = tmp;
            yield return null;
        }
        InventoryPanal.parent.localPosition = tmp;
    }

    public void MouseEnterSlot(GameObject _hit)
    {
        int hit_index = _hit.transform.GetChild(0).GetComponent<ClickNDrag>().GetThisSlotIndex();
        GetSlotItem(hit_index).LoadingItemToIndex();

        if (hit_index != 0)
        {
            TextMeshPro hit_nametmp = _hit.transform.GetChild(1).GetChild(0).GetComponent<TextMeshPro>();
            TextMeshPro hit_addtmp = _hit.transform.GetChild(1).GetChild(1).GetComponent<TextMeshPro>();
            hit_nametmp.text = GetSlotItem(hit_index).name;
            hit_addtmp.text = GetSlotItem(hit_index).add;
            _hit.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void MouseOverSlot(GameObject _hit)
    {
        _hit.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void InventoryOpenButton()   //인벤토리 열기/닫기 버튼 클릭 시.
    {
        inventory_open = !inventory_open;
        StartCoroutine("InventoryMove");
    }

    //get set
    public Item GetSlotItem(int _index)
    {
        return Slot_item[_index];
    }
    void SettingSlot()
    {
        for (int i = 0; i < InventoryPanal.childCount; i++)
        {
            ClickNDrag tmp = InventoryPanal.GetChild(i).GetChild(0).GetComponent<ClickNDrag>();
            tmp.SetThisSlotIndex(i);
            Slot_lst.Add(tmp);
            Slot_item.Add(new Item());
            slot_max++;
        }
    }

    
}
