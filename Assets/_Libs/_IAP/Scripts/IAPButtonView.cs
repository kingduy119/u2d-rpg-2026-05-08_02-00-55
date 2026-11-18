using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPButtonView : MonoBehaviour
{

    [SerializeField]
    TMP_Text title;
    [SerializeField]
    TMP_Text price;


    public void OnProductFetched(Product product)
    {
        // Debug.Log("OnProductFetched");
        // _product = product;

        if (title != null)
        {
            title.text = product.metadata.localizedTitle;
        }

        if (price != null)
        {
            price.text = "$" + product.metadata.localizedPriceString;
        }


    }

    // public void OnProductFetchFailed(ProductDefinition definition, string error)
    // {
    //     Debug.Log("OnProductFetchFailed");
    // }

    // public void OnOderPending(PendingOrder pendingOrder)
    // {
    //     Debug.Log("OnOderPending");
    // }

    // public void OnOrderConfirm(ConfirmedOrder confirm)
    // {
    //     Debug.Log("OnOrderConfirm");
    // }

    // public void OnPurchaseFailed(FailedOrder failedOrder)
    // {
    //     Debug.Log("OnPurchaseFailed");

    // }
}
