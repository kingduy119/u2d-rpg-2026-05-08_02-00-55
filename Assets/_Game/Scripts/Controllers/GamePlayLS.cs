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
        builder.RegisterEntryPoint<GamePlayController>(Lifetime.Singleton);
        builder.RegisterEntryPoint<InputService>(Lifetime.Singleton).AsSelf();


        builder.RegisterInstance<IControlsSettings>(_controlsSettings);

        builder.Register<IKeyBoardHandler, KeyBoardHandler>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<PlayerSelectTilemapView>();
        builder.RegisterComponentInHierarchy<GamePlayUIView>();
    }
}

public class GamePlayController : IStartable, IDisposable, ITickable
{
    [Inject] readonly FactoryService _factory;
    [Inject] readonly public InputService Input;
    [Inject] readonly public PlayerSelectTilemapView Tilemap;
    [Inject] readonly public GamePlayUIView UIView;

    public readonly StateMachine States = new();
    public IState IdleState;
    public IState StartState;

    public void Start()
    {
        IdleState = new GameIdleState(this);
        StartState = new GameStartState(this);
        States.Initialize(IdleState);

        CterEvent.Shoot += Character_Shoot;
    }

    public void Dispose()
    {
        CterEvent.Shoot -= Character_Shoot;

        _factory.ClearPool();
    }

    private void Character_Shoot(Character cter, ProjectSO projectSO)
    {
        Vector2 direction = (cter.Target.position - cter.transform.position).normalized;
        var projectile = _factory.GetPrefab(projectSO);

        if (projectile != null)
        {
            projectile.Target = cter.Target.gameObject;
            projectile.Launch(cter.AttackPoint.transform, direction);
        }
    }

    public void Tick()
    {
        States.Execute();
    }
}

