using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager instance;

    public int skillCost = 0; //�Ʊ� ���� ���� �ڽ�Ʈ
    public int maxPlayerSkillPower = 200; 

    public TMP_Text playerSkillPowerText; // TextMeshPro Text ������Ʈ�� �Ҵ��� ����
    public float currentPlayerSkillPower = 0; // �Ʊ� ���� �ڽ�Ʈ �ѷ�
    public Slider playerSkillPowerSlider; // Add this line to reference the Slider

    public bool isStart = false;

    public GameObject cannonPrefab; // ����
    public GameObject meteorPrefab; // ���׿�

    public GameObject buffEffect;
    public GameObject stunStarEffect;

    void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϸ鼭 �̰� �ƴϸ� �ı� ��ȯ
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
        currentPlayerSkillPower = 0; // �÷��̾� ��ų �ʱ�ȭ
        isStart = true;
    }

    void Update()
    {
        if (!isStart) return;

        currentPlayerSkillPower += Time.deltaTime * 5f; //�ð� ��� ���� 
        currentPlayerSkillPower = Mathf.Clamp(currentPlayerSkillPower, 0, maxPlayerSkillPower); // Ensure skill power does not exceed max

        // Update the Slider's value
        if (playerSkillPowerSlider != null)
        {
            playerSkillPowerSlider.value = currentPlayerSkillPower / maxPlayerSkillPower; // Update the Slider to represent the skill power ratio
        }

        // �Ҽ����� ������ ������ ��ȯ�Ͽ� �ؽ�Ʈ�� ǥ��
        int playerSkillPowerInt = Mathf.FloorToInt(currentPlayerSkillPower);
        // UI Text ������Ʈ�� jellyPowerInt ���� ǥ��
        playerSkillPowerText.text = playerSkillPowerInt + " / " + maxPlayerSkillPower;
    }

    public void OnClickSkill_1() // Depo need cost 100
    {
        if (currentPlayerSkillPower < 199f) return;

        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_SkillSoundCannon);

        currentPlayerSkillPower -= 199f;
        Transform slimeCastleTransform = GameObject.FindWithTag("SlimeCastle").transform;
        Transform enemyCastleTransform = GameObject.FindWithTag("EnemyCastle").transform;

        // Calculate the direction vector from the slime castle to the enemy castle
        Vector3 direction = (enemyCastleTransform.position - slimeCastleTransform.position).normalized;

        // Calculate the position 20 units away towards the enemy castle
        Vector3 skillPosition = slimeCastleTransform.position + direction * 5f;

        // Assuming you have a skill prefab or effect to instantiate
        GameObject skillEffect0 = Instantiate(cannonPrefab, skillPosition + new Vector3(3f,0f,0f), Quaternion.identity);
        skillPosition = slimeCastleTransform.position + direction * 10f;

        GameObject skillEffect1 = Instantiate(cannonPrefab, skillPosition + new Vector3(-1f, 0f, 0f), Quaternion.identity);
        skillPosition = slimeCastleTransform.position + direction * 15f;

        GameObject skillEffect2 = Instantiate(cannonPrefab, skillPosition + new Vector3(-3f, 0f, 0f), Quaternion.identity);

        skillPosition = slimeCastleTransform.position + direction * 5f;

        GameObject skillEffect3 = Instantiate(cannonPrefab, skillPosition + new Vector3(-3f, 0f, 0f), Quaternion.identity);

        skillPosition = slimeCastleTransform.position + direction * 15f;

        GameObject skillEffect4 = Instantiate(cannonPrefab, skillPosition + new Vector3(2f, 0f, 0f), Quaternion.identity);

        // Rotate the skill effect to face the enemy castle
        skillEffect0.transform.LookAt(enemyCastleTransform);
        skillEffect1.transform.LookAt(enemyCastleTransform);
        skillEffect2.transform.LookAt(enemyCastleTransform);
    }
    public void OnClickSkill_2() // Move Speed need cost 150
    {
        if (currentPlayerSkillPower < 150f) return;

        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_SkillSoundIncreaseAttack);

        currentPlayerSkillPower -= 150f;
        foreach (Transform child in SlimeSpawnManager.instance.slimeParent.transform)
        {
            // �� �ڽ� ��ġ�� ����Ʈ ����
            var effectInstance = Instantiate(buffEffect, child.position, Quaternion.identity, child.transform);
            ISlime slime = child.gameObject.GetComponent<ISlime>();
            slime.AttackDamage *= 1.1f;
            slime.SlimeWeaponDamageUpdate();

            StartCoroutine(RemoveBuffAfterTime(effectInstance, slime, 10f));
        }
    }

    IEnumerator RemoveBuffAfterTime(GameObject effectInstance, ISlime slime, float delay)
    {
        yield return new WaitForSeconds(delay); // 10�� ���

        // ��ȭ ȿ�� ����
        if (slime != null)
        {
            slime.AttackDamage /= 1.1f; // ��ȭ �� ���·� ����
            slime.SlimeWeaponDamageUpdate();
        }

        // ����Ʈ ����
        if (effectInstance != null)
        {
            Destroy(effectInstance);
        }
    }

    public void OnClickSkill_3() 
    {
        if (currentPlayerSkillPower < 150f) return;

        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_SkillSoundMeteor);


        currentPlayerSkillPower -= 150f;
        Transform slimeCastleTransform = GameObject.FindWithTag("SlimeCastle").transform;
        Transform enemyCastleTransform = GameObject.FindWithTag("EnemyCastle").transform;

        // Calculate the direction vector from the slime castle to the enemy castle

        Vector3 midPos = (enemyCastleTransform.position + slimeCastleTransform.position) / 2;

        Vector3 direction = (enemyCastleTransform.position - slimeCastleTransform.position).normalized;

        // Calculate the position 20 units away towards the enemy castle
        Vector3 skillPosition = slimeCastleTransform.position + direction * 40f;

        // Assuming you have a skill prefab or effect to instantiate
        GameObject skillEffect = Instantiate(meteorPrefab, midPos, Quaternion.identity);

        // Rotate the skill effect to face the enemy castle
        skillEffect.transform.LookAt(enemyCastleTransform);
    }

    public void StageEndSettingInit()
    {
        isStart = false;
    }
}
