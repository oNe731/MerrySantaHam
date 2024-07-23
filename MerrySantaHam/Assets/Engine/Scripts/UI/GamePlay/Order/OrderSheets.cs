using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSheets : MonoBehaviour
{
    private int m_maxSlotCount = 3;
    private List<OrderSlot> m_slots = new List<OrderSlot>();

    private Dictionary<string, Sprite> m_orderSprite = new Dictionary<string, Sprite>();

    public List<OrderSlot> Slots => m_slots;
    public Dictionary<string, Sprite> OrderSprite => m_orderSprite;

    private void Start()
    {
        // ���ҽ� �߰�
        m_orderSprite.Add("OJ_VoodooDoll",     Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/BadDoll"));
        m_orderSprite.Add("OJ_FishTanghulu",   Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/FishSugar"));
        m_orderSprite.Add("OJ_SmallFishTank",  Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/FishTank"));
        m_orderSprite.Add("OJ_SantaHat",       Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/SantaHat"));
        m_orderSprite.Add("OJ_StrawberryFish", Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/StrawberryFish"));

        // ���� ����
        for (int i = 0; i < m_maxSlotCount; i++)
            m_slots.Add(new OrderSlot(this, i));
    }

    public void Start_Orders()
    {
        // ����� 3�� Ȱ��ȭ
        for (int i = 0; i < m_maxSlotCount; i++)
        {
            if (m_slots[i].OrderInfo == null)
                m_slots[i].Set_Order();
        }
    }

    public void Use_Order(int orderIndex, bool clear)
    {
        m_slots[orderIndex].Use_Order(clear);
        Sort_Order();

        if (clear == false)
            GameManager.Ins.Player.Diminish_Life();
    }

    public void Sort_Order()
    {
        List<OrderSlot> sortedSlots = new List<OrderSlot>();

        // �� ������ ������ ������ ���Ը� �߰�
        foreach (OrderSlot slot in m_slots)
        {
            if (slot.OrderInfo != null && slot.UI != null)
            {
                // ������� �ʰ� �������� ���� ���� �����۸� ����
                if (slot.UI.Down == false && slot.UI.Clear == false)
                    sortedSlots.Add(slot);
            }
        }
        // ������ ������ �� �������� ä���
        for (int i = sortedSlots.Count; i < m_maxSlotCount; i++)
            sortedSlots.Add(null);


        // ���ĵ� ���� ����Ʈ�� �ٽ� ����
        for (int i = 0; i < m_maxSlotCount; i++)
        {
            if (sortedSlots[i] != null && sortedSlots[i].OrderInfo != null)
                m_slots[i].Set_Order(sortedSlots[i].OrderInfo);
            else
                m_slots[i].Reset_Slot();
        }

        for (int i = 0; i < m_maxSlotCount; i++)
        {
            if (m_slots[i].OrderInfo == null)
            {
                m_slots[i].Set_Order(); // �ֹ��� �߰� ����
            }
        }

        // �ֹ��� ��� Ȯ��
        GameManager.Ins.Player.OrderSheets.Check_Orders(GameManager.Ins.Player.Inventory.Slots);
    }


    public void Check_Orders(List<InvenSlot> slots)
    {
        for(int i = 0; i < m_slots.Count; ++i)
        {
            if (m_slots[i].OrderInfo != null)
                m_slots[i].Check_Order();
        }
    }

    public bool Check_OrdersID(int ID)
    {
        for(int i = 0; i < m_slots.Count; ++i)
        {
            if (m_slots[i].OrderInfo != null)
            {
                if(m_slots[i].OrderInfo.orderID == ID)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void Clear_Order(int orderID) // ������� ��� �Ϸ� �� ȣ��
    {
        for (int i = 0; i < m_slots.Count; ++i)
        {
            if (m_slots[i].OrderInfo != null)
            {
                if (m_slots[i].OrderInfo.orderID == orderID)
                {
                    m_slots[i].Clear_Order();
                }
            }
        }
    }
}
