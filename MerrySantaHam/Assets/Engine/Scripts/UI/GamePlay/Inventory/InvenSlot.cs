using UnityEngine;

public class InvenSlot
{
    private int  m_index = -1;
    private Item m_item = null;

    private Inventory m_inventory = null;
    private UIItem m_uiItem = null;

    public Item Item => m_item;
    public Inventory Inventory { set => m_inventory = value; }

    public InvenSlot(Inventory inventory, int index)
    {
        m_inventory = inventory;
        m_index     = index;

        GameObject gameObject = GameManager.Ins.Create_GameObject("Prefabs/UI/UIItem", GameObject.Find("Canvas").transform.GetChild(1).GetChild(3));
        gameObject.SetActive(false);
        m_uiItem = gameObject.GetComponent<UIItem>();

        float positionY = 0f;
        switch (m_index)
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
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-910f, positionY);
    }

    public void Add_Item(bool increase, Item item)
    {
        if(increase == false)
        {
            m_item = item;

            m_uiItem.gameObject.SetActive(true);
            m_uiItem.Set_ItemInfo(m_inventory, m_item);
        }
        else
        {
            if (m_item.count >= 3) // 최대 개수 제한
                return;

            m_item.count += item.count;
            m_uiItem.Set_ItemInfo(m_inventory, m_item);
        }
    }

    public void Reset_Slot()
    {
        m_item  = null;
        m_uiItem.gameObject.SetActive(false);
    }

    public void Use_Item()
    {
        m_item.count--;

        if(m_item.count <= 0)
            Reset_Slot();
        else
            m_uiItem.Set_ItemInfo(m_inventory, m_item);
    }
}
