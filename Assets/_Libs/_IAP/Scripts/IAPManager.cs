using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

// public static class IAPEvents
// {
//     public static Action<string> OnBuyProductSuccess;
// }

namespace IAPPack
{
    public class IAPManager : IAPPurchaseHandle
    {
        public event Action<StoreController> OnFetchProduct;
        public StoreController Controller => _StoreController;

        public async Task ConnectAsync(string env = "development")
        {
            try
            {
                var option = new InitializationOptions().SetEnvironmentName(env);
                await UnityServices.InitializeAsync(option);

                _StoreController ??= UnityIAPServices.StoreController();
                RegisteEvents();

                await _StoreController.Connect();
                OnFetchProduct?.Invoke(_StoreController);
            }
            catch (Exception e)
            {
                Debug.LogError($"[IAP] Connect failed: {e}");
            }
        }

        public void BuyProduct(string productId)
        {
            Debug.Log($"BuyProduct: {productId}");
            if (!string.IsNullOrEmpty(productId))
            {
                _StoreController.PurchaseProduct(productId);
            }
            else
            {
                Debug.LogWarning("Invalid product key: " + productId);
            }
        }
    }
}
