using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int m_slotCount = 5;
    private List<InvenSlot> m_slots = new List<InvenSlot>();

    public List<InvenSlot> Slots => m_slots;

    private void Start()
    {
        for (int i = 0; i < m_slotCount; i++)
            m_slots.Add(new InvenSlot(i));
    }

    public void Add_Item(Item item)
    {
        // 중복 아이템 개수 추가
        bool sameItem = false;
        for (int i = 0; i < m_slotCount; i++)
        {
            if (m_slots[i].EMPTY == false)
            {
                if (m_slots[i].Item.itemType == item.itemType)
                {
                    m_slots[i].Add_Item(item);
                    sameItem = true;
                    break;
                }
            }
        }

        // 중복 아닌 아이템 추가
        if (sameItem != true)
        {
            for (int i = 0; i < m_slotCount; i++)
            {
                if (m_slots[i].EMPTY == true)
                {
                    m_slots[i].Add_Item(item, Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIItem"), GameObject.Find("Canvas").transform));
                    break;
                }
            }
        }

        // 주문서 요소 확인
        GameManager.Ins.Player.OrderSheets.Check_Orders(m_slots);
    }

    public void Use_OrderItem(ref List<Item.ELEMENT> elemnts)
    {
        for (int i = 0; i < m_slotCount; i++)
        {
            if (m_slots[i].EMPTY == false)
            {
                for(int j = 0; j < elemnts.Count; ++j)
                {
                    if (m_slots[i].Item != null &&  m_slots[i].Item.itemType == elemnts[j])
                        m_slots[i].Use_Item();
                }
            }
        }

        Sort_Inventory();
    }

    private void Sort_Inventory()
    {
        List<InvenSlot> sortedSlots = new List<InvenSlot>();

        // 빈 슬롯을 제외한 아이템 슬롯만 추가
        foreach (InvenSlot slot in m_slots)
        {
            if (!slot.EMPTY)
                sortedSlots.Add(slot);
        }

        // 나머지 슬롯을 빈 슬롯으로 채우기
        for (int i = sortedSlots.Count; i < m_slotCount; i++)
            sortedSlots.Add(null);

        // 정렬된 슬롯 리스트를 다시 설정
        for (int i = 0; i < m_slotCount; i++)
        {
            if (sortedSlots[i] != null && sortedSlots[i].EMPTY == false)
                m_slots[i].Add_Item(sortedSlots[i].Item, Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIItem"), GameObject.Find("Canvas").transform));
            else
                m_slots[i].Reset_Slot();
        }
    }
}
