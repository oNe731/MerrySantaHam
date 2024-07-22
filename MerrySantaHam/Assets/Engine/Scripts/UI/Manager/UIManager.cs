using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    private bool m_isFade = false;
    private GameObject m_fadeCanvas;
    private Image m_fadeImg;
    private Coroutine m_fadeCoroutine = null;

    private void Awake()
    {
        m_fadeCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UICanvas"), transform);
        m_fadeImg = m_fadeCanvas.GetComponentInChildren<Image>();

        m_fadeCanvas.SetActive(false);
    }

    public void Start_FadeIn(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeCoroutine(1f, 0f, duration, color, onComplete, waitTime, panalOff));
    }

    public void Start_FadeOut(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeCoroutine(0f, 1f, duration, color, onComplete, waitTime, panalOff));
    }

    public void Start_FadeInOut(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeCoroutine(1f, 0f, duration, color, () => Start_FadeOut(duration, color, onComplete), waitTime, panalOff));
    }

    public void Start_FadeOutIn(float duration, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeCoroutine(0f, 1f, duration, color, () => Start_FadeIn(duration, color, onComplete), waitTime, panalOff));
    }

    public void Start_FadeWaitAction(float startAlpha, Color color, Action onComplete = null, float waitTime = 0f, bool panalOff = true)
    {
        if (m_isFade)
            return;

        if (m_fadeCoroutine != null)
            StopCoroutine(m_fadeCoroutine);
        m_fadeCoroutine = StartCoroutine(FadeWaitAction(startAlpha, color, onComplete, waitTime, panalOff));
    }

    private IEnumerator FadeCoroutine(float startAlpha, float targetAlpha, float duration, Color color, Action onComplete, float waitTime, bool panalOff)
    {
        m_isFade = true;
        m_fadeCanvas.SetActive(true);

        float currentTime = 0f;
        Color startColor = color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float fadeProgress = currentTime / duration;
            m_fadeImg.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startAlpha, targetAlpha, fadeProgress));

            yield return null;
        }

        m_fadeImg.color = targetColor;
        m_isFade = false;

        if (panalOff)
            m_fadeCanvas.SetActive(false);

        if (onComplete != null)
        {
            yield return new WaitForSeconds(waitTime);
            onComplete?.Invoke(); // �ݹ� �Լ� ȣ��
        }

        yield break;
    }

    private IEnumerator FadeWaitAction(float startAlpha, Color color, Action onComplete, float waitTime, bool panalOff)
    {
        m_isFade = true;
        m_fadeCanvas.SetActive(true);

        m_fadeImg.color = new Color(color.r, color.g, color.b, startAlpha);

        yield return new WaitForSeconds(waitTime);

        m_isFade = false;

        if (panalOff)
            m_fadeCanvas.SetActive(false);

        if (onComplete != null)
            onComplete?.Invoke(); // �ݹ� �Լ� ȣ��

        yield break;
    }

}
