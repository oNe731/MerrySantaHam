using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UIOrder : MonoBehaviour
{
    private List<GameObject> m_slots = new List<GameObject>();

    private Image m_timerImg;
    private RectTransform m_rectTransform;

    private Target m_uItarget = null;

    private Quaternion m_startRotation;

    Sprite activeSprite;
    Sprite inactiveSprite;
    //

    private Coroutine m_coroutine = null;
    private bool m_timeOver = false;
    private bool m_down = false;
    private bool m_clear = false;
    private bool m_over = false;
    //

    private OrderSlot m_orderSlot = null;
    private Vector3 m_startPosition;
    private Vector3 m_targetPosition;
    private float m_heightArc; // 포물선의 높이
    private float m_moveSpeed; // 이동 속도
    private Quaternion m_targetRotation;
    private float m_rotationSpeed = 500.0f; // 회전 속도

    //
    private float m_minShakeTime   = 0.5f;
    private float m_maxShakeTime   = 0.8f;
    private float m_minShakeAmount = 500f;
    private float m_maxShakeAmount = 1000f;

    private float m_minTargetX = -100f;
    private float m_maxTargetX = 100f;
    private float m_setTargetY = -700f;
    private float m_minHeight = 400f;
    private float m_maxHeight = 500f;

    private float m_minMoveSpeed = 500f;
    private float m_maxMoveSpeed = 800f;

    private bool  m_rotation = false;
    private float m_minAngle = -70f;
    private float m_maxAngle = 70f;
    private float m_minRotationSpeed = -50f;
    private float m_maxRotationSpeed = 50f;

    private float m_shakeTime   = 0.5f;
    private float m_shakeAmount = 1000f;

    public bool Down 
    {
        get => m_down;
        set => m_down = value; 
    }
    public bool Clear { get => m_clear; }
    public Target TargetUI => m_uItarget;

    public void Initialize_UIOrder()
    {
        for (int i = 0; i < 3; ++i)
        {
            m_slots.Add(GameManager.Ins.Create_GameObject("Prefabs/UI/OrderSlot", transform.GetChild(3)));
        }

        m_timerImg      = transform.GetChild(0).GetComponent<Image>();
        m_rectTransform = GetComponent<RectTransform>();
        m_startRotation = m_rectTransform.rotation;

        // 리소스 로드 최적화
        activeSprite = Resources.Load<Sprite>("Textures/2D/UI/Order/Slot/UI_order_Element_Uses");
        inactiveSprite = Resources.Load<Sprite>("Textures/2D/UI/Order/Slot/UI_order_Element");

        // 화살표 생성
        GameObject gameobject = GameManager.Ins.Create_GameObject("Prefabs/UI/UITarget", transform);
        gameobject.SetActive(false);
        m_uItarget = gameobject.GetComponent<Target>();
    }

    public void Set_UIOrder(OrderSlot orderSlot)
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
        m_timeOver = false;
        m_down     = false;
        m_clear    = false;
        m_over     = false;


        m_orderSlot = orderSlot; 

        // 배달 아이템 이미지 할당
        transform.GetChild(1).GetComponent<Image>().sprite = orderSlot.OrderSheet.OrderSprite[m_orderSlot.OrderInfo.objectType.ToString()];

        // 요소 이미지 할당
        for (int i = 0; i < m_orderSlot.OrderInfo.elements.Count; ++i)
        {
            if(i >= m_slots.Count || m_slots[i] == null)
                break;

            m_slots[i].transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Ins.Player.Inventory.ItemSprite[m_orderSlot.OrderInfo.elements[i].ToString()];
            m_slots[i].transform.GetChild(0).gameObject.SetActive(true);
        }

        Update_TimerColor();
        Set_Value();

        m_startPosition = m_rectTransform.anchoredPosition;

        Check_Slots();
    }


    private void Set_Value()
    {
        m_shakeTime = UnityEngine.Random.Range(m_minShakeTime, m_maxShakeTime);
        m_shakeAmount = UnityEngine.Random.Range(m_minShakeAmount, m_maxShakeAmount);

        m_targetPosition = new Vector3(UnityEngine.Random.Range(m_minTargetX, m_maxTargetX), m_setTargetY, 0);
        m_heightArc = UnityEngine.Random.Range(m_minHeight, m_maxHeight);
        m_moveSpeed = UnityEngine.Random.Range(m_minMoveSpeed, m_maxMoveSpeed);

        m_targetRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(m_minAngle, m_maxAngle));
        m_rotationSpeed = UnityEngine.Random.Range(m_minRotationSpeed, m_maxRotationSpeed);
    }

    private void Update()
    {
        if (GameManager.Ins.IsGame == false || m_clear == true)
            return;

        if (m_timeOver == false)
            Update_Timer();
        else
            Update_Move();
    }

    private void Update_Timer()
    {
        m_orderSlot.OrderInfo.currentTimer -= Time.deltaTime;
        if (m_orderSlot.OrderInfo.currentTimer <= 0) // 실패
        {
            m_orderSlot.OrderInfo.currentTimer = 0;
            Shake_Object();
        }

        Update_TimerColor();
    }

    private void Update_TimerColor()
    {
        if (m_timerImg == null)
            return;

        m_timerImg.fillAmount = m_orderSlot.OrderInfo.currentTimer / m_orderSlot.OrderInfo.maxTimer;

        Color color;
        if (m_timerImg.fillAmount > 0.75f) // 초록에서 노랑
            color = Color.Lerp(Color.yellow, Color.green, (m_timerImg.fillAmount - 0.75f) * 4);
        else if (m_timerImg.fillAmount > 0.5f) // 노랑에서 주황
            color = Color.Lerp(new Color(1.0f, 0.5f, 0.0f), Color.yellow, (m_timerImg.fillAmount - 0.5f) * 4);
        else if (m_timerImg.fillAmount > 0.25f) // 주황에서 빨강
            color = Color.Lerp(Color.red, new Color(1.0f, 0.5f, 0.0f), (m_timerImg.fillAmount - 0.25f) * 4);
        else // 빨강
            color = Color.red;

        m_timerImg.color = color;
    }



    private void Update_Move()
    {
        if (m_down == false) 
            return;

        float nextX = Mathf.MoveTowards(m_rectTransform.anchoredPosition.x, m_targetPosition.x, m_moveSpeed * Time.deltaTime);
        float distance = m_targetPosition.x - m_startPosition.x;
        float baseY = Mathf.Lerp(m_startPosition.y, m_targetPosition.y, (nextX - m_startPosition.x) / distance);
        float arc = m_heightArc * (nextX - m_startPosition.x) * (nextX - m_targetPosition.x) / (-0.25f * distance * distance);
        m_rectTransform.anchoredPosition = new Vector2(nextX, baseY + arc);

        // 회전
        if (m_rotation == true)
        {
            float step = m_rotationSpeed * Time.deltaTime;
            m_rectTransform.rotation = Quaternion.RotateTowards(m_rectTransform.rotation, m_targetRotation, step);
        }

        // 다 떨어지면 오브젝트 삭제
        if (Mathf.Approximately(nextX, m_targetPosition.x) && Mathf.Approximately(baseY + arc, m_targetPosition.y)) 
        {
            if (m_over == true)
                return;

            m_over = true;
            m_orderSlot.OrderSheet.Use_Order(m_orderSlot.Index, false);
        }
    }

    public void Shake_Object(Action isAction = null)
    {
        if (m_timeOver == true)
            return;

        m_uItarget.Reset_Arrow();
        m_orderSlot.OrderInfo.targetHouse.Available_Target(false);

        m_timeOver = true;
        m_coroutine = StartCoroutine(Shake(isAction));
    }

    public IEnumerator Shake(Action isAction)
    {
        float timer = 0;
        while (timer <= m_shakeTime)
        {
            if (m_rectTransform == null)
                break;

            timer += Time.deltaTime;

            Vector3 randomPoint = m_startPosition + UnityEngine.Random.insideUnitSphere * m_shakeAmount;
            m_rectTransform.anchoredPosition = Vector2.Lerp(m_startPosition, randomPoint, Time.deltaTime);
            yield return null;
        }

        if (m_rectTransform != null)
            m_rectTransform.anchoredPosition = m_startPosition;

        if (isAction != null)
            isAction?.Invoke();

        m_down = true;

        yield break;
    }




    public void Close_Move()
    {
        if (m_clear == true)
            return;

        m_clear = true;
        m_coroutine = StartCoroutine(Coroutine_Move(m_startPosition, new Vector3(m_startPosition.x, m_startPosition.y + 250f, m_startPosition.z), 0.5f, false));
    }

    IEnumerator Coroutine_Move(Vector3 startPosition, Vector3 targetPosition, float duration, bool active)
    {
        float time = 0f;
        while (time < duration)
        {
            m_rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        m_rectTransform.anchoredPosition = targetPosition;
        Clear_Order();

        yield break;
    }

    private void Clear_Order()
    {
        // 인벤토리 해당 요소 삭제 및 정렬
        m_orderSlot.OrderSheet.Use_Order(m_orderSlot.Index, true);

        // 점수 증가
        if (m_orderSlot.OrderInfo.level >= 3)
            GameManager.Ins.Score += 200;
        else if (m_orderSlot.OrderInfo.level >= 2)
            GameManager.Ins.Score += 150;
        else
            GameManager.Ins.Score += 100;
    }



    public void Check_Slots()
    {
        if (m_timeOver == true || m_clear == true)
            return;

        List<Item.ELEMENT> elements = m_orderSlot.OrderInfo.elements;
        List<InvenSlot> inven = GameManager.Ins.Player.Inventory.Slots;
        if (elements == null || inven == null)
            return;

        // 요소의 등장 횟수를 추적
        Dictionary<Item.ELEMENT, int> elementOccurrences = new Dictionary<Item.ELEMENT, int>();

        // 주문서의 엘레멘트 요소 검사
        int sameCount = 0;
        for (int i = 0; i < elements.Count; ++i)
        {
            bool elementFound = false;
            Item.ELEMENT currentElement = elements[i];

            // 현재 요소 등장 횟수 증가
            if (!elementOccurrences.ContainsKey(currentElement))
                elementOccurrences[currentElement] = 1;
            else
                elementOccurrences[currentElement]++;

            // 인벤토리의 아이템과 비교
            int requiredCount = elementOccurrences[currentElement]; // 현재 요소가 몇 번째 중복 요소인지
            for (int j = 0; j < inven.Count; ++j)
            {
                if (inven[j].Item != null && inven[j].Item.itemType == currentElement)
                {
                    if (inven[j].Item.count >= requiredCount)
                    {
                        elementFound = true;
                        break;
                    }
                }
            }

            if (elementFound)
            {
                m_slots[i].GetComponent<Image>().sprite = activeSprite;
                sameCount++;
            }
            else
            {
                m_slots[i].GetComponent<Image>().sprite = inactiveSprite;
            }
        }

        // 모든 요소가 만족되었을 때 배달 가능 상태로 변경
        if (sameCount == elements.Count)
        {
            m_uItarget.Start_Arrow(m_orderSlot.OrderInfo.targetHouse);
            m_orderSlot.OrderInfo.targetHouse.Available_Target(true);
        }
        else
        {
            m_uItarget.Reset_Arrow();
            m_orderSlot.OrderInfo.targetHouse.Available_Target(false);
        }
    }
}
