using UnityEngine;
using VContainer;
using VContainer.Unity;



public class GameManagerLS : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // builder.RegisterComponentInHierarchy<SceneLoadingView>();

        builder.RegisterEntryPoint<GameManager>(Lifetime.Singleton);
        builder.RegisterEntryPoint<DownloadService>(Lifetime.Singleton).AsSelf();

        builder.Register<FactoryService>(Lifetime.Singleton);
    }
}
