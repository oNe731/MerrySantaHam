using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UIOrder : MonoBehaviour
{
    private List<GameObject> m_slots = new List<GameObject>();
    public List<Item.ELEMENT> m_elements;

    private Dictionary<string, Sprite> m_orderSpr   = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_elementSpr = new Dictionary<string, Sprite>();

    private Image m_timerImg;

    private bool m_possible = false;
    private bool m_timeOver = false;


    private float m_minShakeTime = 0.5f;
    private float m_maxShakeTime = 0.8f;
    private float m_minShakeAmount = 500f;
    private float m_maxShakeAmount = 1000f;

    private float m_minTargetX = -100f;
    private float m_maxTargetX = 100f;
    private float m_setTargetY = -700f;
    private float m_minHeight = 300f;
    private float m_maxHeight = 400f;
    private float m_minMoveSpeed = 20f;
    private float m_maxMoveSpeed = 25f;

    private bool m_rotation = false;
    private float m_minAngle = -70f;
    private float m_maxAngle = 70f;
    private float m_minRotationSpeed = -50f;
    private float m_maxRotationSpeed = 50f;


    private float m_shakeTime = 0.5f;
    private float m_shakeAmount = 1000f;

    private bool m_down = false;
    private Vector3 m_targetPosition;
    private float m_heightArc; // 포물선의 높이
    private float m_moveSpeed; // 이동 속도

    private Quaternion m_startRotation;
    private Quaternion m_targetRotation;
    private float m_rotationSpeed = 500.0f; // 회전 속도

    private RectTransform m_rectTransform = null;
    private Vector3 m_startPosition;
    private Coroutine m_shakeCoroutine = null;

    private OrderSlot m_orderSlot = null;

    private bool m_clear = false;
    private bool m_over = false;

    public bool Down 
    {
        get => m_down;
        set => m_down = value; 
    }
    public bool Clear { get => m_clear; }

    public void Initialize_UIOrder(OrderSlot orderSlot)
    {
        m_orderSlot = orderSlot; 

        // 초기화
        m_orderSpr.Add("BadDoll",   Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/BadDoll"));
        m_orderSpr.Add("FishSugar", Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/FishSugar"));
        m_orderSpr.Add("FishTank",  Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/FishTank"));
        m_orderSpr.Add("SantaHat",  Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/SantaHat"));
        m_orderSpr.Add("StrawberryFish", Resources.Load<Sprite>("Textures/2D/UI/Order/OrderItem/StrawberryFish"));

        m_elementSpr.Add("CitizenHat", Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_SnowManHat"));
        m_elementSpr.Add("Cloud", Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Cloud"));
        m_elementSpr.Add("Fish",  Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Fish"));
        m_elementSpr.Add("Strawberry", Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Strawberry"));
        m_elementSpr.Add("Wood",  Resources.Load<Sprite>("Textures/2D/UI/Order/Element/UI_Element_Wood"));

        for (int i = 0; i < 3; ++i)
        {
            m_slots.Add(GameManager.Ins.Create_GameObject("Prefabs/UI/OrderSlot", transform.GetChild(3)));
        }


        m_timerImg = transform.GetChild(0).GetComponent<Image>();
        m_rectTransform = GetComponent<RectTransform>();
        m_startPosition = m_rectTransform.anchoredPosition;
        m_startRotation = m_rectTransform.rotation;
        Set_Value();

        // 아이템 이미지 할당
        Sprite sprite = null;
        switch (m_orderSlot.OrderInfo.objectType)
        {
            case Order.OBJECT.OJ_FishTanghulu:
                sprite = m_orderSpr["FishSugar"];
                break;
            case Order.OBJECT.OJ_SantaHat:
                sprite = m_orderSpr["SantaHat"];
                break;
            case Order.OBJECT.OJ_SmallFishTank:
                sprite = m_orderSpr["FishTank"];
                break;
            case Order.OBJECT.OJ_StrawberryFish:
                sprite = m_orderSpr["StrawberryFish"];
                break;
            case Order.OBJECT.OJ_VoodooDoll:
                sprite = m_orderSpr["BadDoll"];
                break;
        }
        transform.GetChild(1).GetComponent<Image>().sprite = sprite;

        // 요소 이미지 할당
        m_elements = m_orderSlot.OrderInfo.elements;
        for (int i = 0; i < m_elements.Count; ++i)
        {
            switch (m_elements[i])
            {
                case Item.ELEMENT.EM_Tree:
                    sprite = m_elementSpr["Wood"];
                    break;
                case Item.ELEMENT.EM_Cloud:
                    sprite = m_elementSpr["Cloud"];
                    break;
                case Item.ELEMENT.EM_Fish:
                    sprite = m_elementSpr["Fish"];
                    break;
                case Item.ELEMENT.EM_Person:
                    sprite = m_elementSpr["CitizenHat"];
                    break;
                case Item.ELEMENT.EM_Strawberry:
                    sprite = m_elementSpr["Strawberry"];
                    break;
            }

            if(i >= m_slots.Count)
                break;

            if (m_slots[i] == null)
                break;

            Transform transform = m_slots[i].transform.GetChild(0);
            if (transform == null)
                break;

            Image image = transform.GetComponent<Image>();
            if (image == null)
                break;
            image.sprite = sprite;

            transform.gameObject.SetActive(true);
        }

        Update_TimerColor();
        Check_Slots(GameManager.Ins.Player.Inventory.Slots);
    }

    private void Set_Value()
    {
        m_shakeTime = UnityEngine.Random.Range(m_minShakeTime, m_maxShakeTime);
        m_shakeAmount = UnityEngine.Random.Range(m_minShakeAmount, m_maxShakeAmount);

        m_targetPosition = new Vector3(UnityEngine.Random.Range(m_minTargetX, m_maxTargetX), m_setTargetY, 0);
        m_heightArc = UnityEngine.Random.Range(m_minHeight, m_maxHeight);
        m_moveSpeed = Mathf.Lerp(m_minMoveSpeed, m_maxMoveSpeed, (m_heightArc - m_minHeight) / (m_maxHeight - m_minHeight));
        float angle = Vector3.Angle((m_startPosition - m_targetPosition).normalized, Vector3.up); // 각도에 따른 속도 조절
        if (angle < 90)
            angle = 90 + (90 - angle);
        float result = (90 - Mathf.Abs(angle - 90));
        m_moveSpeed *= result;

        m_targetRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(m_minAngle, m_maxAngle));
        m_rotationSpeed = UnityEngine.Random.Range(m_minRotationSpeed, m_maxRotationSpeed);
    }

    private void Update()
    {
        // Test
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    m_rectTransform.anchoredPosition = m_startPosition;
        //    m_rectTransform.rotation = m_startRotation;
        //    Set_Value();

        //    Shake_Object();
        //}

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
        if(m_orderSlot.OrderInfo.currentTimer <= 0) // 실패
        {
            m_orderSlot.OrderInfo.currentTimer = 0;
            Shake_Object();

            m_timeOver = true;
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

            Destroy(gameObject);
        }
    }

    public void Shake_Object(Action isAction = null)
    {
        if (m_timeOver == true)
            return;

        if (m_shakeCoroutine != null)
            StopCoroutine(m_shakeCoroutine);
        m_shakeCoroutine = StartCoroutine(Shake(isAction));
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

    public void Check_Slots(List<InvenSlot> slots)
    {
        if (m_elements == null)
            return;

        int sameCount = 0;
        for (int i = 0; i < m_elements.Count; ++i) // 해당 주문서의 요소
        {
            for(int j = 0; j < slots.Count; ++j) // 인벤토리
            {
                if(slots[j].EMPTY == false) // 아이템이 존재할 시
                {
                    if (m_elements[i] == slots[j].Item.itemType) // 해당 요소와 인벤토리 요소와 같을 시
                    {
                        // 같은 그룹 내 같은 요소 타입이 있는지
                        int sameElementCounts = 0;
                        for (int r = 0; r < slots.Count; ++r)
                        {
                            if (i == r) // 같은 요소 번호는 건너뜀
                                break;

                            if (m_elements[i] == m_elements[r])
                                sameElementCounts++;
                        }

                        // 중복 요소보다 더 많을 시 불 활성화
                        if(slots[j].Item.count > sameElementCounts)
                        {
                            if(i >= m_slots.Count)
                            {
                                return;
                            }
                            m_slots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/2D/UI/Order/Slot/UI_order_Element_Uses");
                            sameCount++;
                        }
                    }
                    //else // 나머지 타입과도 중복이 아닌지 검사
                    //{
                    //    bool same = false;
                    //    for (int k = 0; k < slots.Count; ++k)
                    //    {
                    //        if (slots[k].EMPTY == false)
                    //        {
                    //            if (m_elements[i] == slots[k].Item.itemType)
                    //            {
                    //                // 개수
                    //                if (slots[k].Item.count > 0)
                    //                {
                    //                    same = true;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }

                    //    // 중복이 전혀 없을 시 초기화
                    //    if(same == false)
                    //        m_slots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/2D/UI/Order/Slot/UI_order_Element");
                    //}
                }
            }
        }

        if (sameCount == m_elements.Count)
        {
            if (m_possible == true)
                return;
            m_possible = true;

            if (m_orderSlot == null)
                return;
            if (m_orderSlot.TargetHouse == null)
                return;
            m_orderSlot.TargetHouse.Create_UI();
        }
    }


    public void Close_Move()
    {
        if (m_clear == true)
            return;

        m_clear = true;
        StartCoroutine(Coroutine_Move(m_startPosition, new Vector3(m_startPosition.x, m_startPosition.y + 250f, m_startPosition.z), 0.5f, false));
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

        Destroy(gameObject);
    }

}
