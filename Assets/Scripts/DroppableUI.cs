
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
    {
        //아이템 슬롯 색상을 노란색으로 변경
        image.color = Color.yellow;
    }
    /// <summary>
    /// 마우스 포인터가 현재 아이템 슬롯 영역을 빠져나갈 때 1회 호출
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        //아이템 슬롯 색상을 하얀색으로 변경
        image.color = Color.white;
    }


        /// <summary>
        /// 마우스 포인터가 현재 아이템 슬롯 영역 내부에서 드롭했을 때 1회 호출
        /// </summary>
    public void OnDrop(PointerEventData eventData)
{
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

