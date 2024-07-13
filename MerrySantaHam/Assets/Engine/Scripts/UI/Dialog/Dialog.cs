using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Dialog : MonoBehaviour
{
    [SerializeField] private List<DialogData> m_dialogs;
    private Coroutine m_dialogTextCoroutine = null;
    private Coroutine m_arrowCoroutine = null;

    private bool m_isTyping = false;
    private bool m_cancelTyping = false;
    private int m_dialogIndex = 0;
    private float m_typeSpeed = 0.05f;
    private float m_arrowSpeed = 0.6f;

    private Image m_cutSceneImg;
    private TMP_Text m_dialogTxt;
    private GameObject m_arrowObj;

    private Dictionary<string, Sprite> m_cutSceneImage = new Dictionary<string, Sprite>();

    private void Awake()
    {
        m_cutSceneImg = GetComponent<Image>();
        m_dialogTxt = transform.GetChild(1).GetComponent<TMP_Text>();
        m_arrowObj = transform.GetChild(2).gameObject;

        m_cutSceneImage.Add("Intro1", Resources.Load<Sprite>("Textures/2D/Intro/Intro1"));
        m_cutSceneImage.Add("Intro2", Resources.Load<Sprite>("Textures/2D/Intro/Intro2"));
        m_cutSceneImage.Add("Intro3", Resources.Load<Sprite>("Textures/2D/Intro/Intro3"));
        m_cutSceneImage.Add("Intro4", Resources.Load<Sprite>("Textures/2D/Intro/Intro4"));
        m_cutSceneImage.Add("Intro5", Resources.Load<Sprite>("Textures/2D/Intro/Intro5"));
        m_cutSceneImage.Add("Intro6", Resources.Load<Sprite>("Textures/2D/Intro/Intro6"));
        m_cutSceneImage.Add("Intro7", Resources.Load<Sprite>("Textures/2D/Intro/Intro7"));
        m_cutSceneImage.Add("Intro8", Resources.Load<Sprite>("Textures/2D/Intro/Intro8"));
        m_cutSceneImage.Add("Intro9", Resources.Load<Sprite>("Textures/2D/Intro/Intro9"));
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            Update_Dialog();
    }

    private void Update_Dialog(bool IsPonter = true)
    {
        bool isUp = EventSystem.current.IsPointerOverGameObject();
        if (IsPonter && isUp == true)
            return;

        if (m_isTyping)
            m_cancelTyping = true;
        else if (!m_isTyping)
        {
            // 다이얼로그 진행
            if (m_dialogIndex < m_dialogs.Count)
            {
                switch (m_dialogs[m_dialogIndex].dialogEvent)
                {
                    case DialogData.DIALOGEVENT_TYPE.DET_NONE:
                        Update_None();
                        break;

                    case DialogData.DIALOGEVENT_TYPE.DET_FADEIN:
                        Update_FadeIn();
                        break;

                    case DialogData.DIALOGEVENT_TYPE.DET_FADEOUT:
                        Update_FadeOut();
                        break;

                    case DialogData.DIALOGEVENT_TYPE.DET_FADEOUTIN:
                        Update_FadeOutIn();
                        break;

                    case DialogData.DIALOGEVENT_TYPE.DET_GAMESTART:
                        Update_StartGame();
                        break;
                }
            }
            else // 다이얼로그 종료
            {
                Close_Dialog();
            }
        }
    }

    #region Update
    private void Update_Basic(int index)
    {
        m_arrowObj.SetActive(false);

        if (!string.IsNullOrEmpty(m_dialogs[index].cutSceneSpr))
            m_cutSceneImg.sprite = m_cutSceneImage[m_dialogs[index].cutSceneSpr];
    }

    private void Update_None()
    {
        Update_Basic(m_dialogIndex);

        if (m_dialogTextCoroutine != null)
            StopCoroutine(m_dialogTextCoroutine);
        m_dialogTextCoroutine = StartCoroutine(Type_Text(m_dialogIndex, m_dialogTxt, m_arrowObj));

        m_dialogIndex++;
    }

    private void Update_FadeIn()
    {
        Update_Basic(m_dialogIndex + 1);

        m_dialogTxt.text = "";
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Next_FadeIn());
    }

    private void Next_FadeIn()
    {
        m_dialogIndex++;
        Update_Dialog(false);
    }

    private void Update_FadeOut()
    {
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black);
    }

    private void Update_FadeOutIn()
    {
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Update_FadeIn(), 0.5f, false);
    }

    private void Update_StartGame()
    {
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Start_Game(), 0.5f, false);
    }

    private void Start_Game()
    {
        Close_Dialog();
        GameManager.Ins.Start_Game();
    }
    #endregion

    #region Common
    public void Start_Dialog(List<DialogData> dialogs = null)
    {
        m_dialogs = dialogs;

        m_isTyping = false;
        m_cancelTyping = false;
        m_dialogIndex = 0;

        gameObject.SetActive(true);
        Update_Dialog(false);
    }

    private void Close_Dialog()
    {
        gameObject.SetActive(false);
    }

    IEnumerator Type_Text(int dialogIndex, TMP_Text currentText, GameObject arrow)
    {
        m_isTyping = true;
        m_cancelTyping = false;

        currentText.text = "";
        foreach (char letter in m_dialogs[dialogIndex].dialogText.ToCharArray())
        {
            if (m_cancelTyping)
            {
                currentText.text = m_dialogs[dialogIndex].dialogText;
                break;
            }

            currentText.text += letter;
            yield return new WaitForSeconds(m_typeSpeed);
        }

        m_isTyping = false;

        if (m_arrowCoroutine != null)
            StopCoroutine(m_arrowCoroutine);
        m_arrowCoroutine = StartCoroutine(Use_Arrow(arrow));

        yield break;
    }

    IEnumerator Use_Arrow(GameObject arrow)
    {
        while (false == m_isTyping)
        {
            arrow.SetActive(!arrow.activeSelf);
            yield return new WaitForSeconds(m_arrowSpeed);
        }

        yield break;
    }
    #endregion

    public void Button_Skip()
    {
        m_cancelTyping = false;
        m_isTyping = false;

        m_dialogIndex = m_dialogs.Count - 1;
        Update_Dialog(false);
    }
}
