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

        // 타겟의 월드 좌표를 화면 좌표로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(m_target.gameObject.transform.position);

        // 타겟이 카메라 뒤에 있는 경우, 화면 좌표 반전
        if (screenPos.z < 0)
            screenPos = -screenPos;

        // 타겟 방향 계산
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2) + m_positionOffset;
        Vector2 direction = ((Vector2)screenPos - screenCenter).normalized;

        // UI 화살표 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 화살표를 화면 중앙에서 일정 거리만큼 떨어진 위치로 설정
        float radius = Mathf.Min(Screen.width, Screen.height) / m_distanceOffset;
        Vector2 arrowPosition = screenCenter + direction * radius;

        // 화살표 위치 설정
        transform.position = arrowPosition;
    }
}
