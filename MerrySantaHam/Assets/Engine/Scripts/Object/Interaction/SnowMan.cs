using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMan : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ����
        if (collision.gameObject.name == "Hamster")
            GameManager.Ins.Player.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Person, 1));
    }
}
