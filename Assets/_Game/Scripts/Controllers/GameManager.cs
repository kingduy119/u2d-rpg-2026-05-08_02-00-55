using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;
using IAPPack;
using UnityEngine.Purchasing;
using System.Collections.Generic;

public enum IAPProductKeys
{
    iap_diamon_500
}
public class GameManager : IAsyncStartable, IDisposable
{

    // [Inject] readonly DownloadService _DownloadService;
    [Inject] readonly AuthService _AuthService;
    [Inject] readonly FacebookService _FacebookService;
    [Inject] readonly ProfileSerice _ProfileService;
    [Inject] readonly FactoryService _FactoryService;

    readonly public IAPManager _IAPManager = new();

    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        ButtonEvents.OnAuth += OnAuth;

        _AuthService.OnLoginSuccess += OnLoginSuccess;
        _AuthService.SignInAnonymouslyAsync(cancellation).Forget();

        _IAPManager.OnFetchProduct += OnFetchProduct;
        await _IAPManager.ConnectAsync().AsUniTask().AttachExternalCancellation(cancellation);
    }

    private void OnFetchProduct(StoreController store)
    {
        var intialProductToFetch = new List<ProductDefinition>
            {
                new(IAPProductKeys.iap_diamon_500.ToString(), ProductType.Consumable),
                // new("wood100", ProductType.Consumable),
                // new("remove_ads", ProductType.NonConsumable)
            };
        store.FetchProducts(intialProductToFetch);
    }

    public void Dispose()
    {
        ButtonEvents.OnAuth -= OnAuth;

        _AuthService.OnLoginSuccess -= OnLoginSuccess;

        _FactoryService.Release();

    }

    private void OnLoginSuccess(string PlayerId) =>
        _ProfileService.LoadUserAsync(PlayerId).Forget();

    public void OnAuth(AuthMethods method)
    {
        switch (method)
        {
            case AuthMethods.Anonymously:
                _AuthService.SignInAnonymouslyAsync().Forget();
                break;
            case AuthMethods.Facebook:
                _FacebookService.Login();
                break;
            case AuthMethods.FacebookUnlink:
                _FacebookService.UnlinkFacebookAsync().Forget();
                break;
            case AuthMethods.Logout:
                _AuthService.SignOut();
                break;
            default:
                break;
        }
    }

}

