using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameMenuLS : LifetimeScope
{

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<CharacterDetailTabView>();
    }

    protected override void OnDestroy()
    {
    }
}
