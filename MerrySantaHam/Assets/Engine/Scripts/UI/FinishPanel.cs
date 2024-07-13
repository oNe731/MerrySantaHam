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
            m_EvaluationTxt.text = "이 점수라니 자네는 배달의 천재라네.";
            m_Employee.text = "우수 사원";
            //m_ProfileImage.sprite;
        }
        else if (GameManager.Ins.Score >= 500)
        {
            level = 2;
            m_EvaluationTxt.text = "조금만 더 노력하면 되겠네.";
            m_Employee.text = "성실 사원";
        }
        else
        {
            level = 1;
            m_EvaluationTxt.text = "자네는 배달하지 말게.";
            m_Employee.text = "최악의 사원";
        }

        m_ScoreTxt.text = GameManager.Ins.Score.ToString();
        for (int i = 0; i < level; ++i)
            m_Stars[i].SetActive(true);
    }
}
