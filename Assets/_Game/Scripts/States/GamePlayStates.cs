using Characters;
using UnityEngine;

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
        ButtonEvents.GamePlayEvents += GamePlayEvents;
        ButtonEvents.GamePlayCardSelect += Controller.Tilemap.GamePlayCardSelect;

        Controller.UIView.Show();
        Controller.Tilemap.InitEnemies();
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
        ButtonEvents.GamePlayEvents -= GamePlayEvents;
        ButtonEvents.GamePlayCardSelect -= Controller.Tilemap.GamePlayCardSelect;

        Controller.UIView.Hide();
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

    private void GamePlayEvents(string eventName)
    {
        switch (eventName)
        {
            case "StartGame":
                Controller.States.TransitionTo(Controller.StartState);
                break;
            default:
                Debug.LogWarning($"Unknown event: {eventName}");
                break;
        }
    }
}

public class GameStartState : GameplayState
{
    int _enemies;
    int _players;
    public GameStartState(GamePlayController controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("GameStartState.Enter");
        CterEvent.Dead += CterDead;

        foreach (var target in Controller.Targets) target.Start();
    }

    public override void Execute()
    {
        Debug.Log($"Controller.Targets: {Controller.Targets.Count}");
    }

    public override void Exit()
    {
        foreach (var target in Controller.Targets) target.Reset();

        CterEvent.Dead -= CterDead;
    }

    private void CterDead()
    {
        Debug.Log("CterDead");
    }
}