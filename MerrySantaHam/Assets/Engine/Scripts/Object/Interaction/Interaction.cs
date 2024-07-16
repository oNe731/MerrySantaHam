using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    protected private bool m_IsCool = false;

    protected private float m_coolTime = 2f;
    protected private float m_time = 0f;

    protected void Start_Timer()
    {
        m_IsCool = true;
        m_time = 0f;
    }

    protected void Update_Timer()
    {
        if (m_IsCool == false)
            return;

        m_time += Time.deltaTime;
        if(m_time >= m_coolTime)
            m_IsCool = false;
    }
}
