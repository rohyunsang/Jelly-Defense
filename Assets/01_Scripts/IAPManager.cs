using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IAP ��� 
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    #region SingleTon Pattern
    public static IAPManager instance;  // Singleton instance
    void Awake() // SingleTon
    {
        // �̹� �ν��Ͻ��� �����ϸ鼭 �̰� �ƴϸ� �ı� ��ȯ
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Header("Product ID")]
    public readonly string productId_test_id = "test_id";
    public readonly string productId_test_id2 = "test_id2";

    [Header("Cache")]
    private IStoreController storeController; //���� ������ �����ϴ� �Լ� ������
    private IExtensionProvider storeExtensionProvider; //���� �÷����� ���� Ȯ�� ó�� ������

    private void Start()
    {
        InitUnityIAP(); //Start ������ �ʱ�ȭ �ʼ�
    }

    /* Unity IAP�� �ʱ�ȭ�ϴ� �Լ� */
    private void InitUnityIAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        /* ���� �÷��� ��ǰ�� �߰� */
        builder.AddProduct(productId_test_id, ProductType.Consumable, new IDs() { { productId_test_id, GooglePlay.Name } });
        builder.AddProduct(productId_test_id2, ProductType.Consumable, new IDs() { { productId_test_id2, GooglePlay.Name } });

        UnityPurchasing.Initialize(this, builder);
    }

    /* �����ϴ� �Լ� */
    public void Purchase(string productId)
    {
        Product product = storeController.products.WithID(productId); //��ǰ ����

        if (product != null && product.availableToPurchase) //��ǰ�� �����ϸ鼭 ���� �����ϸ�
        {
            storeController.InitiatePurchase(product); //���Ű� �����ϸ� ����
        }
        else //��ǰ�� �������� �ʰų� ���� �Ұ����ϸ�
        {
            Debug.Log("��ǰ�� ���ų� ���� ���Ű� �Ұ����մϴ�");
        }
    }

    #region Interface
    /* �ʱ�ȭ ���� �� ����Ǵ� �Լ� */
    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�");

        storeController = controller;
        storeExtensionProvider = extension;
    }

    /* �ʱ�ȭ ���� �� ����Ǵ� �Լ� */
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�");
    }

    /* ���ſ� �������� �� ����Ǵ� �Լ� */
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("���ſ� �����߽��ϴ�");
    }

    /* ���Ÿ� ó���ϴ� �Լ� */
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("���ſ� �����߽��ϴ�");

        if (args.purchasedProduct.definition.id == productId_test_id)
        {
            /* test_id ���� ó�� */
        }
        else if (args.purchasedProduct.definition.id == productId_test_id2)
        {
            /* test_id2 ���� ó�� */
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
