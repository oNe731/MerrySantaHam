using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSlot
{
    private int m_index = -1;
    private Order m_order = null;

    private OrderSheets m_orderSheets;
    private UIOrder m_uIOrder = null;


    private Vector2 m_startPosition;

    public int Index => m_index;
    public Order OrderInfo => m_order;
    public OrderSheets OrderSheet => m_orderSheets;
    public UIOrder UI => m_uIOrder;


    public OrderSlot(OrderSheets orderSheets, int index)
    {
        m_orderSheets = orderSheets;
        m_index       = index;

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
        m_startPosition = new Vector2(positionX, 415.75f);
    }

    public void Set_Order(Order OrderInfo = null)
    {
        if(OrderInfo == null)
            m_order = new Order(this);
        else
            m_order = OrderInfo;

        if (m_uIOrder == null)
        {
            GameObject gameObject = GameManager.Ins.Create_GameObject("Prefabs/UI/UIOrder", GameObject.Find("Canvas").transform.GetChild(1).GetChild(2));
            gameObject.SetActive(false);
            m_uIOrder = gameObject.GetComponent<UIOrder>();
            m_uIOrder.Initialize_UIOrder();
        }

        if (m_uIOrder == null)
            return;

        m_uIOrder.gameObject.SetActive(true);
        m_uIOrder.gameObject.GetComponent<RectTransform>().anchoredPosition = m_startPosition;
        m_uIOrder.Set_UIOrder(this);
    }


    public void Check_Order()
    {
        if (m_uIOrder == null)
            return;

        m_uIOrder.Check_Slots();
    }

    public void Use_Order(bool clear)
    {
        // 인벤토리 아이템 사용
        if(clear == true)
            GameManager.Ins.Player.Inventory.Use_Item(m_order.elements);

        Reset_Slot();
    }

    public void Clear_Order()
    {
        m_uIOrder.Close_Move();
    }

    public void Reset_Slot()
    {
        if (m_order != null)
            m_order = null;

        if (m_uIOrder != null)
        {
            if (m_uIOrder.Down == true || m_uIOrder.Clear == true)
                return;

            GameObject gameObject = m_uIOrder.gameObject;
            GameManager.Ins.Destroy_GameObject(ref gameObject);
            m_uIOrder = null;
        }
    }
}
