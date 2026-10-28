using Characters;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

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
    TargetController _target;
    GameObject _character;
    InputService Input => Controller.Input;

    public GameIdleState(GamePlayController controller) : base(controller) { }

    public override void Enter()
    {
        ButtonEvents.GamePlayCardSelect += GamePlayCardSelect;

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
        ButtonEvents.GamePlayCardSelect -= GamePlayCardSelect;

        Controller.UIView.IdleExit();
    }

    private void OnPointerDown()
    {
        if (_target == null)
        {
            Collider2D collider = Input.PointerCollider2D("Player");
            if (collider != null)
            {
                _target = collider.GetComponent<TargetController>();
            }
        }
    }

    private void OnPointerUp()
    {
        if (_target == null) return;

        var currentCell = Controller.Tilemap.WorldToCell(_target.GetPosition());
        var prevCell = _target.Cell;

        if (Controller.Tilemap.ActiveCells.Contains(currentCell) && currentCell != prevCell)
        {
            Controller.PlayerObjects.TryGetValue(currentCell, out var other);

            if (other != null)
            {
                other.SetPosition(Controller.Tilemap.CellToWorld(prevCell));
                other.Cell = prevCell;
            }
            _target.SetPosition(Controller.Tilemap.CellToWorld(currentCell));
            _target.Cell = currentCell;

            Controller.PlayerObjects[currentCell] = _target;
            Controller.PlayerObjects[prevCell] = other;

            Controller.PlayerMapSO.TryGetValue(currentCell, out var soCurrent);
            Controller.PlayerMapSO.TryGetValue(prevCell, out var soPrev);
            Controller.PlayerMapSO[currentCell] = soPrev;
            Controller.PlayerMapSO[prevCell] = soCurrent;
        }
        else _target.SetPosition(Controller.Tilemap.CellToWorld(prevCell));

        Controller.Tilemap.HideTilemaps();
        _target = null;
    }

    private void HandleTarget()
    {
        if (_target == null) return;

        _target.MoveByPointer(Input.MouseWorldPosition);
        Controller.Tilemap.ShowPreviewTile(_target.GetPosition());
    }


    private ButtonView _prevButton;
    private void GamePlayCardSelect(ButtonView button, CharacterSO so)
    {
        foreach (var cell in Controller.Tilemap.ActiveCells)
        {
            if (!Controller.PlayerObjects.TryGetValue(cell, out var _))
            {

                if (Controller.Tilemap.TileGrid == null) return;

                var character = Controller.Factory.GetPrefab(so);
                if (character == null) return;

                var target = character.AddComponent<TargetController>();

                target.Init("Player", "Enemy");
                target.Cell = cell;
                target.SetPosition(Controller.Tilemap.CellToWorld(cell));


                Controller.PlayerObjects[cell] = target;
                Controller.PlayerMapSO[cell] = so;
                return;
            }
        }


        if (_prevButton != null) _prevButton.UnSelected();
        _prevButton = button;
    }
}

public class GameStartState : GameplayState
{
    int _enemies;
    int _players;
    // List<TargetController> enemies;
    // List<TargetController> players;

    List<TargetController> enemies => Controller.Enemies;
    List<TargetController> players => Controller.PlayerObjects.Values
            .Where(v => v != null)
            .ToList();

    public GameStartState(GamePlayController controller) : base(controller) { }

    public override void Enter()
    {
        _enemies = 0;
        _players = 0;

        CterEvent.Dead += CterDead;

        // players = Controller.PlayerObjects.Values
        //     .Where(v => v != null)
        //     .ToList();
        // enemies = Controller.Enemies;

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
        // foreach (var target in Controller.Targets.Concat(Controller.Enemies))
        // {
        //     if (target != null)
        //     {
        //         target.Reset();
        //         // target.SetTargets([]);
        //     }
        // }

        CterEvent.Dead -= CterDead;
    }

    private void CterDead(GameObject obj)
    {

        string name = LayerMask.LayerToName(obj.layer);
        var control = obj.GetComponent<TargetController>();

        if (name == "Enemy")
        {

            _enemies++;
            if (_enemies >= enemies.Count)
            {
                Controller.UIView.ToggleConfirm("Players Win!");
                return;
            }

            foreach (var target in players)
            {
                target.SetTargets(enemies);
                target.StartCombat();
            }
        }
        else if (name == "Player")
        {
            _players++;
            Controller.PlayerObjects.Remove(control.Cell);

            if (_players >= players.Count)
            {
                Controller.UIView.ToggleConfirm("Enemies Win!");
                return;
            }

            Debug.Log($"Player Dead, Remaining: {players.Count}");
            foreach (var target in enemies)
            {
                target.SetTargets(players);
                target.StartCombat();
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
