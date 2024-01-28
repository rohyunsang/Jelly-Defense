using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required for detecting clicks on GameObjects

public class PickUpSlime : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // Assuming you have a way to reference or identify the specific slime prefab to instantiate.
    public GameObject slimePrefab;
    public GameObject checkImage;

    private float clickStartTime; // 클릭 시작 시간
    private bool longClickDetected = false; // 긴 클릭 감지 여부

    public void OnPointerClick(PointerEventData eventData)
    {
        // 긴 클릭이 감지되면, 클릭 처리를 하지 않음
        if (longClickDetected)
        {
            longClickDetected = false; // 긴 클릭 상태 초기화
            return;
        }

        TryPickUpSlime();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickStartTime = Time.time; // 클릭 시작 시간 기록
        longClickDetected = false; // 긴 클릭 상태 초기화
        StartCoroutine(CheckLongClick()); // 긴 클릭 체크 시작
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 클릭 종료 시, 긴 클릭 체크 중단
        StopAllCoroutines();
    }

    private IEnumerator CheckLongClick()
    {
        // 2초 대기
        yield return new WaitForSeconds(1.5f);

        // 2초 후에 여전히 클릭 중이면 정보창 표시
        if (Time.time - clickStartTime >= 1.5f)
        {
            longClickDetected = true; // 긴 클릭 감지 상태 설정
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