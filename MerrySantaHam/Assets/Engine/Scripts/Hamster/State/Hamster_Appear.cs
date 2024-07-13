using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster_Appear : State<Hamster>
{
    private Hamster m_hamster;

    public Hamster_Appear(StateMachine<Hamster> stateMachine) : base(stateMachine)
    {
        m_hamster = m_stateMachine.Owner.GetComponent<Hamster>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
        if (m_hamster.IsGrounded == true)
            m_stateMachine.Change_State((int)Hamster.HamsterState.HT_RUN);
    }

    public override void Exit_State()
    {
        GameObject gameObject = m_stateMachine.Owner.transform.GetChild(0).gameObject;
        GameManager.Ins.Destroy_GameObject(ref gameObject);
    }
}
