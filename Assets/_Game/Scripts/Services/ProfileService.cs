using System;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;


public class ProfileSerice : Service
{
    [Inject] readonly DownloadService _DownloadService;
    [Inject] readonly FirebaseService _FirebaseService;

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
        User = await _FirebaseService.LoadUserAsync(PlayerId);

        if (User == null)
        {
            User = new()
            {
                UserId = PlayerId,
                Nickname = "New Player",
            };
            UserResource = new();
            await _FirebaseService.SaveUserAsync(User);
            await _FirebaseService.SaveUserResourceAsync(User, UserResource);
        }
        else
        {
            UserResource = await _FirebaseService.LoadUserResourceAsync(User);
        }
        OnChange?.Invoke(this);

        var scene = User.FirstLogin ? SceneEvents.GAME_AUTH : SceneEvents.GAME_MENU;
        await _DownloadService.LoadScene(scene);
    }

    public void ClaimRewards(List<RewardData> rewards)
    {
        foreach (var reward in rewards)
        {
            switch (reward.Item.Type)
            {
                case ItemType.Diamond:
                    UserResource.Diamond += reward.Value;
                    break;
                case ItemType.Gold:
                    UserResource.Gold += reward.Value;
                    break;
                default: break;
            }
        }

        OnChange?.Invoke(this);
        _FirebaseService.SaveUserResourceAsync(User, UserResource).Forget();
    }

    public void PurchaseItem(ShopItemSO item)
    {
        if (item == null) return;
        ResourceData tempResource = new ResourceData
        {
            Diamond = UserResource.Diamond,
            Gold = UserResource.Gold
        };
        foreach (var requiredResource in item.RequiredResources)
        {
            switch (requiredResource.Item.Type)
            {
                case ItemType.Diamond:
                    if (UserResource.Diamond < requiredResource.Value)
                    {
                        Debug.LogWarning("Not enough diamonds to purchase the item.");
                        return;
                    }
                    tempResource.Diamond -= (int)requiredResource.Value;
                    break;
                case ItemType.Gold:
                    if (UserResource.Gold < requiredResource.Value)
                    {
                        Debug.LogWarning("Not enough gold to purchase the item.");
                        return;
                    }
                    tempResource.Gold -= (int)requiredResource.Value;
                    break;
                default:
                    Debug.LogWarning("Unsupported purchase type.");
                    return;
            }
        }

        UserResource = tempResource;
        ClaimRewards(item.Rewards);
        OnChange?.Invoke(this);
        _FirebaseService.SaveUserResourceAsync(User, UserResource).Forget();
    }

    private void ConfirmChangeNickname(string nickname)
    {
        if (User != null)
        {
            User.Nickname = nickname;
            User.FirstLogin = false;
            _FirebaseService.SaveUserAsync(User, onSuccess: () =>
            {
                _DownloadService.LoadScene(SceneEvents.GAME_MENU).Forget();
            });
        }
        else
        {
            Debug.LogWarning("User is null. Cannot set nickname.");
        }
    }
}

