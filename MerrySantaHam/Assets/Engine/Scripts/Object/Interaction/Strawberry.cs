using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 모자 생성
        if (collision.gameObject.name == "Hamster")
            GameManager.Ins.Player.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Strawberry, 1));
    }
}
