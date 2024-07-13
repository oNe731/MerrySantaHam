using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster_Run : State<Hamster>
{
    private bool m_isLock = false;

    private float m_moveSpeed = 20.0f;
    private float m_lerpSpeed = 5.0f;

    private Rigidbody m_rigidbody;

    public Hamster_Run(StateMachine<Hamster> stateMachine) : base(stateMachine)
    {
        m_rigidbody = m_stateMachine.Owner.GetComponent<Rigidbody>();
    }

    public override void Enter_State()
    {
        GameManager.Ins.Player.OrderSheets.Add_Order();
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
        // 회전
        //transform.rotation = Camera.main.transform.rotation;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        m_rigidbody.velocity = new Vector3(inputX, 0, inputZ) * m_moveSpeed;

        // 이동
        //Vector3 Velocity = Vector3.zero;
        //if (Input.GetKey(KeyCode.W))
        //    Velocity += transform.forward * m_moveSpeed * Time.deltaTime;
        //else if (Input.GetKey(KeyCode.S))
        //    Velocity += -transform.forward * m_moveSpeed * Time.deltaTime;

        //if (Input.GetKey(KeyCode.D))
        //    Velocity += transform.right * m_moveSpeed * Time.deltaTime;
        //else if (Input.GetKey(KeyCode.A))
        //    Velocity += -transform.right * m_moveSpeed * Time.deltaTime;

        //if (Velocity == Vector3.zero)
        //{
        //    //m_rigidbody.isKinematic = true;
        //}
        //else
        //{
        //    //m_rigidbody.isKinematic = false;
        //    m_rigidbody.velocity = Vector3.Lerp(m_rigidbody.velocity, Velocity, Time.deltaTime * m_lerpSpeed);
        //}
    }

    public void Set_Lock(bool isLock)
    {
        m_isLock = isLock;
        //if (isLock)
        //    m_rigidbody.isKinematic = true;
    }
}
