using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 물고기 생성
        if (collision.gameObject.name == "Hamster")
            GameManager.Ins.Player.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Fish, 1));
    }
}
