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
    private Rigidbody m_rigidbody;

    private GameObject m_call = null;

    public bool IsGrounded => m_isGrounded;
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
        m_rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

        m_stateMachine.Update_State();

        Check_Ground();

        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Cloud, 1));
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Fish, 1));
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Person, 1));
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Strawberry, 1));
        //}
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    m_inventory.Add_Item(new Item(Item.ELEMENT.EM_Tree, 1));
        //}
    }

    private void Check_Ground()
    {
        Vector3 origin    = transform.position;
        Vector3 direction = Vector3.down;
        float distance    = 0.8f;

        RaycastHit hit;
        m_isGrounded = Physics.Raycast(origin, direction, out hit, distance, LayerMask.GetMask("Ground"));
        if (m_isGrounded == false) // 중력
        {
            if (m_stateMachine.CurState == (int)HamsterState.HT_APPEAR)
                return;
            m_rigidbody.velocity += Physics.gravity * Time.deltaTime * 300f; // 떨어지는 속도 : 300f
        }

#if UNITY_EDITOR
        Debug.DrawRay(origin, direction * distance, Color.red);
#endif
    }

    public void Diminish_Life()
    {
        m_life--;
        if (m_life <= 0)
        {
            if (m_call != null)
                Destroy(m_call);
            GameManager.Ins.Over_Game();
        }
        else
        {
            if (m_call == null)
                m_call = GameManager.Ins.Create_GameObject("Prefabs/UI/CallPanel", GameObject.Find("Canvas").transform); // 전화 UI 생성
            m_call.GetComponent<Call>().Reset_Call();
        }
    }

    public void Jump_Player()
    {
        if (GameManager.Ins.Player.IsGrounded == false)
            return;

        m_rigidbody.AddForce(Vector3.up * Random.Range(100f, 300f), ForceMode.Impulse); // 점프 높이 : 100f - 300f;
        Debug.Log("점프");
    }

    public void Add_Acceleration()
    {
        if (GameManager.Ins.Player.IsGrounded == false)
            return;

        Vector3 velocity = m_rigidbody.velocity;
        velocity.y = 0f;
        m_rigidbody.AddForce(velocity * 100f);
        Debug.Log("가속");
    }
}
