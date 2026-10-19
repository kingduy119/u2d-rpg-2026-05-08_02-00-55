using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameMenuLS : LifetimeScope
{

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<CharacterDetailTabView>();
        // Debug.Log("GameMenuLS.Configure");
    }

    protected override void OnDestroy()
    {
        // base.OnDestroy();
        // Debug.Log("GameMenuLS.OnDestroy");
    }
}
