using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDemonSkill : MonoBehaviour
{
    public GameObject flyingDemonSkillEffect;
    private Rigidbody rb; // Rigidbody 컴포넌트 참조
    public GameObject explosionArea;
    public float skillWeaponDemage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Slime") || other.gameObject.CompareTag("SlimeCastle"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            flyingDemonSkillEffect.SetActive(true);
            StopMovement(); // 마법의 이동을 멈춤
            explosionArea.SetActive(true); // 폭발 영역 활성화
            Destroy(gameObject, 2f);
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
