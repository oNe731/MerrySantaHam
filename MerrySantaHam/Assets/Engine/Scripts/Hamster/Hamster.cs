using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : MonoBehaviour
{
    public enum HamsterState { HT_APPEAR, HT_RUN, HT_END }


    private int m_life = 3;
    private Call m_callUI = null;

    private StateMachine<Hamster> m_stateMachine;
    private OrderSheets m_orderSheets;
    private Inventory m_inventory;

    public int life => m_life;
    public StateMachine<Hamster> StateMachine => m_stateMachine;
    public OrderSheets OrderSheets => m_orderSheets;
    public Inventory Inventory => m_inventory;


    private void Start()
    {
        // 상태
        m_stateMachine = new StateMachine<Hamster>(gameObject);
        List<State<Hamster>> states = new List<State<Hamster>>();
        states.Add(new Hamster_Appear(m_stateMachine)); // 0
        states.Add(new Hamster_Run(m_stateMachine));    // 1
        m_stateMachine.Initialize_State(states, (int)HamsterState.HT_APPEAR);

        m_orderSheets = gameObject.AddComponent<OrderSheets>();
        m_inventory = gameObject.AddComponent<Inventory>();

        if (m_callUI == null) // 전화 UI 생성
        {
            GameObject gameObject = GameManager.Ins.Create_GameObject("Prefabs/UI/CallPanel", GameObject.Find("Canvas").transform);
            m_callUI = gameObject.GetComponent<Call>();
            m_callUI.gameObject.SetActive(false);
        }

    }

    public void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

        m_stateMachine.Update_State();
    }

    public void Diminish_Life()
    {
        m_life--;
        if (m_life <= 0)
        {
            if (m_callUI != null)
                Destroy(m_callUI.gameObject);
            GameManager.Ins.Over_Game();
        }
        else
        {
            if (m_callUI == null)
                return;
            m_callUI.Reset_Call();
        }
    }
}
