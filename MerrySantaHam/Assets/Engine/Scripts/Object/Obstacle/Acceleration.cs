using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceleration : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Hamster")
        {
            // ����
            GameManager.Ins.Player.Add_Acceleration();
        }
    }
}
