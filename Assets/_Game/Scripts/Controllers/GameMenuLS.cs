using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameMenuLS : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameMenuController>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<UserProfileView>();
        builder.RegisterComponentInHierarchy<CharacterDetailTabView>();
    }
}


public class GameMenuController : Service
{
    // [Inject] readonly FactoryService _FactoryService;
    // [Inject] readonly GameManager _GameManager;
    [Inject] readonly AdsManager _AdsManager;

    // public override void Start()
    // {
    // }

    public override void PostStart()
    {
        _AdsManager.RandomInterstitalAds();
    }

}