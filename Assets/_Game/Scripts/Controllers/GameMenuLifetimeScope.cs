using VContainer;
using VContainer.Unity;

public class GameMenuLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<NavSceneButtonView>();
    }
}
