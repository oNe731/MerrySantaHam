using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int m_slotCount = 5;
    private List<InvenSlot> m_slots = new List<InvenSlot>();

    private Dictionary<string, Sprite> m_itemSprite = new Dictionary<string, Sprite>();

    public List<InvenSlot> Slots => m_slots;
    public Dictionary<string, Sprite> ItemSprite => m_itemSprite;

    private void Start()
    {
        // ���ҽ� �߰�
        m_itemSprite.Add("EM_Tree", Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Wood"));
        m_itemSprite.Add("EM_Cloud", Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Cloud"));
        m_itemSprite.Add("EM_Fish", Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Fish"));
        m_itemSprite.Add("EM_Person", Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_SnowManHat"));
        m_itemSprite.Add("EM_Strawberry", Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Strawberry"));

        // ���� ����
        for (int i = 0; i < m_slotCount; i++)
            m_slots.Add(new InvenSlot(this, i));
    }

    public void Add_Item(Item item)
    {
        // �ߺ� ������ ���� �߰�
        bool sameItem = false;
        for (int i = 0; i < m_slotCount; i++)
        {
            if (m_slots[i].Item != null)
            {
                if (m_slots[i].Item.itemType == item.itemType)
                {
                    sameItem = true;
                    m_slots[i].Add_Item(true, item);
                    break;
                }
            }
        }

        // �ߺ� �ƴ� ������ �߰�
        if (sameItem != true)
        {
            for (int i = 0; i < m_slotCount; i++)
            {
                if (m_slots[i].Item == null)
                {
                    m_slots[i].Add_Item(false, item);
                    break;
                }
            }
        }

        // �ֹ��� ��� Ȯ��
        GameManager.Ins.Player.OrderSheets.Check_Orders(m_slots);
    }

    public void Use_Item(List<Item.ELEMENT> elemnts)
    {
        for (int i = 0; i < m_slotCount; i++)
        {
            for (int j = 0; j < elemnts.Count; ++j)
            {
                if (m_slots[i].Item != null && m_slots[i].Item.itemType == elemnts[j])
                    m_slots[i].Use_Item();
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
            if (slot.Item != null)
                sortedSlots.Add(slot);
        }
        // ������ ������ �� �������� ä���
        for (int i = sortedSlots.Count; i < m_slotCount; i++)
            sortedSlots.Add(null);


        // ���ĵ� ���� ����Ʈ�� �ٽ� ����
        for (int i = 0; i < m_slotCount; i++)
        {
            if (sortedSlots[i] != null && sortedSlots[i].Item != null)
                m_slots[i].Add_Item(false, sortedSlots[i].Item);
            else
                m_slots[i].Reset_Slot();
        }
    }
}
