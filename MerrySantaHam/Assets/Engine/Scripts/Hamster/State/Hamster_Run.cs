using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster_Run : Hamster_Base
{
    private float m_moveSpeed = 20.0f;

    private readonly float m_maxTime = 5f;
    private float m_currentTime = 0f;

    public Hamster_Run(StateMachine<Hamster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // ���� �÷��� UI Ȱ��ȭ
        GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);

        // ���� ����� ����
        GameManager.Ins.Player.OrderSheets.Start_Orders();
    }

    public override void Update_State()
    {
        base.Update_State();

        // ���� �ð� �̻� �Է¹��� ���� ��
        if (m_currentTime >= m_maxTime)
        {
            m_rigidbody.velocity = Vector3.zero;
            GameManager.Ins.Over_Game();
            return;
        }

        Input_Player();
    }

    public override void Exit_State()
    {
    }

    private void Input_Player()
    {
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

    public void Jump_Player()
    {
        if (IsGrounded == false)
            return;

        m_rigidbody.AddForce(Vector3.up * Random.Range(100f, 300f), ForceMode.Impulse); // ���� ���� : 100f - 300f;
    }

    public void Add_Acceleration()
    {
        if (IsGrounded == false)
            return;

        Vector3 velocity = m_rigidbody.velocity;
        velocity.y = 0f;

        m_rigidbody.AddForce(velocity * 100f);
    }
}
