using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMan : Interaction
{
    private void Update()
    {
        Update_Timer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ����
        if (collision.gameObject.name == "Hamster" && m_IsCool == false)
        {
            GameManager.Ins.Player.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Person, 1));
            Start_Timer();
        }
    }
}
