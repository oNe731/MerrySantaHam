using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : Interaction
{
    private void Update()
    {
        Update_Timer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 모자 생성
        if (collision.gameObject.name == "Hamster" && m_IsCool == false)
        {
            GameManager.Ins.Player.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Strawberry, 1));
            Start_Timer();
        }
    }
}
