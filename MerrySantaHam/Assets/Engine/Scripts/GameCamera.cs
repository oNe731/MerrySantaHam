using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    private Vector3 m_offset;

    void Start()
    {
        m_offset = transform.position;
    }

    void Update()
    {
        if (m_target == null)
            return;

        transform.position = new Vector3(m_target.transform.position.x + m_offset.x, m_offset.y, m_target.transform.position.z + m_offset.z);
    }
}
