using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler
{

    private Transform canvas; //UI�� �ҼӵǾ��ִ� �ֻ���� ĵ���� ��ġ
    private Transform previousParent; //�ش� ������Ʈ�� ������ �ҼӵǾ��ִ� �θ� ��ġ
    private RectTransform rect; //UI��ġ ��� ���� ��ƮƮ������
    private CanvasGroup canvasGroup;//UI���İ�, ��ȣ�ۿ� ��� ���� ĵ���� �׷�


    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();  
        canvasGroup = GetComponent<CanvasGroup>();
    }
    /// <summary>
    /// ���� ������Ʈ�� �巡���ϱ� ������ �� 1ȸ ȣ��
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //�巡�� ���� �ҼӵǾ��ִ� �θ� Ʈ������ ���� ����
        previousParent = transform.parent;

        //���� �巡������ UI�� ȭ���� �ֻ�ܿ� ��µǵ��� �ϱ� ����
        transform.SetParent(canvas); //�θ� ������Ʈ�� ĵ������ ����
        transform.SetAsLastSibling(); //���� �տ� ���̵��� ������ �ڽ����� ����

        //�巡�� ������ ������Ʈ�� �ϳ��� �ƴ� �ڽĵ��� ������ �������� ������ ĵ�����׷����� ����
        //���İ��� 0.6���� �����ϰ�, ���� �浹 ó���� ���� �ʵ��� �Ѵ�.
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

       // throw new System.NotImplementedException();
    }

    /// <summary>
    /// ���� ������Ʈ�� �巡������ �� �� ������ ȣ��
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        //���� ��ũ������ ���콺 ��ġ�� UI��ġ�� ����(UI�� ���콺�� �Ѿƴٴϴ� ����)
        rect.position = eventData.position;
    }

    /// <summary>
    /// ���� ������Ʈ�� �巡�׸� ������ �� 1ȸ ȣ��
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        //�巡�׸� �����ϸ� �θ� ĵ������ �����Ǳ⿡
        //�巡�׸� ������ �� �θ� ĵ�����̸� ������ ������ �ƴ� �����Ѱ��� �����������
        //�巡�� ������ �Ҽӵ��ִ� ������ �������� ������ �̵�
        if (transform.parent == canvas)
        {

            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponentInParent<RectTransform>().position;
        }
            //���İ��� 1�� �����ϰ� ���� �浹ó���� �ǵ��� ��
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
        
    }
}
