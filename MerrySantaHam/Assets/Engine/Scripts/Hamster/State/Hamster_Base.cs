using UnityEngine;

public class Hamster_Base : State<Hamster>
{
    private bool m_isGrounded = false;
    public bool IsGrounded => m_isGrounded;

    protected Rigidbody m_rigidbody;

    public Hamster_Base(StateMachine<Hamster> stateMachine) : base(stateMachine)
    {
        m_rigidbody = m_stateMachine.Owner.GetComponent<Rigidbody>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
        Check_Ground();

        if (m_isGrounded == false) // 중력
        {
            if (m_stateMachine.CurStateIndex == (int)Hamster.HamsterState.HT_APPEAR)
                return;
            m_rigidbody.velocity += Physics.gravity * Time.deltaTime * 300f; // 떨어지는 속도 : 300f
        }
    }

    public override void Exit_State()
    {
    }

    private void Check_Ground()
    {
        Vector3 origin    = m_stateMachine.Owner.transform.position;
        Vector3 direction = Vector3.down;
        float   distance  = 0.8f;

        RaycastHit hit;
        m_isGrounded = Physics.Raycast(origin, direction, out hit, distance, LayerMask.GetMask("Ground"));

#if UNITY_EDITOR
        Debug.DrawRay(origin, direction * distance, Color.red);
#endif
    }
}
