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

        builder.RegisterEntryPoint<GameManager>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<AdsManager>(Lifetime.Singleton).AsSelf();

        builder.RegisterEntryPoint<DownloadService>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<ProfileSerice>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<FirebaseService>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<AuthService>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<FacebookService>(Lifetime.Singleton).AsSelf();


        builder.Register<FactoryService>(Lifetime.Singleton);
    }
}


