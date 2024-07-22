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
            m_EvaluationTxt.text = "매년 회사를위해 헌신하는 산타햄,\n귀하에게 무궁한 영광이 함께하기를 빕니다!";
            m_Employee.text = "우수 사원";
            m_ProfileImage.sprite = m_profile[2];
        }
        else if (GameManager.Ins.Score >= 500)
        {
            level = 2;
            m_EvaluationTxt.text = "올해 크리스마스도 열심히 노력한 산타햄!\n앞으로도 더욱 노력해주세요!";
            m_Employee.text = "성실 사원";
            m_ProfileImage.sprite = m_profile[1];
        }
        else
        {
            level = 1;
            m_EvaluationTxt.text = "배달한 집보다 부숴버린 집이 더 많군.\n자네한테 먹인 해씨가 아깝네!!";
            m_Employee.text = "최악의 사원";
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
