using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteIconPickUp : MonoBehaviour
{
    public GameObject slimeIconContent;
    public void DeleteSlimeIcon(UnityEngine.UI.Button button)
    {
        // ��ư�� Transform ������Ʈ���� ù ��° �ڽ� ������Ʈ�� ã��
        if (button.transform.childCount > 0)
        {
            // ù ��° �ڽ� ������Ʈ�� ������
            Transform firstChild = button.transform.GetChild(0);

            // �ش� �ڽ� ������Ʈ(������ ������)�� ����
            slimeIconContent.transform.Find(firstChild.name+"Icon").GetComponent<PickUpSlime>().checkImage.SetActive(false);


            Destroy(firstChild.gameObject);
        }
        else
        {
            // �ڽ� ������Ʈ�� ���� ���, �ֿܼ� �޽��� ���
            Debug.Log("No child object found to delete.");
        }
    }
}