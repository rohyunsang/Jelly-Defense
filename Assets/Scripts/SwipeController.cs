using System.Collections;
using UnityEngine;

public class SwipeController : MonoBehaviour
{   //�۵��� ���� ���� ���� �ð��� �������ִ��� Ȯ���� ��
    //�����̴��� ���������� ���ϴ� �̵�����ŭ �ִϸ��̼�ó�� �������ִ� ��ũ��Ʈ.
    //ImpleementMainScreen��> Canvas_1> StageScreen> Scroll View,
    //ImpleementMainScreen��> Canvas_1> StageScreen_Chaos> Scroll View �� ������

    [SerializeField] int maxPage; // �ִ� ������ ��
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pagestep; // ������ �ܰ�
    [SerializeField] RectTransform levelPageRect; // ������ �ܰ�

    [SerializeField] float tweenTime; // �ִϸ��̼� �ð� (��)
    [SerializeField] AnimationCurve tweenCurve; // �ִϸ��̼� � (�ɼ�)


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
        // �ڽ��� RectTransform ������Ʈ�� �����ɴϴ�.
        RectTransform rectTransform = GetComponent<RectTransform>();

        // ���ΰ��� �����ɴϴ�.
        float width = rectTransform.rect.width;
        // Debug.Log("Width: " + width); // ���ΰ��� �α׿� ����մϴ�.

        pagestep.x = -width;

    }

    public void OnClickNextButton() // ��ư�� ���� �������� �������� �ִϸ��̼� ����
    {
        Pagestep();
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pagestep;
            MovePage();
        }
    }

    public void OnClickPreviousButton() //��ư�� ���� �������� �������� �ִϸ��̼� ����
    {
        Pagestep();

        // ImpleementMainScreen�� > Canvas_1 > StageScreen/_Chaos > Scroll View > Button_Prev
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pagestep;
            MovePage();
        }
    }

    void MovePage() //������ �̵�
    {
        // �ִϸ��̼��� ���� �����մϴ�.
        StartCoroutine(MovePageCoroutine());
    }

    IEnumerator MovePageCoroutine()
    {
        Vector3 startPos = levelPageRect.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < tweenTime) //�ִϸ��̼��� ����� �ð����ϵ��ȸ�
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / tweenTime);
            float easedT = tweenCurve != null ? tweenCurve.Evaluate(t) : t; //Ʈ��Ŀ�꿡�� ������ ���ӵ����

            levelPageRect.localPosition = Vector3.Lerp(startPos, targetPos, easedT);

            yield return null;
        }

        levelPageRect.localPosition = targetPos;
    }
}
