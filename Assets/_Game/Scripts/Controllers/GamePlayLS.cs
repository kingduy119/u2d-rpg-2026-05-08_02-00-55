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
        builder.RegisterComponentInHierarchy<GamePlayResourceView>();
    }
}

public class GamePlayController : IStartable, IDisposable, ITickable
{
    [Inject] readonly public FactoryService Factory;
    [Inject] readonly public ProfileSerice Profile;
    [Inject] readonly public InputService Input;
    [Inject] readonly public GamePlayTilemapView Tilemap;
    [Inject] readonly public GamePlayUIView UIView;
    [Inject] readonly public GameLevelView LevelView;
    [Inject] readonly public GamePlayResourceView ResourceView;

    public Dictionary<Vector3Int, TargetController> EnemyObjects = new();
    public Dictionary<Vector3Int, TargetController> PlayerObjects = new();
    public Dictionary<Vector3Int, CharacterSO> EnemyMapSO = new();
    public Dictionary<Vector3Int, CharacterSO> PlayerMapSO = new();

    public readonly StateMachine States = new();
    public IState IdleState;
    public IState StartState;

    public List<GameObject> PlayerHealths;
    public List<GameObject> EnemyHealths;

    public void Start()
    {

        IdleState = new GamePlayIdleState(this);
        StartState = new GamePlayStartState(this);
        States.Initialize(IdleState);

        CterEvent.Shoot += Character_Shoot;
        ButtonEvents.OnClick += ButtonEvents_OnClick;

        if (LevelView != null)
        {
            Profile.gResources.Reset(LevelView.Level.Resources);
        }

        PlayerHealths = UIView.PlayerHealth.GetItems();
        EnemyHealths = UIView.EnemyHealth.GetItems();
    }

    public void Tick() => States.Execute();

    public void Dispose()
    {
        CterEvent.Shoot -= Character_Shoot;
        ButtonEvents.OnClick -= ButtonEvents_OnClick;

        Factory.ClearPool();
    }

    private void ButtonEvents_OnClick(string eventName)
    {
        switch (eventName)
        {
            case "Idle_Start":
                if (States.CurrentState is not GamePlayStartState)
                    States.TransitionTo(StartState);
                break;
            case "Play_Confirm":
                HandleConfirmClick();
                break;
            default:
                Debug.LogWarning($"Unknown event: {eventName}");
                break;
        }
    }

    private void HandleConfirmClick()
    {
        if (LevelView.NextWave())
        {
            if (States.CurrentState is not GamePlayIdleState)
                States.TransitionTo(IdleState);
        }
        else SceneEvents.ChangeScene?.Invoke(SceneEvents.GameMenu);
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

    public void CterCardSelect(ButtonView button, CharacterSO cterSO, Resources resources)
    {
        if (!Profile.gResources.Enough(resources)) return;

        foreach (var cell in Tilemap.ActiveCells)
        {
            if (!PlayerObjects.TryGetValue(cell, out var _))
            {

                if (Tilemap.TileGrid == null) return;
                var target = SpawnCharacter(cterSO, cell);

                if (target != null)
                {
                    target.Init("Player", "Enemy");
                    PlayerObjects[cell] = target;
                    PlayerMapSO[cell] = cterSO;

                    Profile.gResources.Subtract(resources);
                    button.Hidden();
                }
                return;
            }
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