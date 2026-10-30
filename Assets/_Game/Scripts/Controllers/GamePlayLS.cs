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

    // public List<TargetController> Targets = new();
    // public List<TargetController> Enemies = new();
    // public List<TargetController> Players = new();
    public Dictionary<Vector3Int, TargetController> EnemyObjects = new();
    public Dictionary<Vector3Int, TargetController> PlayerObjects = new();
    public Dictionary<Vector3Int, CharacterSO> EnemyMapSO = new();
    public Dictionary<Vector3Int, CharacterSO> PlayerMapSO = new();

    public readonly StateMachine States = new();
    public IState IdleState;
    public IState StartState;

    public void Start()
    {
        IdleState = new GameIdleState(this);
        StartState = new GameStartState(this);
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
                if (States.CurrentState is not GameStartState)
                    States.TransitionTo(StartState);
                break;
            case "BackToIdle":
                if (States.CurrentState is not GameIdleState)
                    States.TransitionTo(IdleState);
                break;
            default:
                Debug.LogWarning($"Unknown event: {eventName}");
                break;
        }
    }

    public void InitEnemies()
    {
        foreach (var item in LevelView.GetWaveTargets())
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

        target.enabled = true;
        target.SetPosition(Tilemap.CellToWorld(cell));
        target.Cell = cell;
        return target;
    }

    public void ClearMap()
    {
        foreach (var entity in EnemyObjects.Values.ToList())
        {
            if (entity != null && entity.TryGetComponent<TargetController>(out var target))
            {
                target.Deactive();
            }
        }

        foreach (var entity in PlayerObjects.Values.ToList())
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

