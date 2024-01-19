
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
    /// ���콺 �����Ͱ� ���� ������ ���� ���� ���η� �� �� 1ȸ ȣ��
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {/* �������� ������ ��������ʱ�� ��
        // �Ľ������� ���� �ϴû� RGB ��
        Color pastelSkyBlue = new Color(0.69f, 0.88f, 0.9f); // ���ð�, ���� RGB ���� ������ּ���

        // �̹��� ������Ʈ�� ���� ����
        image.color = pastelSkyBlue;*/
    }
    /// <summary>
    /// ���콺 �����Ͱ� ���� ������ ���� ������ �������� �� 1ȸ ȣ��
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        //������ ���� ������ �Ͼ������ ����
      //  image.color = Color.white;
    }


        /// <summary>
        /// ���콺 �����Ͱ� ���� ������ ���� ���� ���ο��� ������� �� 1ȸ ȣ��
        /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        // ���� ó�� 
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject != null && draggedObject.tag != "Icon") return;
        
        //Scroll View�϶� ó��
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

