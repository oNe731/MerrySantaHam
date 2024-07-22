using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIItem : MonoBehaviour
{
    [SerializeField] Image m_itemImg;
    [SerializeField] TMP_Text m_countTxt;

    private Dictionary<string, Sprite> m_elementSpr = null;

    public void Set_Info(Item item)
    {
        if(m_elementSpr == null)
        {
            m_elementSpr = new Dictionary<string, Sprite>();

            Sprite sprite = null;
            switch (item.itemType)
            {
                case Item.ELEMENT.EM_Tree:
                    sprite = Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Wood");
                    break;
                case Item.ELEMENT.EM_Cloud:
                    sprite = Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Cloud");
                    break;
                case Item.ELEMENT.EM_Fish:
                    sprite = Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Fish");
                    break;
                case Item.ELEMENT.EM_Person:
                    sprite = Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_SnowManHat");
                    break;
                case Item.ELEMENT.EM_Strawberry:
                    sprite = Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Strawberry");
                    break;
            }
            m_itemImg.sprite = sprite;
        }

        m_countTxt.text = "x " + item.count.ToString();
    }
}
