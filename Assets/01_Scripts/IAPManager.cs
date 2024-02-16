using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IAP ��� 
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{

    [Header("Product ID")]
    [SerializeField] private string jelly1000 = "jelly1000";

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
        builder.AddProduct(jelly1000, ProductType.Consumable);
        
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

        var product = purchaseEvent.purchasedProduct;
        Debug.Log("���ſ� �����߽��ϴ�" + product.definition.id);

        if (product.definition.id == jelly1000)
        {
            CurrenyManager.Instance.jellyStone += 1000;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave(); // �پƷ� ���� 
            
        }

        return PurchaseProcessingResult.Complete;
    }
}
