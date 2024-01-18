using System.Collections;
using UnityEngine;

public class SwipeController : MonoBehaviour
{   //작동이 되지 않을 때는 시간이 멈춰져있는지 확인할 것
    //슬라이더의 스와이프를 원하는 이동량만큼 애니메이션처럼 조정해주는 스크립트.
    //ImpleementMainScreen씬> Canvas_1> StageScreen> Scroll View,
    //ImpleementMainScreen씬> Canvas_1> StageScreen_Chaos> Scroll View 에 적용중

    [SerializeField] int maxPage; // 최대 페이지 양
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pagestep; // 페이지 단계
    [SerializeField] RectTransform levelPageRect; // 페이지 단계

    [SerializeField] float tweenTime; // 애니메이션 시간 (초)
    [SerializeField] AnimationCurve tweenCurve; // 애니메이션 곡선 (옵션)


    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPageRect.localPosition;
    }

    private void Start()
    {
    }

    void Pagestep()
    {
        // 자신의 RectTransform 컴포넌트를 가져옵니다.
        RectTransform rectTransform = GetComponent<RectTransform>();

        // 가로값을 가져옵니다.
        float width = rectTransform.rect.width;
        // Debug.Log("Width: " + width); // 가로값을 로그에 출력합니다.

        pagestep.x = -width;

    }

    public void OnClickNextButton() // 버튼을 통한 우측으로 스와이프 애니메이션 실행
    {
        Pagestep();
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pagestep;
            MovePage();
        }
    }

    public void OnClickPreviousButton() //버튼을 통한 좌측으로 스와이프 애니메이션 실행
    {
        Pagestep();

        // ImpleementMainScreen씬 > Canvas_1 > StageScreen/_Chaos > Scroll View > Button_Prev
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pagestep;
            MovePage();
        }
    }

    void MovePage() //페이지 이동
    {
        // 애니메이션을 직접 구현합니다.
        StartCoroutine(MovePageCoroutine());
    }

    IEnumerator MovePageCoroutine()
    {
        Vector3 startPos = levelPageRect.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < tweenTime) //애니메이션이 진행될 시간이하동안만
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / tweenTime);
            float easedT = tweenCurve != null ? tweenCurve.Evaluate(t) : t; //트윈커브에서 선택한 가속도대로

            levelPageRect.localPosition = Vector3.Lerp(startPos, targetPos, easedT);

            yield return null;
        }

        levelPageRect.localPosition = targetPos;
    }
}
