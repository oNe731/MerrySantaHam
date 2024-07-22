public class Hamster_Appear : Hamster_Base
{
    public Hamster_Appear(StateMachine<Hamster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // ���ϻ� ������Ʈ Ȱ��ȭ
        if(m_stateMachine.Owner.transform.GetChild(0) != null)
            m_stateMachine.Owner.transform.GetChild(0).gameObject.SetActive(true);
    }

    public override void Update_State()
    {
        base.Update_State();

        if (IsGrounded == true)
            m_stateMachine.Change_State((int)Hamster.HamsterState.HT_RUN);
    }

    public override void Exit_State()
    {
        // ���ϻ� ������Ʈ ��Ȱ��ȭ
        if (m_stateMachine.Owner.transform.GetChild(0) != null)
            m_stateMachine.Owner.transform.GetChild(0).gameObject.SetActive(false);
    }
}
