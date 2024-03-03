using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems; // Required for detecting clicks on GameObjects

public class PickUpSlime : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // Assuming you have a way to reference or identify the specific slime prefab to instantiate.
    public GameObject checkImage;

    private float clickStartTime; // Ŭ�� ���� �ð�
    private bool longClickDetected = false; // �� Ŭ�� ���� ����

    public void OnPointerClick(PointerEventData eventData)
    {
        // �� Ŭ���� �����Ǹ�, Ŭ�� ó���� ���� ����
        if (longClickDetected)
        {
            longClickDetected = false; // �� Ŭ�� ���� �ʱ�ȭ
            return;
        }

        TryPickUpSlime();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickStartTime = Time.time; // Ŭ�� ���� �ð� ���
        longClickDetected = false; // �� Ŭ�� ���� �ʱ�ȭ
        StartCoroutine(CheckLongClick()); // �� Ŭ�� üũ ����
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Ŭ�� ���� ��, �� Ŭ�� üũ �ߴ�
        StopAllCoroutines();
    }

    private IEnumerator CheckLongClick()
    {
        // 2�� ���
        yield return new WaitForSeconds(1.0f);

        // 2�� �Ŀ� ������ Ŭ�� ���̸� ����â ǥ��
        if (Time.time - clickStartTime >= 1.0f)
        {
            longClickDetected = true; // �� Ŭ�� ���� ���� ����
            ShowInfoPanel();
        }
    }

    private void ShowInfoPanel()
    {
        //���� �κ�. 
        Debug.Log(gameObject.name);
        Slime slimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == gameObject.name.Replace("Icon",""));
        SlimeManager.instance.slimeName.text = slimeData.KRName;
        SlimeManager.instance.slimeDesText.text = slimeData.DesText;
        SlimeManager.instance.slimeHPText.text = slimeData.HP.ToString();
        SlimeManager.instance.slimeAttackText.text = slimeData.AttackDamage.ToString();
        SlimeManager.instance.slimeDefenseText.text = slimeData.Defense.ToString(); 
        SlimeManager.instance.slimeMoveSpeedText.text = slimeData.MoveSpeed.ToString();
        SlimeManager.instance.slimeRangeText.text = slimeData.AttackRange.ToString();
        SlimeManager.instance.slimeAttackSpeedText.text = slimeData.AttackSpeed.ToString();
        SlimeManager.instance.ActivateSlimeInfo();
    }

    private void TryPickUpSlime()
    {
        UIManager.instance.UIClickSound();
        // �̹� ���õ� ������ �������� �ִ��� Ȯ��
        if (!string.IsNullOrEmpty(SlimeManager.instance.selectedLegendSlime))
        {
            // ���õ� ������ �������� �ְ�, ���� �����Ϸ��� �������� ������ ������ �ִ��� Ȯ��
            int slimeIndex = Array.FindIndex(SlimeManager.instance.slimeIconPrefabs, item => item.name == gameObject.name);
            if (slimeIndex >= 20 && slimeIndex <= 24)
            {
                UIManager.instance.onlyOneLegendSlimeInfo.SetActive(true);
                return; // �߰� ���� ����
            }
        }

        if (checkImage.activeSelf) return;
        int emptySlotIndex = SlimeManager.instance.FindFirstEmptySlot();

        if (emptySlotIndex != -1) // �� ������ �ִ� ���
        {
            // ���� ����...
            PickUp(emptySlotIndex);

            // ���� ������ �������� ������ ������ �ִ� ���, ���õ� ������ ���������� ����
            int pickedSlimeIndex = Array.FindIndex(SlimeManager.instance.slimeIconPrefabs, item => item.name == gameObject.name);
            if (pickedSlimeIndex >= 20 && pickedSlimeIndex <= 24)
            {
                SlimeManager.instance.selectedLegendSlime = gameObject.name;
            }
        }
        else
        {
            Debug.Log("��� ������ ������ �� �ֽ��ϴ�.");
        }

        if(emptySlotIndex == 4)
        {
            TutorialManager.Instance.EndTutorial(6);
            TutorialManager.Instance.StartTutorial(7);
        }
    }

    private void PickUp(int slotIndex)
    {
        // Assuming SlimeManager.instance.slimeIconPrefabs is an array of the prefabs that correspond to the slimes
        // and that slimePrefab is one of those prefabs, find its index or directly use the provided prefab.
        GameObject slimeIcon = Instantiate(gameObject, SlimeManager.instance.SlimeSlots[slotIndex].transform);
        slimeIcon.name = gameObject.name;
        // Set the local position of the slimeIcon to zero to ensure it's correctly positioned within the slot.
        slimeIcon.transform.localPosition = Vector3.zero;
        RectTransform rectTransform = slimeIcon.GetComponent<RectTransform>();
        // Set the width and height of the slimeIcon
        rectTransform.sizeDelta = new Vector2(250, 250);

        slimeIcon.GetComponent<PickUpSlime>().enabled = false;

        checkImage.SetActive(true);

        // Perform any additional setup or assignments here
    }

}