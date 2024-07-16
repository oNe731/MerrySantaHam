using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Interaction
{
    private void Update()
    {
        Update_Timer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 물고기 생성
        if (collision.gameObject.name == "Hamster" && m_IsCool == false)
        {
            GameManager.Ins.Player.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Fish, 1));
            Start_Timer();
        }
    }
}
