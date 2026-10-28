using VContainer;
using VContainer.Unity;
using UnityEngine;
using Characters;
using System;
using Weapons;
using System.Collections.Generic;


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
    [Inject] readonly public FactoryService Factory;
    [Inject] readonly public InputService Input;
    [Inject] readonly public PlayerSelectTilemapView Tilemap;
    [Inject] readonly public GamePlayUIView UIView;

    public List<TargetController> Targets = new();
    public List<TargetController> Enemies = new();
    public Dictionary<Vector3Int, TargetController> MapObjects = new();
    public Dictionary<Vector3Int, CharacterSO> MapData = new();

    public readonly StateMachine States = new();
    public IState IdleState;
    public IState StartState;

    public void Start()
    {
        IdleState = new GameIdleState(this);
        StartState = new GameStartState(this);
        States.Initialize(IdleState);

        CterEvent.Shoot += Character_Shoot;
        GamePlayEvents.AddTarget += AddTarget;
        ButtonEvents.Gameplay += Gameplay;
    }

    public void Tick()
    {
        States.Execute();
    }

    public void Dispose()
    {
        CterEvent.Shoot -= Character_Shoot;
        GamePlayEvents.AddTarget -= AddTarget;
        ButtonEvents.Gameplay -= Gameplay;

        Factory.ClearPool();
    }

    private void Gameplay(string eventName)
    {
        switch (eventName)
        {
            case "StartGame":
                States.TransitionTo(StartState);
                break;
            case "BackToIdle":
                States.TransitionTo(IdleState);
                break;
            default:
                Debug.LogWarning($"Unknown event: {eventName}");
                break;
        }
    }

    private void AddTarget(string team, TargetController target)
    {
        if (team == "Enemy")
            Enemies.Add(target);
        else
            Targets.Add(target);
    }

    private void Character_Shoot(Character cter, ProjectSO projectSO)
    {
        Vector2 direction = (cter.Target.position - cter.transform.position).normalized;
        var projectile = Factory.GetPrefab(projectSO);

        if (projectile != null)
        {
            projectile.Target = cter.Target.gameObject;
            projectile.Launch(cter.AttackPoint.transform, direction);
        }
    }


}

