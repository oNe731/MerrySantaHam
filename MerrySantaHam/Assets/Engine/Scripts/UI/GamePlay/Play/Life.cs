using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Life : MonoBehaviour
{
    [SerializeField] TMP_Text m_lifeTxt;
    private int m_currentLife = -1;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(m_currentLife != GameManager.Ins.Player.life)
        {
            m_currentLife = GameManager.Ins.Player.life;
            m_lifeTxt.text = "x " + m_currentLife.ToString();
        }
    }
}
