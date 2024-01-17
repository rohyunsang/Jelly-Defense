using UnityEngine;

public class TestResizeUI : MonoBehaviour //기기마다 캔버스 사이즈를 1번 받아와서 사이즈 설정 바꿔줌, UIManager에 합쳐보려 했으나 실패//StageScreen 등
{ //UIManager 오브젝트에 들어있습니다
    public RectTransform canvasRectTransform; // 캔버스의 RectTransform
    public RectTransform[] uiElements; // 크기를 조정할 UI 요소

    void Start()
    {
        // 캔버스의 현재 크기를 가져옵니다.
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // uiElements 배열의 각 UI 요소의 크기를 캔버스 크기에 맞춥니다.
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
