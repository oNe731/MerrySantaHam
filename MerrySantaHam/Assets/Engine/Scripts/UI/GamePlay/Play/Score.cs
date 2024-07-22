using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TMP_Text m_scoreTxt;
    private int m_currentScore = -1;

    private void Start()
    {

    }

    private void Update()
    {
        if (m_currentScore != GameManager.Ins.Score)
        {
            m_currentScore = GameManager.Ins.Score;
            m_scoreTxt.text = "x " + m_currentScore.ToString();
        }
    }
}
