using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSheets : MonoBehaviour
{
    private int m_maxSlotCount = 3;
    private List<OrderSlot> m_slots = new List<OrderSlot>();

    private void Start()
    {
        for (int i = 0; i < m_maxSlotCount; i++)
            m_slots.Add(new OrderSlot(this, i));
    }

    public void Start_Orders()
    {
        for (int i = 0; i < m_maxSlotCount; i++)
        {
            if (m_slots[i].EMPTY == true)
                m_slots[i].Create_Order(i, ref m_slots);
        }
    }




    public void Check_Orders(List<InvenSlot> slots)
    {
        for(int i = 0; i < m_slots.Count; ++i)
        {
            if (m_slots[i].EMPTY == false)
                m_slots[i].Check_Order(slots);
        }
    }

    public void Clear_Order(int orderIndex)
    {
        if (orderIndex >= m_slots.Count)
            return;

        if(orderIndex < 0)
        {
            return;
        }

        if (m_slots[orderIndex] == null)
            return;

        m_slots[orderIndex].Clear_Order();
    }

    public void Use_Order(int orderIndex, bool clear)
    {
        Debug.Log(orderIndex + "�ֹ��� ����");

        m_slots[orderIndex].Use_Order(clear);
        Sort_Order();

        if (clear == false)
            GameManager.Ins.Player.Diminish_Life();
    }

    private void Sort_Order()
    {
        List<OrderSlot> sortedSlots = new List<OrderSlot>();

        // �� ������ ������ ������ ���Ը� �߰�
        foreach (OrderSlot slot in m_slots)
        {
            // ������� �ʰ� �������� ���� ���� �����۸� ����
            if (!slot.EMPTY && slot.UI.Down == false && slot.UI.Clear == false)
                sortedSlots.Add(slot);
        }

        // ������ ������ �� �������� ä���
        for (int i = sortedSlots.Count; i < m_maxSlotCount; i++)
            sortedSlots.Add(null);

        // ���ĵ� ���� ����Ʈ�� �ٽ� ����
        for (int i = 0; i < m_maxSlotCount; i++)
        {
            if (sortedSlots[i] != null && sortedSlots[i].EMPTY == false)
                m_slots[i].Set_Order(i, sortedSlots[i].OrderInfo, sortedSlots[i].TargetHouse);
            else
                m_slots[i].Reset_Slot();
        }

        for (int i = 0; i < m_maxSlotCount; i++)
        {
            if (m_slots[i].EMPTY == true)
                m_slots[i].Create_Order(i, ref m_slots); // �ֹ��� �߰� ����
        }
    }
}
