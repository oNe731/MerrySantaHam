using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    private Vector3 m_offset;

    void Start()
    {
        m_offset = transform.position;
    }

    void Update()
    {
        if (GameManager.Ins.Player == null)
            return;

        transform.position = new Vector3(GameManager.Ins.Player.transform.position.x + m_offset.x, m_offset.y, GameManager.Ins.Player.transform.position.z + m_offset.z);
    }
}
