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
            Hamster_Run state = (Hamster_Run)GameManager.Ins.Player.StateMachine.CurState;
            if (state == null)
                return;
            state.Add_Acceleration();
        }
    }
}
