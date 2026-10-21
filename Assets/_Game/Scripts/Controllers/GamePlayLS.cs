using VContainer;
using VContainer.Unity;
using UnityEngine;
using Characters;
using System;
using Weapons;


public class GamePlayLS : LifetimeScope
{
    [SerializeField] ControlsSettings _controlsSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GamePlay>(Lifetime.Singleton);

        builder.RegisterInstance<IControlsSettings>(_controlsSettings);

        builder.Register<IKeyBoardHandler, KeyBoardHandler>(Lifetime.Singleton);

        // builder.RegisterComponentInHierarchy<PlayerController>();
    }


}

public class GamePlay : IStartable, IDisposable
{
    [Inject] readonly FactoryService _factory;

    public void Start()
    {
        Debug.Log($"GamePlay.Start: {_factory != null}");
        CterEvent.Shoot += Character_Shoot;
    }

    public void Dispose()
    {
        CterEvent.Shoot -= Character_Shoot;
    }

    private void Character_Shoot(Character cter, ProjectSO projectSO)
    {
        // if (projectSO == null) return;
        Vector2 direction = (cter.Target.position - cter.transform.position).normalized;
        var projectile = _factory.GetPrefab(projectSO);

        if (projectile != null)
        {
            projectile.Target = cter.Target.gameObject;
            projectile.Launch(cter.AttackPoint.transform, direction);
        }
    }
}
