using Characters;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GamePlayStartState : GameplayState
{
    int _enemies;
    int _players;

    List<TargetController> enemies => Controller.EnemyObjects.Values
        .Where(v => v != null)
        .ToList();
    List<TargetController> players => Controller.PlayerObjects.Values
        .Where(v => v != null)
        .ToList();

    public GamePlayStartState(GamePlayController controller) : base(controller) { }

    public override void Enter()
    {
        CterEvent.Dead += CterDead;

        _enemies = 0;
        _players = 0;

        // foreach (var target in enemies)
        // {
        //     target.SetTargets(players);
        //     target.enabled = true;
        // }
        // foreach (var target in players)
        // {
        //     target.SetTargets(enemies);
        //     target.enabled = true;
        // }
        foreach (var target in enemies.Concat(players))
        {
            if (target.gameObject.tag == "Enemy")
                target.SetTargets(players);
            else if (target.gameObject.tag == "Player")
                target.SetTargets(enemies);

            target.enabled = true;
        }
    }

    public override void Execute() { }

    public override void Exit()
    {
        CterEvent.Dead -= CterDead;
        Controller.ClearMap();
    }

    private void CterDead(GameObject obj)
    {

        var control = obj.GetComponent<TargetController>();
        if (obj.CompareTag("Enemy"))
        {

            _enemies++;
            if (control != null)
            {
                Controller.EnemyObjects.Remove(control.Cell);

                foreach (var target in players)
                    target.SetTargets(enemies);

                if (enemies.Count <= 0)
                {
                    if (Controller.LevelView.NextWave()) UIView.ToggleConfirm("Player Win!");
                    else SceneEvents.ChangeScene?.Invoke(SceneEvents.GameMenu);
                }
            }
        }
        else if (obj.CompareTag("Player"))
        {
            _players++;
            if (control != null)
            {
                Controller.PlayerObjects.Remove(control.Cell);

                foreach (var target in enemies)
                {
                    target.SetTargets(players);
                }

                if (_players >= players.Count)
                {
                    UIView.ToggleConfirm("Enemies Win!");
                }
            }

        }
    }
}
