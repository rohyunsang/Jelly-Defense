using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager instance;

    public int skillCost = 0; //아군 유닛 스폰 코스트
    public int maxPlayerSkillPower = 200; 

    public TMP_Text playerSkillPowerText; // TextMeshPro Text 오브젝트를 할당할 변수
    public float currentPlayerSkillPower = 0; // 아군 스폰 코스트 총량
    public Slider playerSkillPowerSlider; // Add this line to reference the Slider

    public bool isStart = false;

    public GameObject cannonPrefab; // 대포
    public GameObject meteorPrefab; // 메테오

    public GameObject buffEffect;

    void Awake()
    {
        // 이미 인스턴스가 존재하면서 이게 아니면 파괴 반환
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitPlayerSkill()
    {
        currentPlayerSkillPower = 0; // 플레이어 스킬 초기화
        isStart = true;
    }

    void Update()
    {
        if (!isStart) return;

        currentPlayerSkillPower += Time.deltaTime * 5f; //시간 비례 증가 
        currentPlayerSkillPower = Mathf.Clamp(currentPlayerSkillPower, 0, maxPlayerSkillPower); // Ensure skill power does not exceed max

        // Update the Slider's value
        if (playerSkillPowerSlider != null)
        {
            playerSkillPowerSlider.value = currentPlayerSkillPower / maxPlayerSkillPower; // Update the Slider to represent the skill power ratio
        }

        // 소수점을 버리고 정수로 변환하여 텍스트로 표시
        int playerSkillPowerInt = Mathf.FloorToInt(currentPlayerSkillPower);
        // UI Text 오브젝트에 jellyPowerInt 값을 표시
        playerSkillPowerText.text = playerSkillPowerInt + " / " + maxPlayerSkillPower;
    }

    public void OnClickSkill_1() // Depo need cost 100
    {
        currentPlayerSkillPower -= 100f;
        Transform slimeCastleTransform = GameObject.FindWithTag("SlimeCastle").transform;
        Transform enemyCastleTransform = GameObject.FindWithTag("EnemyCastle").transform;

        // Calculate the direction vector from the slime castle to the enemy castle
        Vector3 direction = (enemyCastleTransform.position - slimeCastleTransform.position).normalized;

        // Calculate the position 20 units away towards the enemy castle
        Vector3 skillPosition = slimeCastleTransform.position + direction * 20f;

        // Assuming you have a skill prefab or effect to instantiate
        GameObject skillEffect = Instantiate(cannonPrefab, skillPosition, Quaternion.identity);

        // Rotate the skill effect to face the enemy castle
        skillEffect.transform.LookAt(enemyCastleTransform);
    }
    public void OnClickSkill_2() // Move Speed need cost 150
    {
        currentPlayerSkillPower -= 150f;
        foreach (Transform child in SlimeSpawnManager.instance.slimeParent.transform)
        {
            // 각 자식 위치에 이펙트 생성
            Instantiate(buffEffect, child.position, Quaternion.identity, child.transform);
            ISlime slime = child.gameObject.GetComponent<ISlime>();
            slime.AttackDamage *= 1.2f;
            slime.SlimeWeaponDamageUpdate();
        }
    }

    public void OnClickSkill_3() //Meteo need cost 200
    {
        currentPlayerSkillPower -= 199f;
        Transform slimeCastleTransform = GameObject.FindWithTag("SlimeCastle").transform;
        Transform enemyCastleTransform = GameObject.FindWithTag("EnemyCastle").transform;

        // Calculate the direction vector from the slime castle to the enemy castle
        Vector3 direction = (enemyCastleTransform.position - slimeCastleTransform.position).normalized;

        // Calculate the position 20 units away towards the enemy castle
        Vector3 skillPosition = slimeCastleTransform.position + direction * 20f;

        // Assuming you have a skill prefab or effect to instantiate
        GameObject skillEffect = Instantiate(meteorPrefab, skillPosition, Quaternion.identity);

        // Rotate the skill effect to face the enemy castle
        skillEffect.transform.LookAt(enemyCastleTransform);
    }
}
