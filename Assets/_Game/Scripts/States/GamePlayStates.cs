using Characters;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class GameplayState : State
{
    protected readonly GamePlayController Controller;
    public GameplayState(GamePlayController controller)
    {
        Controller = controller;
    }
}

public class GameIdleState : GameplayState
{
    ITargetController _target;
    InputService Input => Controller.Input;

    public GameIdleState(GamePlayController controller) : base(controller) { }

    public override void Enter()
    {
        // ButtonEvents.GamePlayEvents += GamePlayEvents;
        ButtonEvents.GamePlayCardSelect += Controller.Tilemap.GamePlayCardSelect;

        Controller.UIView.IdleEnter();
    }

    public override void Execute()
    {
        if (Input.MouseOver) return;
        if (Input.IsMouseDown) OnPointerDown();
        else if (Input.IsMouseUp) OnPointerUp();

        HandleTarget();
    }

    public override void Exit()
    {
        // ButtonEvents.GamePlayEvents -= GamePlayEvents;
        ButtonEvents.GamePlayCardSelect -= Controller.Tilemap.GamePlayCardSelect;

        Controller.UIView.IdleExit();
    }

    private void OnPointerDown()
    {
        if (_target == null)
        {
            Collider2D collider = Input.PointerCollider2D("Player");
            if (collider != null)
            {
                var targets = collider.GetComponents<ITargetController>();
                if (targets.Length > 0) _target = targets[0];
            }
        }
    }

    private void OnPointerUp()
    {
        if (_target != null)
        {
            Controller.Tilemap.ChangeTilemapPosition(_target);
            _target = null;
        }
    }

    private void HandleTarget()
    {
        if (_target == null) return;

        _target.MoveByPointer(Input.MouseWorldPosition);
        Controller.Tilemap.ShowPreviewTile(_target.GetPosition());
    }

    // private void GamePlayEvents(string eventName)
    // {
    //     switch (eventName)
    //     {
    //         case "StartGame":
    //             Controller.States.TransitionTo(Controller.StartState);
    //             break;
    //         default:
    //             Debug.LogWarning($"Unknown event: {eventName}");
    //             break;
    //     }
    // }
}

public class GameStartState : GameplayState
{
    int _enemies;
    int _players;
    public GameStartState(GamePlayController controller) : base(controller) { }

    public override void Enter()
    {
        CterEvent.Dead += CterDead;

        _enemies = Controller.Enemies.Count;
        _players = Controller.Targets.Count;

        foreach (var target in Controller.Targets)
        {
            target.SetTargets(Controller.Enemies);
            target.StartCombat();
        }
        foreach (var target in Controller.Enemies)
        {
            target.SetTargets(Controller.Targets);
            target.StartCombat();
        }
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        foreach (var target in Controller.Targets.Concat(Controller.Enemies))
        {
            target.Reset();
        }

        CterEvent.Dead -= CterDead;
    }

    private void CterDead(string name)
    {
        if (name == "Enemy")
        {
            var transforms = GetTargetsByLayer("Enemy");
            Debug.Log($"Enemy Dead, Remaining: {transforms.Count}");
            foreach (var target in Controller.Targets)
            {
                target.SetTargets(transforms);
            }

            if (transforms.Count <= 0)
            {
                Controller.UIView.ToggleConfirm("Players Win!");
            }
        }
        else if (name == "Player")
        {
            var transforms = GetTargetsByLayer("Player");
            Debug.Log($"Player Dead, Remaining: {transforms.Count}");
            foreach (var target in Controller.Targets)
            {
                target.SetTargets(transforms);
            }

            if (transforms.Count <= 0)
            {
                Controller.UIView.ToggleConfirm("Enemies Win!");
            }
        }
    }

    private List<Transform> GetTargetsByLayer(string layer)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            Vector2.zero,
            10f,
            LayerMask.GetMask(layer));

        List<Transform> transforms = colliders
        .Where(c => c != null)
        .Select(c => c.transform)
        .ToList();

        return transforms;
    }
}
