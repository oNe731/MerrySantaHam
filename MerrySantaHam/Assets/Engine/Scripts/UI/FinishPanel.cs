using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text m_ScoreTxt;
    [SerializeField] private TMP_Text m_EvaluationTxt;
    [SerializeField] private TMP_Text m_Employee;
    [SerializeField] private Image m_ProfileImage;
    [SerializeField] private GameObject[] m_Stars;

    private void Start()
    {
        int level = 0;
        if(GameManager.Ins.Score >= 1000)
        {
            level = 3;
            m_EvaluationTxt.text = "�� ������� �ڳ״� ����� õ����.";
            m_Employee.text = "��� ���";
            //m_ProfileImage.sprite;
        }
        else if (GameManager.Ins.Score >= 500)
        {
            level = 2;
            m_EvaluationTxt.text = "���ݸ� �� ����ϸ� �ǰڳ�.";
            m_Employee.text = "���� ���";
        }
        else
        {
            level = 1;
            m_EvaluationTxt.text = "�ڳ״� ������� ����.";
            m_Employee.text = "�־��� ���";
        }

        m_ScoreTxt.text = GameManager.Ins.Score.ToString();
        for (int i = 0; i < level; ++i)
            m_Stars[i].SetActive(true);
    }
}
