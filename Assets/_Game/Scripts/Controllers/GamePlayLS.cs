using VContainer;
using VContainer.Unity;
using UnityEngine;
using Characters;
using System;
using Weapons;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;


public class GamePlayLS : LifetimeScope
{
    [SerializeField] ControlsSettings _controlsSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GamePlayController>(Lifetime.Singleton);
        builder.RegisterEntryPoint<InputService>(Lifetime.Singleton).AsSelf();


        builder.RegisterInstance<IControlsSettings>(_controlsSettings);

        builder.Register<IKeyBoardHandler, KeyBoardHandler>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<GamePlayTilemapView>();
        builder.RegisterComponentInHierarchy<GamePlayUIView>();
        builder.RegisterComponentInHierarchy<GameLevelView>();
    }
}

public class GamePlayController : IStartable, IDisposable, ITickable
{
    [Inject] readonly public FactoryService Factory;
    [Inject] readonly public InputService Input;
    [Inject] readonly public GamePlayTilemapView Tilemap;
    [Inject] readonly public GamePlayUIView UIView;
    [Inject] readonly public GameLevelView LevelView;

    public Dictionary<Vector3Int, GameObject> Targets = new();
    public Dictionary<Vector3Int, TargetController> EnemyObjects = new();
    public Dictionary<Vector3Int, TargetController> PlayerObjects = new();
    public Dictionary<Vector3Int, CharacterSO> EnemyMapSO = new();
    public Dictionary<Vector3Int, CharacterSO> PlayerMapSO = new();

    public readonly StateMachine States = new();
    public IState IdleState;
    public IState StartState;

    public void Start()
    {
        IdleState = new GamePlayIdleState(this);
        StartState = new GamePlayStartState(this);
        States.Initialize(IdleState);

        CterEvent.Shoot += Character_Shoot;
        ButtonEvents.Gameplay += HandleGamePlayEvents;
    }

    public void Tick()
    {
        States.Execute();
    }

    public void Dispose()
    {
        CterEvent.Shoot -= Character_Shoot;
        ButtonEvents.Gameplay -= HandleGamePlayEvents;

        Factory.ClearPool();
    }

    private void HandleGamePlayEvents(string eventName)
    {
        switch (eventName)
        {
            case "StartGame":
                if (States.CurrentState is not GamePlayStartState)
                    States.TransitionTo(StartState);
                break;
            case "BackToIdle":
                if (States.CurrentState is not GamePlayIdleState)
                    States.TransitionTo(IdleState);
                break;
            default:
                Debug.LogWarning($"Unknown event: {eventName}");
                break;
        }
    }

    public void SpawnEnemies()
    {
        foreach (var item in LevelView.Wave)
        {
            if (item.position < Tilemap.EnemyCells.Count)
            {
                var cell = Tilemap.EnemyCells.ElementAt(item.position);
                var so = item.character;

                var target = SpawnCharacter(so, cell);
                if (target != null)
                {
                    target.Init("Enemy", "Player");
                    EnemyObjects[cell] = target;

                    Targets[cell] = target.gameObject;
                }
            }
        }
    }

    public void SpawnPlayers()
    {
        foreach (var pair in PlayerMapSO)
        {
            var cell = pair.Key;
            var so = pair.Value;
            var target = SpawnCharacter(so, cell);

            if (target != null)
            {
                target.Init("Player", "Enemy");
                PlayerObjects[cell] = target;

                Targets[cell] = target.gameObject;
            }
        }
    }

    public TargetController SpawnCharacter(CharacterSO so, Vector3Int cell)
    {
        var character = Factory.GetPrefab(so);
        if (character == null) return null;

        if (!character.TryGetComponent<TargetController>(out var target))
        {
            target = character.AddComponent<TargetController>();
        }

        target.SetPosition(Tilemap.CellToWorld(cell));
        target.Cell = cell;
        target.enabled = false;
        return target;
    }

    public void ClearMap()
    {
        foreach (var entity in EnemyObjects.Values.ToList().Concat(PlayerObjects.Values.ToList()))
        {
            if (entity != null && entity.TryGetComponent<TargetController>(out var target))
            {
                target.Deactive();
            }
        }

        EnemyObjects.Clear();
        PlayerObjects.Clear();
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

public abstract class GameplayState : State
{
    protected readonly GamePlayController Controller;
    protected InputService Input => Controller.Input;
    protected GamePlayTilemapView TilemapView => Controller.Tilemap;
    protected GamePlayUIView UIView => Controller.UIView;

    public GameplayState(GamePlayController controller)
    {
        Controller = controller;
    }
}