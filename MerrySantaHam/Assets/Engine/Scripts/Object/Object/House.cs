using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private bool m_registered = false;
    [SerializeField] private bool m_target = false;
    [SerializeField] private int m_orderIndex = -1;

    private GameObject m_targetObject;

    public bool Registered => m_registered;
    public bool Target => m_target;
    public int OrderIndex { get => m_orderIndex; set => m_orderIndex = value; }


        public GameObject TargetObject => m_targetObject;

    private void Start()
    {
        GameManager.Ins.Houses.Add(this);
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_target == false)
            return;

        // 배달 완료
        if (collision.gameObject.name == "Hamster")
        {
            GameManager.Ins.Player.OrderSheets.Clear_Order(m_orderIndex);

            m_registered = false;
            m_target = false;
            m_orderIndex = -1;
        }
    }

    public void Registered_Target(int orderIndex)
    {
        m_registered = true;
        m_orderIndex = orderIndex;

        Debug.Log(gameObject.name + "에" + m_orderIndex + "주문서 배달 진행");
    }

    public void Create_UI()
    {
        if (m_target == true)
            return;

        m_target = true;
        //Debug.Log(gameObject.name + "에" + m_orderIndex + "주문서 배달 가능");

        m_targetObject = GameManager.Ins.Create_GameObject("Prefabs/UI/UITarget", GameObject.Find("Canvas").transform);
        if(m_targetObject != null)
            m_targetObject.GetComponent<Target>().TargetObject = transform;
    }

    public void Reset_Home(bool clear)
    {
        m_registered = false;
        m_target = false;

        m_orderIndex = -1;

        if (m_targetObject != null)
            Destroy(m_targetObject);

        Debug.Log(gameObject.name + "집 초기화");
    }
}
