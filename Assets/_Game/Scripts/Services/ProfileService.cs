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

    public Action<ResourceData> OnResourceChange;

    // Global Data
    public UserData User { get; private set; }
    public ResourceData UserResource { get; private set; }

    // Gameplay Data
    public Resources gResources { get; private set; } = new();

    public override void Start()
    {
        ButtonEvents.ConfirmChangeNickname += ConfirmChangeNickname;
        ButtonEvents.ClaimRewards += ClaimRewards;
    }
    public override void Dispose()
    {
        ButtonEvents.ConfirmChangeNickname -= ConfirmChangeNickname;
        ButtonEvents.ClaimRewards -= ClaimRewards;
    }

    public async UniTaskVoid LoadUserProfileAsync(string PlayerId)
    {
        var User = await _FirebaseService.GetUserAsync(PlayerId);

        if (User == null)
        {
            User = new()
            {
                UserId = PlayerId,
                Name = "New Player",
            };
            UserResource = new();
            await _FirebaseService.SaveUserAsync(User);
            await _FirebaseService.SaveUserResourceAsync(User, UserResource);
        }
        else
        {
            UserResource = await _FirebaseService.LoadUserResourceAsync(User);
        }

        var scene = User.FirstLogin ? SceneEvents.GAME_AUTH : SceneEvents.GAME_MENU;
        await _DownloadService.LoadScene(scene);
    }

    private void ClaimRewards(List<ItemSO> rewards)
    {
        foreach (var reward in rewards)
        {
            switch (reward.Type)
            {
                case ItemType.Diamond:
                    UserResource.Diamond += reward.value;
                    break;
                case ItemType.Gold:
                    UserResource.Gold += reward.value;
                    break;
                default: break;
            }
        }

        // _UserProfileView.UpdateView(_GameManager.UserResource);
        OnResourceChange?.Invoke(UserResource);
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

