using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PurchaseOption
{
    JellyOnly, // 젤리로만 구매 가능
    GoldOnly, // 골드로만 구매 가능
    Both // 젤리와 골드 둘 다로 구매 가능
}

public class ShopSlime : MonoBehaviour
{
    public string slimeName; // 슬라임의 이름
    public int goldPrice; // 골드로 구매할 때의 가격
    public int jellyPrice; // 젤리로 구매할 때의 가격

    public PurchaseOption purchaseOption; // 구매 옵션

    // 클릭 이벤트를 처리하는 메서드
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
