using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ī�޶� ���� ��ũ��Ʈ Made By SSABI
/// ����
/// ��ũ��Ʈ�� ī�޶� ����
/// </summary>
public class CameraMovement : MonoBehaviour
{
    public float leanerSpeed = 10f;                     // ���� �̵� �ӵ�
    public float rollSpeed = 50f;                       // ���� ȸ�� �ӵ�
    public float WheelSpeed = 150f;                     // ���� �� �ӵ�
    public float FastRatio = 10f;                       // ���� ����
    public float RotateDistance = 50f;                  // �⺻ ȸ�� �Ÿ�
    public float TargetDistance = 10f;                  // Ÿ�� �⺻ �Ÿ�
    public KeyCode RotateAroundKey = KeyCode.LeftAlt;   // Rotate Around Key
    public KeyCode FastKey = KeyCode.LeftShift;         // Fast Key (Ű�� ���� ��� �������� �� ���� ������)

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
    /// Ÿ������ �̵� �� ī�޶� �Ÿ��� ����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="transform">Ÿ�� Ʈ������</param>
    /// <returns>�Ÿ�</returns>
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

        // �Ÿ��� 0�� ��� �⺻ �Ÿ� ����
        if (distance == 0f)
        {
            distance = TargetDistance;
        }

        return distance;
    }

    /// <summary>
    /// ī�޶� Ÿ������ �̵��ϴ� �޼ҵ�
    /// </summary>
    /// <param name="target">Ÿ��</param>
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
    /// ī�޶� Ÿ������ �ε巴�� �̵��ϴ� �޼ҵ�
    /// </summary>
    /// <param name="target">Ÿ��</param>
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
    /// ī�޶� Ÿ������ �ε巴�� �̵��ϴ� �ڷ�ƾ
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