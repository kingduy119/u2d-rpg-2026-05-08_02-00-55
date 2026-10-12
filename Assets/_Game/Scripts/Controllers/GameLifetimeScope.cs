using UnityEngine;
using VContainer;
using VContainer.Unity;



public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<SceneLoadingView>();

        builder.RegisterEntryPoint<GameManager>(Lifetime.Singleton);
        builder.RegisterEntryPoint<DownloadService>(Lifetime.Singleton);

        builder.Register<FactoryService>(Lifetime.Singleton);
    }
}
