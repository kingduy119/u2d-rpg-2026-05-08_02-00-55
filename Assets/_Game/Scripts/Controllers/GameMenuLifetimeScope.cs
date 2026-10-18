using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameMenuLifetimeScope : LifetimeScope
{

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<CharacterDetailTabView>();
        // Debug.Log("GameMenuLifetimeScope.Configure");
    }

    protected override void OnDestroy()
    {
        // base.OnDestroy();
        // Debug.Log("GameMenuLifetimeScope.OnDestroy");
    }
}
