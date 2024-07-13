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
        // �ߺ� ������ ���� �߰�
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

        // �ߺ� �ƴ� ������ �߰�
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

        // �ֹ��� ��� Ȯ��
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

        // �� ������ ������ ������ ���Ը� �߰�
        foreach (InvenSlot slot in m_slots)
        {
            if (!slot.EMPTY)
                sortedSlots.Add(slot);
        }

        // ������ ������ �� �������� ä���
        for (int i = sortedSlots.Count; i < m_slotCount; i++)
            sortedSlots.Add(null);

        // ���ĵ� ���� ����Ʈ�� �ٽ� ����
        for (int i = 0; i < m_slotCount; i++)
        {
            if (sortedSlots[i] != null && sortedSlots[i].EMPTY == false)
                m_slots[i].Add_Item(sortedSlots[i].Item, Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIItem"), GameObject.Find("Canvas").transform));
            else
                m_slots[i].Reset_Slot();
        }
    }
}
