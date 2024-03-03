using UnityEngine;

public class MoveImage : MonoBehaviour
{
    public float moveDistance = 10f; // ������ �Ÿ�
    private float direction = 1f; // �̵� ����
    private Vector3 startPosition; // ���� ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ
    public float moveTime = 0.5f; // �̵��� �ɸ��� �ð�
    private float timer = 0; // Ÿ�̸�

    void Start()
    {
        startPosition = transform.position; // ���� ��ġ �ʱ�ȭ
        targetPosition = startPosition + new Vector3(moveDistance * direction, 0, 0); // ���� ��ǥ ��ġ ����
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime; // Ÿ�̸� ������Ʈ

        if (timer >= moveTime)
        {
            // ���� ��ȯ
            direction *= -1;
            startPosition = transform.position; // ���� ��ġ�� ���ο� ���� ��ġ�� ����
            targetPosition = startPosition + new Vector3(moveDistance * direction, 0, 0); // ���ο� ��ǥ ��ġ ����
            timer = 0; // Ÿ�̸� ����
        }

        // ��ġ�� Lerp�� ����Ͽ� �ε巴�� �̵�
        transform.position = Vector3.Lerp(startPosition, targetPosition, timer / moveTime);
    }
}