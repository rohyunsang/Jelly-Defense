using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler
{

    private Transform canvas; //UI가 소속되어있는 최상단의 캔버스 위치
    private Transform previousParent; //해당 오브젝트가 직전에 소속되어있던 부모 위치
    private RectTransform rect; //UI위치 제어를 위한 렉트트랜스폼
    private CanvasGroup canvasGroup;//UI알파값, 상호작용 제어를 위한 캔버스 그룹


    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();  
        canvasGroup = GetComponent<CanvasGroup>();
    }
    /// <summary>
    /// 현재 오브젝트를 드래그하기 시작할 때 1회 호출
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그 직전 소속되어있던 부모 트랜스폼 정보 저장
        previousParent = transform.parent;

        //현재 드래그중인 UI가 화면의 최상단에 출력되도록 하기 위해
        transform.SetParent(canvas); //부모 오브젝트를 캔버스로 설정
        transform.SetAsLastSibling(); //가장 앞에 보이도록 마지막 자식으로 설정

        //드래그 가능한 오브젝트가 하나가 아닌 자식들을 가지고 있을수도 때문에 캔버스그룹으로 통제
        //알파값을 0.6으로 설정하고, 광선 충돌 처리가 되지 않도록 한다.
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

       // throw new System.NotImplementedException();
    }

    /// <summary>
    /// 현재 오브젝트를 드래그중일 때 매 프레임 호출
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        //현재 스크린상의 마우스 위치를 UI위치로 설정(UI가 마우스를 쫓아다니는 상태)
        rect.position = eventData.position;
    }

    /// <summary>
    /// 현재 오브젝트를 드래그를 종료할 때 1회 호출
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        //드래그를 시작하면 부모가 캔버스로 설정되기에
        //드래그를 종료할 때 부모가 캔버스이면 아이템 슬롯이 아닌 엉뚱한곳에 드롭했음으로
        //드래그 직전에 소속돼있던 아이템 슬롯으로 아이템 이동
        if (transform.parent == canvas)
        {

            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponentInParent<RectTransform>().position;
        }
            //알파값을 1로 설정하고 광선 충돌처리가 되도록 함
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
        
    }
}
