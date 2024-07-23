using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private House m_target;
    private Vector2 m_positionOffset = new Vector2(0f, -47f);
    private float   m_distanceOffset = 8f;
    private bool    m_IsTarget       = false;

    public bool IsTarget
    {
        get => m_IsTarget;
        set => m_IsTarget = value;
    }

    public void Start_Arrow(House targetHouse)
    {
        if (targetHouse == null)
            return;
        
        m_IsTarget = true;
        m_target   = targetHouse;

        gameObject.SetActive(true);
    }

    public void Reset_Arrow()
    {
        gameObject.SetActive(false);

        m_IsTarget = false;
        m_target   = null;
    }

    private void LateUpdate()
    {
        if (m_target == null)
            return;

        // Ÿ���� ���� ��ǥ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 screenPos = Camera.main.WorldToScreenPoint(m_target.gameObject.transform.position);

        // Ÿ���� ī�޶� �ڿ� �ִ� ���, ȭ�� ��ǥ ����
        if (screenPos.z < 0)
            screenPos = -screenPos;

        // Ÿ�� ���� ���
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2) + m_positionOffset;
        Vector2 direction = ((Vector2)screenPos - screenCenter).normalized;

        // UI ȭ��ǥ ȸ��
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // ȭ��ǥ�� ȭ�� �߾ӿ��� ���� �Ÿ���ŭ ������ ��ġ�� ����
        float radius = Mathf.Min(Screen.width, Screen.height) / m_distanceOffset;
        Vector2 arrowPosition = screenCenter + direction * radius;

        // ȭ��ǥ ��ġ ����
        transform.position = arrowPosition;
    }
}
