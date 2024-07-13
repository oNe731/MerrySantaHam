using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceleration : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Hamster")
        {
            // °¡¼Ó
            GameManager.Ins.Player.Add_Acceleration();
        }
    }
}
