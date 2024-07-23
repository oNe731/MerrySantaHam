using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIItem : MonoBehaviour
{
    [SerializeField] Image    m_itemImg;
    [SerializeField] TMP_Text m_countTxt;

    public void Set_ItemInfo(Inventory inventory, Item item)
    {
        m_itemImg.sprite = inventory.ItemSprite[item.itemType.ToString()];
        m_countTxt.text  = "x  " + item.count.ToString() + " / 3";
    }
}
