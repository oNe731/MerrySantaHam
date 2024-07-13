using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSlot
{
    private int m_index = -1;
    private bool m_empty = true;
    private Order m_order = null;
    private GameObject m_uIItem = null;

    private OrderSheets m_orderSheets;
    private House m_targetHouse = null;

    public int Index => m_index;
    public bool EMPTY => m_empty;
    public Order OrderInfo => m_order;
    public UIOrder UI => m_uIItem.GetComponent<UIOrder>();
    public OrderSheets OrderSheet => m_orderSheets;
    public House TargetHouse => m_targetHouse;

    public OrderSlot(OrderSheets orderSheets,int index)
    {
        m_orderSheets = orderSheets;
        m_index = index;
    }

    public void Create_Order(int index, ref List<OrderSlot> orderSlots)
    {
        Reset_Slot();

        m_index = index;
        m_order = new Order(ref orderSlots);

        m_uIItem = GameManager.Ins.Create_GameObject("Prefabs/UI/UIOrder", GameObject.Find("Canvas").transform);
        m_uIItem.GetComponent<RectTransform>().anchoredPosition = Get_Position();
        m_uIItem.GetComponent<UIOrder>().Initialize_UIOrder(this);

        m_targetHouse = GameManager.Ins.Create_Target(m_index);

        m_empty = false;
    }

    public void Set_Order(int index, Order OrderInfo, House house)
    {
        Reset_Slot();

        m_index = index;
        m_order = OrderInfo;

        m_uIItem = GameManager.Ins.Create_GameObject("Prefabs/UI/UIOrder", GameObject.Find("Canvas").transform);
        m_uIItem.GetComponent<RectTransform>().anchoredPosition = Get_Position();
        m_uIItem.GetComponent<UIOrder>().Initialize_UIOrder(this);

        m_targetHouse = house;
        m_targetHouse.Registered_Target(m_index);

        m_empty = false;
    }

    public void Reset_Slot()
    {
        m_empty = true;
        m_order = null;

        if (m_uIItem != null)
        {
            if (UI.Down == true || UI.Clear == true)
                return;

            GameManager.Ins.Destroy_GameObject(ref m_uIItem);
        }
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

    public void Check_Order(List<InvenSlot> slots)
    {
        if (m_uIItem == null)
            return;

        m_uIItem.GetComponent<UIOrder>().Check_Slots(slots);
    }

    public void Clear_Order()
    {
        m_uIItem.GetComponent<UIOrder>().Close_Move();
    }

    public void Use_Order(bool clear)
    {
        // 인벤토리 아이템 사용
        if(clear == true)
            GameManager.Ins.Player.Inventory.Use_OrderItem(ref m_order.elements);

        Reset_Slot();

        m_targetHouse.Reset_Home(clear);
        m_targetHouse = null;
    }
}
