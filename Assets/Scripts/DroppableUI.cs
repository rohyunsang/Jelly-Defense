
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler 
{
    private Image image;
    private RectTransform rect;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    
    /// <summary>
    /// 마우스 포인터가 현재 아이템 슬롯 영역 내부로 들어갈 때 1회 호출
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {/* 지저분해 보여서 사용하지않기로 함
        // 파스텔톤의 연한 하늘색 RGB 값
        Color pastelSkyBlue = new Color(0.69f, 0.88f, 0.9f); // 예시값, 실제 RGB 값을 사용해주세요

        // 이미지 컴포넌트의 색상 설정
        image.color = pastelSkyBlue;*/
    }
    /// <summary>
    /// 마우스 포인터가 현재 아이템 슬롯 영역을 빠져나갈 때 1회 호출
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        //아이템 슬롯 색상을 하얀색으로 변경
      //  image.color = Color.white;
    }


        /// <summary>
        /// 마우스 포인터가 현재 아이템 슬롯 영역 내부에서 드롭했을 때 1회 호출
        /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        // 예외 처리 
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject != null && draggedObject.tag != "Icon") return;
        
        //Scroll View일때 처리
        if (eventData.pointerDrag != null && this.name == "SlimeIconContent")
        {
            eventData.pointerDrag.transform.SetParent(transform);

            RectTransform droppedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();

            // Setting the RectTransform to full stretch within its parent
            droppedRectTransform.anchorMin = Vector2.zero; // Anchors to the bottom left
            droppedRectTransform.anchorMax = Vector2.one; // Anchors to the top right

            // Setting all the offsets to zero
            droppedRectTransform.offsetMin = Vector2.zero; // Left and Bottom
            droppedRectTransform.offsetMax = Vector2.zero; // Right and Top

            // Optionally, you can also reset the localScale if needed
            droppedRectTransform.localScale = Vector3.one;
            
        }

        // Check if this object already has any children
        if (transform.childCount == 0 && eventData.pointerDrag != null)
        {
        eventData.pointerDrag.transform.SetParent(transform);

        RectTransform droppedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();

        // Setting the RectTransform to full stretch within its parent
        droppedRectTransform.anchorMin = Vector2.zero; // Anchors to the bottom left
        droppedRectTransform.anchorMax = Vector2.one; // Anchors to the top right

        // Setting all the offsets to zero
        droppedRectTransform.offsetMin = Vector2.zero; // Left and Bottom
        droppedRectTransform.offsetMax = Vector2.zero; // Right and Top

        // Optionally, you can also reset the localScale if needed
        droppedRectTransform.localScale = Vector3.one;  // Resetting scale to default (1,1,1)
        }
    }
}

