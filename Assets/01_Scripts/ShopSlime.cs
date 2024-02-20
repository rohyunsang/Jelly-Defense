using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PurchaseOption
{
    JellyOnly, // �����θ� ���� ����
    GoldOnly, // ���θ� ���� ����
    Both // ������ ��� �� �ٷ� ���� ����
}

public class ShopSlime : MonoBehaviour
{
    public string slimeName; // �������� �̸�
    public int goldPrice; // ���� ������ ���� ����
    public int jellyPrice; // ������ ������ ���� ����

    public PurchaseOption purchaseOption; // ���� �ɼ�

    // Ŭ�� �̺�Ʈ�� ó���ϴ� �޼���
    public void OnShopSlimeClicked()
    {
        UIManager.instance.UIClickSound();
        slimeName = transform.name.Replace("Icon(Clone)", "");
        ShopManager.Instance.goldPrice = goldPrice;
        ShopManager.Instance.jellyPrice = jellyPrice;
        ShopManager.Instance.purchasePanel.SetActive(true);
        ShopManager.Instance.purchaseOption = purchaseOption;
        ShopManager.Instance.currentSlimeName = slimeName;

        ShopManager.Instance.OnPurchasePanel();
    }

}
