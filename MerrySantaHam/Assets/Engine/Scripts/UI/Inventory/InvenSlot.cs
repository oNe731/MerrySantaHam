using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    private int m_index = -1;
    private bool m_empty = true;
    private Item m_item = null;
    private GameObject m_uIItem = null;

    private Inventory m_inventory = null;

    public bool EMPTY => m_empty;
    public Item Item => m_item;
    public Inventory Inventory { set => m_inventory = value; }

    public InvenSlot(int index)
    {
        m_index = index;
    }

    public void Add_Item(Item item, GameObject gameObject = null)
    {
        if (gameObject != null) // 아이템 추가
        {
            Reset_Slot();
            m_uIItem = gameObject;
            m_item = item;

            // 위치 지정
            float positionY = 0f;
            switch(m_index)
            {
                case 0:
                    positionY = 255f;
                    break;
                case 1:
                    positionY = 185f;
                    break;
                case 2:
                    positionY = 115f;
                    break;
                case 3:
                    positionY = 45f;
                    break;
                case 4:
                    positionY = -25f;
                    break;
            }
            m_uIItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(-910f, positionY);

            m_empty = false;
        }
        else
        {
            if (m_item.count >= 3)
                return;
            m_item.count += item.count;
        }

        m_uIItem.GetComponent<UIItem>().Set_Info(m_item);
    }

    public void Reset_Slot()
    {
        m_empty = true;
        m_item = null;

        if (m_uIItem != null)
            GameManager.Ins.Destroy_GameObject(ref m_uIItem);
    }
}
