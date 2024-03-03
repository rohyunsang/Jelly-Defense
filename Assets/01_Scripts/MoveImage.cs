using UnityEngine;

public class MoveImage : MonoBehaviour
{
    public float moveDistance = 10f; // 움직일 거리
    private float direction = 1f; // 이동 방향
    private Vector3 startPosition; // 시작 위치
    private Vector3 targetPosition; // 목표 위치
    public float moveTime = 0.5f; // 이동에 걸리는 시간
    private float timer = 0; // 타이머

    void Start()
    {
        startPosition = transform.position; // 시작 위치 초기화
        targetPosition = startPosition + new Vector3(moveDistance * direction, 0, 0); // 최초 목표 위치 설정
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime; // 타이머 업데이트

        if (timer >= moveTime)
        {
            // 방향 전환
            direction *= -1;
            startPosition = transform.position; // 현재 위치를 새로운 시작 위치로 설정
            targetPosition = startPosition + new Vector3(moveDistance * direction, 0, 0); // 새로운 목표 위치 설정
            timer = 0; // 타이머 리셋
        }

        // 위치를 Lerp를 사용하여 부드럽게 이동
        transform.position = Vector3.Lerp(startPosition, targetPosition, timer / moveTime);
    }
}