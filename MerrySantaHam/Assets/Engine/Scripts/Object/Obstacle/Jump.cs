using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Hamster")
        {
            // มกวม
            GameManager.Ins.Player.Jump_Player();
        }
    }
}
