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
            Hamster_Run state = (Hamster_Run)GameManager.Ins.Player.StateMachine.CurState;
            if (state == null)
                return;
            state.Jump_Player();
        }
    }
}
