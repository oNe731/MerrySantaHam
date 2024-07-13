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

        // Ÿ���� ���� ��ǥ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 screenPos = Camera.main.WorldToScreenPoint(m_target.position);

        // UI ȭ��ǥ ȸ��
        Vector2 direction = ((Vector2)screenPos - new Vector2(Screen.width / 2, Screen.height / 2)).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // ���� ������Ʈ���� Ÿ�� �������� �̵�
        Vector3 startPosition = GameManager.Ins.Player.transform.position;
        Vector3 targetPosition = m_target.position;
        Vector3 moveDirection = (targetPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, targetPosition);

        // ���� ��ġ ���
        Vector3 newPosition = startPosition + moveDirection * m_distanceOffset;

        // ȭ��ǥ ��ġ ���� (ȭ�� ��ǥ�� ��ȯ�Ͽ� ����)
        transform.position = Camera.main.WorldToScreenPoint(newPosition);
        transform.position += m_startOffset;
    }
}
