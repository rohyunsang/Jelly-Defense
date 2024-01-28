using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 카메라 제어 스크립트 Made By SSABI
/// 사용법
/// 스크립트를 카메라에 부착
/// </summary>
public class CameraMovement : MonoBehaviour
{
    public float leanerSpeed = 10f;                     // 현재 이동 속도
    public float rollSpeed = 50f;                       // 현재 회전 속도
    public float WheelSpeed = 150f;                     // 현재 줌 속도
    public float FastRatio = 10f;                       // 증가 비율
    public float RotateDistance = 50f;                  // 기본 회전 거리
    public float TargetDistance = 10f;                  // 타겟 기본 거리
    public KeyCode RotateAroundKey = KeyCode.LeftAlt;   // Rotate Around Key
    public KeyCode FastKey = KeyCode.LeftShift;         // Fast Key (키를 누를 경우 기존보다 더 빨리 움직임)

    private float x;
    private float y;
    private float z;
    private Vector3 target;
    private Vector3 halfExtents;
    private RaycastHit hit;
    private Coroutine coroutine;

    void Update()
    {
        if (coroutine != null)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // Move
        if (Input.GetMouseButton(0))
        {
            Move();
        }
        // Set Target
        else if (Input.GetMouseButtonDown(1))
        {
            SetTarget();
        }
        // Rotate
        else if (Input.GetMouseButton(1))
        {
            if (Input.GetKey(RotateAroundKey))
            {
                RotateAround();
            }
            else
            {
                Rotate();
            }
        }
        // Zoom
        else if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Zoom();
        }
    }

    private void SetTarget()
    {
        Ray v1 = Camera.main.ScreenPointToRay(new Vector3(0f, 0f, 0f));
        Ray v2 = Camera.main.ScreenPointToRay(new Vector3(0f, Screen.height, 0f));
        Ray v3 = Camera.main.ScreenPointToRay(new Vector3(Screen.width, 0f, 0f));
        float width = Mathf.Abs(v1.origin.x - v3.origin.x);
        float height = Mathf.Abs(v1.origin.y - v2.origin.y);

        halfExtents = new Vector3(width, height, 0f) / 2f;

        if (Physics.BoxCast(transform.position, halfExtents, transform.forward, out hit))
        {
            target = hit.point;
        }
        else
        {
            target = transform.position + transform.forward * RotateDistance;
        }
    }

    private void Move()
    {
        x = -Input.GetAxis("Mouse X") * leanerSpeed * Time.deltaTime;
        y = -Input.GetAxis("Mouse Y") * leanerSpeed * Time.deltaTime;

        if (Input.GetKey(FastKey))
        {
            x *= FastRatio;
            y *= FastRatio;
        }

        transform.Translate(new Vector3(x, y, 0f));
    }

    private void Rotate()
    {
        x = Input.GetAxis("Mouse X") * rollSpeed * Time.deltaTime;
        y = -Input.GetAxis("Mouse Y") * rollSpeed * Time.deltaTime;

        if (Input.GetKey(FastKey))
        {
            x *= FastRatio;
            y *= FastRatio;
        }

        transform.RotateAround(transform.position, transform.right, y);
        transform.RotateAround(transform.position, Vector3.up, x);
    }

    private void RotateAround()
    {
        x = Input.GetAxis("Mouse X") * rollSpeed * Time.deltaTime;
        y = -Input.GetAxis("Mouse Y") * rollSpeed * Time.deltaTime;

        if (Input.GetKey(FastKey))
        {
            x *= FastRatio;
            y *= FastRatio;
        }

        transform.RotateAround(target, transform.right, y);
        transform.RotateAround(target, Vector3.up, x);
    }

    private void Zoom()
    {
        z = Input.GetAxis("Mouse ScrollWheel") * WheelSpeed * Time.deltaTime;

        if (Input.GetKey(FastKey))
        {
            z *= FastRatio;
        }

        transform.Translate(0f, 0f, z);
    }

    /// <summary>
    /// 타겟으로 이동 시 카메라 거리를 계산하는 메소드
    /// </summary>
    /// <param name="transform">타겟 트랜스폼</param>
    /// <returns>거리</returns>
    private float GetDistance(Transform transform)
    {
        float distance = 0f;
        Transform[] transforms = transform.GetComponentsInChildren<Transform>();

        foreach (Transform tr in transforms)
        {
            MeshFilter meshFilter = tr.GetComponent<MeshFilter>();

            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.mesh;
                float size = Mathf.Max(
                    mesh.bounds.size.x * tr.lossyScale.x,
                    mesh.bounds.size.y * tr.lossyScale.y,
                    mesh.bounds.size.z * tr.lossyScale.z);

                if (distance < size)
                {
                    distance = size + TargetDistance;
                }
            }
        }

        // 거리가 0일 경우 기본 거리 대입
        if (distance == 0f)
        {
            distance = TargetDistance;
        }

        return distance;
    }

    /// <summary>
    /// 카메라를 타겟으로 이동하는 메소드
    /// </summary>
    /// <param name="target">타겟</param>
    public void MoveTarget(Transform target)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        float distance = GetDistance(target);
        Vector3 direction = (transform.position - (transform.position + transform.forward)).normalized;
        Vector3 position = target.position + direction * distance;

        transform.position = position;
    }

    /// <summary>
    /// 카메라를 타겟으로 부드럽게 이동하는 메소드
    /// </summary>
    /// <param name="target">타겟</param>
    public void MoveTargetLerp(Transform target)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine = StartCoroutine(MoveTargetCoroutine(target));
    }

    /// <summary>
    /// 카메라를 타겟으로 부드럽게 이동하는 코루틴
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator MoveTargetCoroutine(Transform target)
    {
        float time = 0f;
        float distance = GetDistance(target);
        Vector3 direction = (transform.position - (transform.position + transform.forward)).normalized;
        Vector3 position = target.position + direction * distance;

        while (time <= 1f)
        {
            transform.position = Vector3.Lerp(transform.position, position, time);

            time += Time.deltaTime;
            yield return null;
        }

        coroutine = null;
    }
}