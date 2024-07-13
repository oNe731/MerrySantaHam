using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Transform m_target;

    private Vector3 m_startOffset = new Vector3(0f, -41.5f, 0f);
    private float m_distanceOffset = 3f;

    public Transform TargetObject
    {
        get => m_target;
        set => m_target = value;
    }

    private void LateUpdate()
    {
        if (m_target == null)
            return;

        // 타겟의 월드 좌표를 화면 좌표로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(m_target.position);

        // UI 화살표 회전
        Vector2 direction = ((Vector2)screenPos - new Vector2(Screen.width / 2, Screen.height / 2)).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 시작 오브젝트에서 타겟 방향으로 이동
        Vector3 startPosition = GameManager.Ins.Player.transform.position;
        Vector3 targetPosition = m_target.position;
        Vector3 moveDirection = (targetPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, targetPosition);

        // 최종 위치 계산
        Vector3 newPosition = startPosition + moveDirection * m_distanceOffset;

        // 화살표 위치 설정 (화면 좌표로 변환하여 설정)
        transform.position = Camera.main.WorldToScreenPoint(newPosition);
        transform.position += m_startOffset;
    }
}
