using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public interface IIAPEvents
{
    void RegisteEvents();
    void RemoveEvents();
}

public class IAPAtrribute
{
    protected StoreController _StoreController;
    public bool IsConnected { get; protected set; } = false;
    protected Dictionary<string, Product> Products { get; }

}

public class IAPEvents : IAPAtrribute, IIAPEvents
{
    public virtual void RegisteEvents() { }
    public virtual void RemoveEvents() { }
}

public class IAPConnect : IAPEvents
{
    public override void RegisteEvents()
    {
        base.RegisteEvents();
        _StoreController.OnStoreConnected += OnStoreConnected;
        _StoreController.OnStoreDisconnected += OnStoreDisconnected;
    }

    public override void RemoveEvents()
    {
        base.RemoveEvents();
        _StoreController.OnStoreConnected -= OnStoreConnected;
        _StoreController.OnStoreDisconnected -= OnStoreDisconnected;
    }

    private void OnStoreConnected() => IsConnected = true;
    private void OnStoreDisconnected(StoreConnectionFailureDescription failure)
    {
        IsConnected = false;
        Debug.Log($"OnStoreDisconnected: {failure}");
    }
}

public class IAPProductHandling : IAPConnect
{
    public override void RegisteEvents()
    {
        base.RegisteEvents();
        _StoreController.OnProductsFetched += OnProductsFetched;
        _StoreController.OnProductsFetchFailed += OnProductsFetchFailed;
    }

    public override void RemoveEvents()
    {
        base.RemoveEvents();
        _StoreController.OnProductsFetched -= OnProductsFetched;
        _StoreController.OnProductsFetchFailed -= OnProductsFetchFailed;
    }

    private void OnProductsFetched(List<Product> products)
    {
        foreach (var p in products)
        {
            string productId = p.definition.id;
            string title = p.metadata.localizedTitle;

            Products.Add(productId, p);
            Debug.Log($"OnProductsFetched: {productId} - {title}");
        }

        _StoreController.FetchPurchases();
    }

    private void OnProductsFetchFailed(ProductFetchFailed failure)
    {
        Debug.Log($"OnProductsFetchFailed: {failure}");
    }
}


public class IAPPurchaseHandling : IAPProductHandling
{
    private string _name = "IAPPurchaseHandling";
    public override void RegisteEvents()
    {
        if (_StoreController == null) return;

        base.RegisteEvents();
        _StoreController.OnPurchasesFetched += OnPurchasesFetched;
        _StoreController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;
        _StoreController.OnPurchasePending += OnPurchasePending;
        _StoreController.OnPurchaseConfirmed += OnPurchaseConfirmed;
        _StoreController.OnPurchaseFailed += OnPurchaseFailed;
        _StoreController.OnPurchaseDeferred += OnPurchaseDeferred;
    }

    public override void RemoveEvents()
    {
        if (_StoreController == null) return;

        base.RemoveEvents();
        _StoreController.OnPurchasesFetched -= OnPurchasesFetched;
        _StoreController.OnPurchasesFetchFailed -= OnPurchasesFetchFailed;
        _StoreController.OnPurchasePending -= OnPurchasePending;
        _StoreController.OnPurchaseConfirmed -= OnPurchaseConfirmed;
        _StoreController.OnPurchaseFailed -= OnPurchaseFailed;
        _StoreController.OnPurchaseDeferred -= OnPurchaseDeferred;
    }

    private void OnPurchasesFetched(Orders orders)
    {
        Debug.Log($"OnPurchasesFetched: {orders}");
    }

    private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription failure)
    {
    }

    private void OnPurchasePending(PendingOrder order)
    {
        var product = order.CartOrdered.Items().First()?.Product;
        Debug.Log($"[{_name}] Pending purchase: {product.definition.id}");

        Debug.Log($"[{_name}] OnPurchasePending: {order}");
        _StoreController.ConfirmPurchase(order);
    }

    private void OnPurchaseConfirmed(Order order)
    {
        var product = order.CartOrdered.Items().First()?.Product;
        Debug.Log($"[{_name}] Confirmed purchase: {product.definition.id}");

        GrantReward(product.definition.id);
    }

    private void GrantReward(string productId)
    {
        Debug.Log($"[{_name}] GrantReward: {productId}");
    }

    void OnPurchaseFailed(FailedOrder order)
    {
        var product = order.CartOrdered?.Items()?.FirstOrDefault()?.Product;
        var storeId = product?.definition.id;

        Debug.LogWarning($"[{_name}] Purchase failed for {storeId}: {order.FailureReason}");
        // switch (order.FailureReason)
        // {
        //     case PurchaseFailureReason.UserCancelled:
        //         // Không cần thông báo lỗi
        //         break;

        //     default:
        //         // Hiển thị thông báo thanh toán thất bại
        //         // UI.ShowError("Purchase failed!");
        //         break;
        // }
        // Debug.LogError($"IAP Failed: {order.FailureReason} | {order.Details}");
    }

    private void OnPurchaseDeferred(DeferredOrder order)
    {
        Debug.Log($"OnPurchaseDeferred: ");
    }
}



