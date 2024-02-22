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
        yield return new WaitForSeconds(1.0f);

        // 2초 후에 여전히 클릭 중이면 정보창 표시
        if (Time.time - clickStartTime >= 1.0f)
        {
            longClickDetected = true; // 긴 클릭 감지 상태 설정
            ShowInfoPanel();
        }
    }

    private void ShowInfoPanel()
    {
        //여기 부분. 
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
        // 이미 선택된 레전드 슬라임이 있는지 확인
        if (!string.IsNullOrEmpty(SlimeManager.instance.selectedLegendSlime))
        {
            // 선택된 레전드 슬라임이 있고, 현재 선택하려는 슬라임이 레전드 범위에 있는지 확인
            int slimeIndex = Array.FindIndex(SlimeManager.instance.slimeIconPrefabs, item => item.name == gameObject.name);
            if (slimeIndex >= 20 && slimeIndex <= 24)
            {
                UIManager.instance.onlyOneLegendSlimeInfo.SetActive(true);
                return; // 추가 선택 방지
            }
        }

        if (checkImage.activeSelf) return;
        int emptySlotIndex = SlimeManager.instance.FindFirstEmptySlot();

        if (emptySlotIndex != -1) // 빈 슬롯이 있는 경우
        {
            // 선택 로직...
            PickUp(emptySlotIndex);

            // 현재 선택한 슬라임이 레전드 범위에 있는 경우, 선택된 레전드 슬라임으로 설정
            int pickedSlimeIndex = Array.FindIndex(SlimeManager.instance.slimeIconPrefabs, item => item.name == gameObject.name);
            if (pickedSlimeIndex >= 20 && pickedSlimeIndex <= 24)
            {
                SlimeManager.instance.selectedLegendSlime = gameObject.name;
            }
        }
        else
        {
            Debug.Log("모든 슬라임 슬롯이 차 있습니다.");
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