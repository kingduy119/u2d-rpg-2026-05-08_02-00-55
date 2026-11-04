using Characters;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GamePlayStartState : GameplayState
{
    List<TargetController> Enemies => Controller.EnemyObjects.Values
        .Where(v => v != null)
        .ToList();
    List<TargetController> Players => Controller.PlayerObjects.Values
        .Where(v => v != null)
        .ToList();

    bool _stoped;

    public GamePlayStartState(GamePlayController controller) : base(controller) { }

    public override void Enter()
    {
        CterEvent.Dead += CterDead;

        foreach (var target in Enemies.Concat(Players))
        {
            if (target.gameObject.CompareTag("Enemy"))
                target.SetTargets(Players);
            else if (target.gameObject.CompareTag("Player"))
                target.SetTargets(Enemies);

            target.enabled = true;
        }

        _stoped = false;
    }

    // public override void Execute() { }

    public override void Exit()
    {
        CterEvent.Dead -= CterDead;
        Controller.ClearMap();
    }

    private void CterDead(GameObject obj)
    {
        if (_stoped) return;

        var control = obj.GetComponent<TargetController>();
        if (obj.CompareTag("Enemy"))
        {
            if (control != null)
            {
                Controller.EnemyObjects.Remove(control.Cell);
                foreach (var target in Players)
                    target.SetTargets(Enemies);

                if (Enemies.Count <= 0)
                {
                    UIView.ShowConfirm("Player Win!");
                    if (Controller.EnemyHealths.Count > 0)
                    {
                        var item = Controller.EnemyHealths[0];
                        Controller.EnemyHealths.RemoveAt(0);
                        item.SetActive(false);
                    }
                    _stoped = true;
                }
                // {
                // if (Controller.LevelView.NextWave()) 
                // else SceneEvents.ChangeScene?.Invoke(SceneEvents.GameMenu);
                // }
            }
        }
        else if (obj.CompareTag("Player"))
        {
            if (control != null)
            {
                Controller.PlayerObjects.Remove(control.Cell);
                foreach (var target in Enemies)
                {
                    target.SetTargets(Players);
                }

                if (Players.Count <= 0)
                {
                    UIView.ShowConfirm("Enemies Win!");
                    if (Controller.PlayerHealths.Count > 0)
                    {
                        var item = Controller.PlayerHealths[0];
                        Controller.PlayerHealths.RemoveAt(0);
                        item.SetActive(false);
                    }
                    _stoped = true;
                }
            }
        }
    }
}
