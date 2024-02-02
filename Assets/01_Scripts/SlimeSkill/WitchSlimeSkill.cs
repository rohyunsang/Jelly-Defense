using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchSlimeSkill : MonoBehaviour
{
    public GameObject witchSkillEffect;
    private Rigidbody rb; // Rigidbody 컴포넌트 참조
    public GameObject explosionArea;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyCastle"))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            GetComponent<MeshRenderer>().enabled = false;
            witchSkillEffect.SetActive(true);
            StopMovement(); // 마법의 이동을 멈춤
            explosionArea.SetActive(true); // 폭발 영역 활성화
            Destroy(gameObject, 1f);
        }
    }

    private void StopMovement()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // 마법의 속도를 0으로 설정
            rb.isKinematic = true; // 물리 연산을 비활성화하여 추가적인 이동 방지
        }
    }
}
