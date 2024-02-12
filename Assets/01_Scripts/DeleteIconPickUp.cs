using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteIconPickUp : MonoBehaviour
{
    public GameObject slimeIconContent;
    public void DeleteSlimeIcon(UnityEngine.UI.Button button)
    {
        // 버튼의 Transform 컴포넌트에서 첫 번째 자식 오브젝트를 찾음
        if (button.transform.childCount > 0)
        {
            // 첫 번째 자식 오브젝트를 가져옴
            Transform firstChild = button.transform.GetChild(0);

            string slimeIconName = firstChild.name;

            // 해당 슬라임 아이콘이 레전드리 슬라임인지 확인
            if (SlimeManager.instance.selectedLegendSlime == slimeIconName)
            {
                // 선택된 레전드리 슬라임 초기화
                SlimeManager.instance.selectedLegendSlime = "";
            }

            // 해당 자식 오브젝트(슬라임 아이콘)를 삭제
            slimeIconContent.transform.Find(firstChild.name).GetComponent<PickUpSlime>().checkImage.SetActive(false);

            Destroy(firstChild.gameObject);
        }
        else
        {
            // 자식 오브젝트가 없는 경우, 콘솔에 메시지 출력
            Debug.Log("No child object found to delete.");
        }
    }
}
