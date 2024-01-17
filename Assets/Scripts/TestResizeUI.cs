using UnityEngine;

public class TestResizeUI : MonoBehaviour //��⸶�� ĵ���� ����� 1�� �޾ƿͼ� ������ ���� �ٲ���, UIManager�� ���ĺ��� ������ ����//StageScreen ��
{ //UIManager ������Ʈ�� ����ֽ��ϴ�
    public RectTransform canvasRectTransform; // ĵ������ RectTransform
    public RectTransform[] uiElements; // ũ�⸦ ������ UI ���

    void Start()
    {
        // ĵ������ ���� ũ�⸦ �����ɴϴ�.
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // uiElements �迭�� �� UI ����� ũ�⸦ ĵ���� ũ�⿡ ����ϴ�.
        foreach (RectTransform element in uiElements)
        {
            if (element != null)
            {
                element.sizeDelta = new Vector2(canvasSize.x, canvasSize.y);
            }
            else
            {
                Debug.LogError("RectTransform is null in uiElements array");
            }
        }
    }
}
