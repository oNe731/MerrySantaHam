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
        // 전화 종료
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
            transform.GetChild(1).GetComponent<TMP_Text>().text = "산타햄씨, 이제 신입도 아닌데 이런 실수는 아니죠.";
        }
        else
        {
            transform.GetChild(1).GetComponent<TMP_Text>().text = "...배달 끝나고 잠시 회의실에서 볼까요?";
        }

        gameObject.SetActive(true);
    }
}
