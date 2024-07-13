using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private bool m_registered = false;
    private bool m_target = false;
    private int m_orderIndex = -1;

    public bool Registered => m_registered;
    public bool Target => m_target;

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
        Debug.Log(gameObject.name + "에" + m_orderIndex + "주문서 배달 진행");

        m_registered = true;
        m_orderIndex = orderIndex;
    }

    public void Create_UI()
    {
        if (m_target == true)
            return;

        m_target = true;
        Debug.Log(gameObject.name + "에" + m_orderIndex + "주문서 배달 가능");
    }
}
