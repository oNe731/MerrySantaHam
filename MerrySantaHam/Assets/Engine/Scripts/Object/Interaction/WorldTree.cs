using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTree : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ����
        if (collision.gameObject.name == "Hamster")
            GameManager.Ins.Player.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Tree, 1));
    }
}
