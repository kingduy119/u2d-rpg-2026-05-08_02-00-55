using VContainer;
using VContainer.Unity;

public class GameLoadingLS : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<SceneLoadingView>();
    }
}
