using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public interface IIAPEventsHandle
{
    void RegisteEvents();
    void RemoveEvents();
}

public class IAPAtrribute
{
    protected StoreController _StoreController;
    public bool IsConnected { get; protected set; } = false;
    protected Dictionary<string, Product> Products { get; } = new();

}

public class IAPEventsHandle : IAPAtrribute, IIAPEventsHandle
{
    public virtual void RegisteEvents() { }
    public virtual void RemoveEvents() { }
}

public class IAPConnect : IAPEventsHandle
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

public class IAPProductFetchHandle : IAPConnect
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
        foreach (var product in products)
        {
            string productId = product.definition.id;
            string title = product.metadata.localizedTitle;

            Debug.Log($"[IAPProductFetchHandle] OnProductsFetched: {productId} - {title}");
            Products.Add(productId, product);
        }

        _StoreController.FetchPurchases();
    }

    private void OnProductsFetchFailed(ProductFetchFailed failure)
    {
        Debug.Log($"[IAPProductFetchHandle] OnProductsFetchFailed: {failure}");
    }
}


public class IAPPurchaseHandle : IAPProductFetchHandle
{
    public Action<string> PurchaseConfirmed;
    private string _name = "IAPPurchaseHandle";
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
        Debug.Log($"[{_name}] OnPurchasesFetched: {orders}");
        Debug.Log($"[{_name}] orders.ConfirmedOrders: {orders.ConfirmedOrders.Count}");
        Debug.Log($"[{_name}] orders.PendingOrders: {orders.PendingOrders.Count}");
    }
    private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription failure)
    {
        Debug.Log($"[{_name}] OnPurchasesFetchFailed: {failure}");
    }

    private void OnPurchasePending(PendingOrder order)
    {
        var product = order.CartOrdered.Items().First()?.Product;
        Debug.Log($"[{_name}] Pending purchase: {product.definition.id}");

        _StoreController.ConfirmPurchase(order);
    }

    private void OnPurchaseConfirmed(Order order)
    {
        var product = order.CartOrdered.Items().First()?.Product;
        Debug.Log($"[{_name}] Confirmed purchase: {product.definition.id}");

        // IAPEvents.OnBuyProductSuccess?.Invoke(product.definition.id);
        PurchaseConfirmed?.Invoke(product.definition.id);
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



