
using AdsLib;
using IAPLib;
using UnityEngine;
using VContainer;
using VContainer.Unity;



public class GameManagerLS : LifetimeScope
{
    [SerializeField] AdsSettingSO AdsSetting;

    protected override void Configure(IContainerBuilder builder)
    {
        // builder.RegisterComponentInHierarchy<AdsConfigView>();
        builder.RegisterInstance(AdsSetting);

        builder.Register<IAPManager>(Lifetime.Singleton).AsSelf();
        builder.Register<AdsManager>(Lifetime.Singleton).AsSelf();

        builder.RegisterEntryPoint<GameManager>(Lifetime.Singleton).AsSelf();

        builder.RegisterEntryPoint<DownloadService>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<DBService>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<AuthService>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<FacebookService>(Lifetime.Singleton).AsSelf();

        builder.RegisterEntryPoint<ProfileSerice>(Lifetime.Singleton).AsSelf();
        // builder.RegisterEntryPoint<InventoryService>(Lifetime.Singleton).AsSelf();


        builder.Register<FactoryService>(Lifetime.Singleton);
    }

}
