using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IAP ��� 
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
    private IStoreController storeController; //���� ������ �����ϴ� �Լ� ������
    private IExtensionProvider storeExtensionProvider; //���� �÷����� ���� Ȯ�� ó�� ������

    void Start()
    {
        InitIAP();//Start ������ �ʱ�ȭ �ʼ�
    }

    /* Unity IAP�� �ʱ�ȭ�ϴ� �Լ� */
    private void InitIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        /* ���� �÷��� ��ǰ�� �߰� */
        builder.AddProduct(jelly900, ProductType.Consumable);
        builder.AddProduct(jelly1500, ProductType.Consumable);
        builder.AddProduct(jelly2700, ProductType.Consumable);
        builder.AddProduct(jelly3500, ProductType.Consumable);
        builder.AddProduct(jelly1900, ProductType.Consumable);
        builder.AddProduct(jelly4500, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    /* �����ϴ� �Լ� */
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

    /* �ʱ�ȭ ���� �� ����Ǵ� �Լ� */
    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�");

        storeController = controller;
    }

    /* �ʱ�ȭ ���� �� ����Ǵ� �Լ� */
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�" + error);
    }
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�" + error + message);
    }

    /* ���ſ� �������� �� ����Ǵ� �Լ� */
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("���ſ� �����߽��ϴ�");
    }

    /* ���Ÿ� ó���ϴ� �Լ� */
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        UIManager.instance.UIClickSound();

        var product = purchaseEvent.purchasedProduct;
        Debug.Log("���ſ� �����߽��ϴ�" + product.definition.id);
        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_PurchaseSound);

        if (product.definition.id == jelly900)
        {
            CurrenyManager.Instance.jellyStone += 100;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // �پƷ� ���� 
        }
        else if (product.definition.id == jelly1500)
        {
            CurrenyManager.Instance.jellyStone += 220;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // �پƷ� ���� 
        }
        else if (product.definition.id == jelly2700)
        {
            CurrenyManager.Instance.jellyStone += 520;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // �پƷ� ���� 
        }
        else if (product.definition.id == jelly3500)
        {
            CurrenyManager.Instance.jellyStone += 950;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // �پƷ� ���� 
        }
        else if (product.definition.id == jelly1900)  // Ư�� ��Ű��
        {
            Debug.Log("Ư����Ű�� ���ſϷ�:");
            CurrenyManager.Instance.jellyStone += 500;
            CurrenyManager.Instance.gold += 50000;
            CurrenyManager.Instance.actionPoint += 100;

            SlimeManager.instance.UpdateSlime("BearSlime");

            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // �پƷ� ���� 
        }
        else if (product.definition.id == jelly4500) // ���� ���� 
        {
            //CurrenyManager.Instance.jellyStone += 1000;
            //UIManager.instance.AsycCurrenyUI();
            //DataManager.Instance.JsonSave(); // �پƷ� ���� 
        }

        return PurchaseProcessingResult.Complete;
    }
}
