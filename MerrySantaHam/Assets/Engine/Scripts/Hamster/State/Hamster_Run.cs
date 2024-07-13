using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster_Run : State<Hamster>
{
    private bool m_isLock = false;

    private float m_moveSpeed = 20.0f;

    private float m_currentTime = 0f;
    private float m_maxTime = 1f;

    private Rigidbody m_rigidbody;

    public Hamster_Run(StateMachine<Hamster> stateMachine) : base(stateMachine)
    {
        m_rigidbody = m_stateMachine.Owner.GetComponent<Rigidbody>();
    }

    public override void Enter_State()
    {
        GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);
        GameManager.Ins.Player.OrderSheets.Start_Orders();
    }

    public override void Update_State()
    {
        if (m_isLock == false)
            Input_Player();
    }

    public override void Exit_State()
    {
    }

    private void Input_Player()
    {
        // 일정 시간 이상 입력받지 않을 시
        //if (m_currentTime >= m_maxTime)
        //{
        //    m_rigidbody.velocity = Vector3.zero;
        //    GameManager.Ins.Over_Game();
        //    return;
        //}

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(inputX, 0, inputZ).normalized;

        m_rigidbody.velocity = moveDirection * m_moveSpeed;

        if (inputX != 0 || inputZ != 0)
        {
            m_currentTime = 0;
            m_stateMachine.Owner.transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
            m_currentTime += Time.deltaTime;
    }
}
