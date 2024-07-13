using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSlot
{
    private int m_index = -1;
    private bool m_empty = true;
    private Order m_order = null;
    private GameObject m_uIItem = null;

    private House m_targetHouse = null;

    public bool EMPTY => m_empty;
    public Order OrderInfo => m_order;
    public House TargetHouse => m_targetHouse;

    public OrderSlot(int index)
    {
        m_index = index;
    }

    public void Create_Order(ref List<OrderSlot> orderSlots)
    {
        Reset_Slot();

        while(true)
        {
            m_order = new Order();
            if(m_index <= 0)
                break;

            if (orderSlots[m_index - 1].m_order.objectType != m_order.objectType)
                break;
        }

        m_uIItem = GameManager.Ins.Create_GameObject("Prefabs/UI/UIOrder", GameObject.Find("Canvas").transform);
        m_uIItem.GetComponent<RectTransform>().anchoredPosition = Get_Position();
        m_uIItem.GetComponent<UIOrder>().Initialize_UIOrder(this, m_order);

        m_targetHouse = GameManager.Ins.Create_Target(m_index);

        m_empty = false;
    }

    public void Reset_Slot()
    {
        m_empty = true;
        m_order = null;

        if (m_uIItem != null)
            GameManager.Ins.Destroy_GameObject(ref m_uIItem);
    }

    private Vector2 Get_Position()
    {
        float positionX = 0f;
        switch (m_index)
        {
            case 0:
                positionX = -850f;
                break;
            case 1:
                positionX = -635f;
                break;
            case 2:
                positionX = -420f;
                break;
        }

        return new Vector2(positionX, 415.75f);
    }

    public void Check_Order(ref List<InvenSlot> slots)
    {
        if (m_uIItem == null)
            return;

        m_uIItem.GetComponent<UIOrder>().Check_Slots(ref slots);
    }

    public void Clear_Order()
    {
        m_uIItem.GetComponent<UIOrder>().Close_Move();
    }
}
