using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Call : MonoBehaviour
{
    private float m_callTime = 0;

    private void Start()
    {
    }

    private void Update()
    {
        // ��ȭ ����
        m_callTime += Time.deltaTime;
        if (m_callTime > 2f)
            gameObject.SetActive(false);
    }

    public void Reset_Call()
    {
        m_callTime = 0f;

        int index = Random.Range(0, 2);
        if(index == 0)
        {
            transform.GetChild(1).GetComponent<TMP_Text>().text = "��Ÿ�ܾ�, ���� ���Ե� �ƴѵ� �̷� �Ǽ��� �ƴ���.";
        }
        else
        {
            transform.GetChild(1).GetComponent<TMP_Text>().text = "...��� ������ ��� ȸ�ǽǿ��� �����?";
        }

        gameObject.SetActive(true);
    }
}
