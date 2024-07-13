using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSheets : MonoBehaviour
{
    private int m_slotCount = 3;
    private List<OrderSlot> m_slots = new List<OrderSlot>();

    private void Start()
    {
        for (int i = 0; i < m_slotCount; i++)
            m_slots.Add(new OrderSlot(i));
    }

    public void Add_Order(int maxCount = 3)
    {
        for(int i = 0; i < maxCount; ++i)
        {
            // 주문서 추가
            for (int j = 0; j < m_slotCount; j++)
            {
                if (m_slots[i].EMPTY == true)
                {
                    m_slots[i].Create_Order(ref m_slots);
                    break;
                }
            }
        }
    }

    public void Check_Orders(ref List<InvenSlot> slots)
    {
        for(int i = 0; i < m_slots.Count; ++i)
        {
            if (m_slots[i].EMPTY == false)
                m_slots[i].Check_Order(ref slots);
        }
    }

    public void Clear_Order(int orderIndex)
    {
        m_slots[orderIndex].Clear_Order();
    }
}
