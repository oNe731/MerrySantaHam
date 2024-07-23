using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private bool m_registered = false;
    [SerializeField] private bool m_available  = false;
    [SerializeField] private int  m_orderID = -1;

    public bool Registered => m_registered;


    private void Start()
    {
        GameManager.Ins.Houses.Add(this);
    }

    private void LateUpdate()
    {
        if(m_registered == true && GameManager.Ins.Player.OrderSheets.Check_OrdersID(m_orderID) == false)
            Reset_Home();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_available == false || m_orderID == -1)
            return;

        // 배달 완료
        if (collision.gameObject.name == "Hamster")
            GameManager.Ins.Player.OrderSheets.Clear_Order(m_orderID);
    }

    public void Reserve_Target(int orderID)
    {
        m_registered = true;
        m_orderID    = orderID;

        Debug.Log(gameObject.name + "집 예약");
    }

    public void Available_Target(bool available)
    {
        if (m_registered == false || m_orderID == -1)
            return;

        m_available = available;
        if(m_available == true)
            Debug.Log(gameObject.name + "집 배달");
    }

    private void Reset_Home()
    {
        m_registered = false;
        m_available  = false;

        m_orderID = -1;

        Debug.Log(gameObject.name + "집 초기화");
    }
}
