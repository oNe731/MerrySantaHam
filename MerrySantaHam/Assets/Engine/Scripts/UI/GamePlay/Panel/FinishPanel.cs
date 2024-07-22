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

    [SerializeField] private Sprite[] m_profile;

    private void Start()
    {
        int level = 0;
        if(GameManager.Ins.Score >= 1000)
        {
            level = 3;
            m_EvaluationTxt.text = "�ų� ȸ�縦���� ����ϴ� ��Ÿ��,\n���Ͽ��� ������ ������ �Բ��ϱ⸦ ���ϴ�!";
            m_Employee.text = "��� ���";
            m_ProfileImage.sprite = m_profile[2];
        }
        else if (GameManager.Ins.Score >= 500)
        {
            level = 2;
            m_EvaluationTxt.text = "���� ũ���������� ������ ����� ��Ÿ��!\n�����ε� ���� ������ּ���!";
            m_Employee.text = "���� ���";
            m_ProfileImage.sprite = m_profile[1];
        }
        else
        {
            level = 1;
            m_EvaluationTxt.text = "����� ������ �ν����� ���� �� ����.\n�ڳ����� ���� �ؾ��� �Ʊ���!!";
            m_Employee.text = "�־��� ���";
            m_ProfileImage.sprite = m_profile[0];
        }

        m_ScoreTxt.text = GameManager.Ins.Score.ToString();
        for (int i = 0; i < level; ++i)
            m_Stars[i].SetActive(true);
    }

    public void Retry_Button()
    {
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.Change_Scene("GamePlay"), 0.0f, false);
    }
}
