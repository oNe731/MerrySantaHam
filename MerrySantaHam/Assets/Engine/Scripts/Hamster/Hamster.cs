using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : MonoBehaviour
{
    public enum HamsterState { HT_APPEAR, HT_RUN, HT_END }

    private bool m_isGrounded = false;
    private int m_life = 3;

    private StateMachine<Hamster> m_stateMachine;
    private OrderSheets m_orderSheets;
    private Inventory m_inventory;

    public bool IsGrounded => m_isGrounded;
    public int life => m_life;
    public StateMachine<Hamster> StateMachine => m_stateMachine;
    public OrderSheets OrderSheets => m_orderSheets;
    public Inventory Inventory => m_inventory;

    private void Start()
    {
        // ป๓ลย
        m_stateMachine = new StateMachine<Hamster>(gameObject);
        List<State<Hamster>> states = new List<State<Hamster>>();
        states.Add(new Hamster_Appear(m_stateMachine)); // 0
        states.Add(new Hamster_Run(m_stateMachine));    // 1

        m_stateMachine.Initialize_State(states, (int)HamsterState.HT_APPEAR);

        m_orderSheets = gameObject.AddComponent<OrderSheets>();
        m_inventory = gameObject.AddComponent<Inventory>();
    }

    public void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

        m_stateMachine.Update_State();

        Check_Ground();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Cloud, 1));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Fish, 1));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Person, 1));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Strawberry, 1));
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Tree, 1));
        }
    }

    private void Check_Ground()
    {
        Vector3 origin    = transform.position;
        Vector3 direction = Vector3.down;
        float distance    = 0.6f;

        RaycastHit hit;
        m_isGrounded = Physics.Raycast(origin, direction, out hit, distance, LayerMask.GetMask("Ground"));

#if UNITY_EDITOR
        Debug.DrawRay(origin, direction * distance, Color.red);
#endif
    }

    public void Diminish_Life()
    {
        m_life--;
        if(m_life <= 0)
        {
            GameManager.Ins.Over_Game();
        }
    }
}
