using Characters;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

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

public class GameStartState : GameplayState
{
    int _enemies;
    int _players;

    List<TargetController> enemies => Controller.EnemyObjects.Values
        .Where(v => v != null)
        .ToList();
    List<TargetController> players => Controller.PlayerObjects.Values
        .Where(v => v != null)
        .ToList();

    public GameStartState(GamePlayController controller) : base(controller) { }

    public override void Enter()
    {
        CterEvent.Dead += CterDead;

        _enemies = 0;
        _players = 0;

        foreach (var target in enemies)
        {
            target.SetTargets(players);
            target.StartCombat();
        }
        foreach (var target in players)
        {
            target.SetTargets(enemies);
            target.StartCombat();
        }
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        CterEvent.Dead -= CterDead;
        Controller.ClearMap();
    }

    private void CterDead(GameObject obj)
    {

        // string name = LayerMask.LayerToName(obj.layer);
        var control = obj.GetComponent<TargetController>();

        if (obj.CompareTag("Enemy"))
        {

            _enemies++;
            foreach (var target in players)
            {
                target.SetTargets(enemies);
            }

            // if (_enemies >= enemies.Count)
            // {
            //     if (Controller.LevelView.NextWave()) UIView.ToggleConfirm("Player Win!");
            //     else SceneEvents.ChangeScene?.Invoke(SceneEvents.GameMenu);
            // }
        }
        else if (obj.CompareTag("Player"))
        {
            _players++;
            Controller.PlayerObjects.Remove(control.Cell);
            foreach (var target in enemies)
            {
                target.SetTargets(players);
            }

            // if (_players >= players.Count)
            // {
            //     UIView.ToggleConfirm("Enemies Win!");
            // }
        }

        Debug.Log($"players.Count: {players.Count} - {_players}");
        Debug.Log($"enemies.Count: {enemies.Count} - {_enemies}");
        // if (_players >= players.Count)
        // {
        //     UIView.ToggleConfirm("Enemies Win!");
        // }
        // else if (_enemies >= enemies.Count)
        // {
        //     if (Controller.LevelView.NextWave()) UIView.ToggleConfirm("Player Win!");
        //     else SceneEvents.ChangeScene?.Invoke(SceneEvents.GameMenu);
        // }

    }
}
