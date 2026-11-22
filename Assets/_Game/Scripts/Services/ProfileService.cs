using System;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using IAPLib;
using Core;


public class ProfileSerice : Service
{
    [Inject] readonly DownloadService m_DownloadService;
    [Inject] readonly DBService m_DBService;

    public Action<ProfileSerice> OnChange;

    // Global Data
    public UserData User { get; private set; }
    public ResourceData UserResource { get; private set; }

    // Gameplay Data
    public Resources gResources { get; private set; } = new();

    public override void Start()
    {
        ButtonEvents.ConfirmChangeNickname += ConfirmChangeNickname;
        ShopEvents.OnPurchase += PurchaseItem;
    }
    public override void Dispose()
    {
        ButtonEvents.ConfirmChangeNickname -= ConfirmChangeNickname;
        ShopEvents.OnPurchase -= PurchaseItem;
    }

    public async UniTaskVoid LoadUserAsync(string PlayerId)
    {
        User = await m_DBService.LoadUserAsync(PlayerId);

        if (User == null)
        {
            User = new()
            {
                UserId = PlayerId,
                Nickname = "New Player",
            };
            UserResource = new();
            await m_DBService.SaveUserAsync(User);
            await m_DBService.SaveUserResourceAsync(User, UserResource);
        }
        else
        {
            UserResource = await m_DBService.LoadUserResourceAsync(User);
        }
        OnChange?.Invoke(this);

        var scene = User.FirstLogin ? SceneEvents.GAME_AUTH : SceneEvents.GAME_MENU;
        await m_DownloadService.LoadScene(scene);
    }

    public void ClaimRewards(List<RewardItem> rewards)
    {
        foreach (var reward in rewards)
        {
            UserResource.Add(reward);
        }

        OnChange?.Invoke(this);
        m_DBService.SaveUserResourceAsync(User, UserResource).Forget();
    }

    public void PurchaseItem(ShopItemDto item)
    {
        if (item == null || item.PurchaseResource == null) return;

        if (UserResource < item.PurchaseResource)
        {
            Debug.LogWarning("Not enough resources to purchase the item.");
            return;
        }
        UserResource -= item.PurchaseResource;
        ClaimRewards(item.Rewards);
    }

    private void ConfirmChangeNickname(string nickname)
    {
        if (User != null)
        {
            User.Nickname = nickname;
            User.FirstLogin = false;
            m_DBService.SaveUserAsync(User, onSuccess: () =>
            {
                m_DownloadService.LoadScene(SceneEvents.GAME_MENU).Forget();
            });
        }
        else
        {
            Debug.LogWarning("User is null. Cannot set nickname.");
        }
    }
}

