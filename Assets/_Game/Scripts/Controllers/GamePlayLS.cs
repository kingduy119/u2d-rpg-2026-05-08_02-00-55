using VContainer;
using VContainer.Unity;
using UnityEngine;


public class GamePlayLS : LifetimeScope
{
    [SerializeField] ControlsSettings _controlsSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance<IControlsSettings>(_controlsSettings);

        builder.Register<IKeyBoardHandler, KeyBoardHandler>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<PlayerController>();
    }
}
