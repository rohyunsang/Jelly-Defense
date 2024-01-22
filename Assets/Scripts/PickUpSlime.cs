using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required for detecting clicks on GameObjects

public class PickUpSlime : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // Assuming you have a way to reference or identify the specific slime prefab to instantiate.
    public GameObject slimePrefab;
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
        yield return new WaitForSeconds(1.5f);

        // 2�� �Ŀ� ������ Ŭ�� ���̸� ����â ǥ��
        if (Time.time - clickStartTime >= 1.5f)
        {
            longClickDetected = true; // �� Ŭ�� ���� ���� ����
            ShowInfoPanel();
        }
    }

    private void ShowInfoPanel()
    {
        SlimeManager.instance.ActivateSlimeInfo();
    }

    private void TryPickUpSlime()
    {
        if (checkImage.activeSelf) return;
        int emptySlotIndex = SlimeManager.instance.FindFirstEmptySlot();

        if (emptySlotIndex != -1) // Checks if there is an empty slot available
        {
            
            PickUp(emptySlotIndex);
        }
        else
        {
            // No empty slots available, handle accordingly (e.g., show a message)
            Debug.Log("All slime slots are occupied.");
        }
    }

    private void PickUp(int slotIndex)
    {
        // Assuming SlimeManager.instance.slimeIconPrefabs is an array of the prefabs that correspond to the slimes
        // and that slimePrefab is one of those prefabs, find its index or directly use the provided prefab.
        GameObject slimeIcon = Instantiate(slimePrefab, SlimeManager.instance.SlimeSlots[slotIndex].transform);
        slimeIcon.name = slimePrefab.name;
        // Set the local position of the slimeIcon to zero to ensure it's correctly positioned within the slot.
        slimeIcon.transform.localPosition = Vector3.zero;
        checkImage.SetActive(true);

        // Perform any additional setup or assignments here
    }

}