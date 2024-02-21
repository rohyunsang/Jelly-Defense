using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IAP 사용 
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{

    [Header("Product ID")]
    [SerializeField] private string jelly900 = "jelly900";
    [SerializeField] private string jelly1500 = "jelly1500";
    [SerializeField] private string jelly2700 = "jelly2700";
    [SerializeField] private string jelly3500 = "jelly3500";
    [SerializeField] private string jelly1900 = "jelly1900";
    [SerializeField] private string jelly4500 = "jelly4500";

    [Header("Cache")]
    private IStoreController storeController; //구매 과정을 제어하는 함수 제공자
    private IExtensionProvider storeExtensionProvider; //여러 플랫폼을 위한 확장 처리 제공자

    void Start()
    {
        InitIAP();//Start 문에서 초기화 필수
    }

    /* Unity IAP를 초기화하는 함수 */
    private void InitIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        /* 구글 플레이 상품들 추가 */
        builder.AddProduct(jelly900, ProductType.Consumable);
        builder.AddProduct(jelly1500, ProductType.Consumable);
        builder.AddProduct(jelly2700, ProductType.Consumable);
        builder.AddProduct(jelly3500, ProductType.Consumable);
        builder.AddProduct(jelly1900, ProductType.Consumable);
        builder.AddProduct(jelly4500, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    /* 구매하는 함수 */
    public void Purchase(string productId)
    {
       storeController.InitiatePurchase(productId);
    }

    private void CheckNonConsumalbe(string id)
    {
        var product = storeController.products.WithID(id);

        if (product != null)
        {
            bool isCheck = product.hasReceipt;
        }
    }

    /* 초기화 성공 시 실행되는 함수 */
    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        Debug.Log("초기화에 성공했습니다");

        storeController = controller;
    }

    /* 초기화 실패 시 실행되는 함수 */
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화에 실패했습니다" + error);
    }
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("초기화에 실패했습니다" + error + message);
    }

    /* 구매에 실패했을 때 실행되는 함수 */
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("구매에 실패했습니다");
    }

    /* 구매를 처리하는 함수 */
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        UIManager.instance.UIClickSound();

        var product = purchaseEvent.purchasedProduct;
        Debug.Log("구매에 성공했습니다" + product.definition.id);
        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_PurchaseSound);

        if (product.definition.id == jelly900)
        {
            CurrenyManager.Instance.jellyStone += 100;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // 바아로 저장 
        }
        else if (product.definition.id == jelly1500)
        {
            CurrenyManager.Instance.jellyStone += 220;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // 바아로 저장 
        }
        else if (product.definition.id == jelly2700)
        {
            CurrenyManager.Instance.jellyStone += 520;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // 바아로 저장 
        }
        else if (product.definition.id == jelly3500)
        {
            CurrenyManager.Instance.jellyStone += 950;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // 바아로 저장 
        }
        else if (product.definition.id == jelly1900)  // 특별 패키지
        {
            Debug.Log("특별패키지 구매완료:");
            CurrenyManager.Instance.jellyStone += 500;
            CurrenyManager.Instance.gold += 50000;
            CurrenyManager.Instance.actionPoint += 100;

            SlimeManager.instance.UpdateSlime("BearSlime");

            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // 바아로 저장 
        }
        else if (product.definition.id == jelly4500) // 광고 제거 
        {
            //CurrenyManager.Instance.jellyStone += 1000;
            //UIManager.instance.AsycCurrenyUI();
            //DataManager.Instance.JsonSave(); // 바아로 저장 
        }

        return PurchaseProcessingResult.Complete;
    }
}
