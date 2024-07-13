using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_MethodPanel;

    public void Button_Start()
    {
        Destroy(gameObject);
        transform.parent.GetChild(2).GetComponent<Dialog>().Start_Dialog(GameManager.Ins.Load_JsonData<DialogData>("Data/Intro"));
    }

    public void Button_Method()
    {
        m_MethodPanel.SetActive(!m_MethodPanel.activeSelf);
    }

    public void Button_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
