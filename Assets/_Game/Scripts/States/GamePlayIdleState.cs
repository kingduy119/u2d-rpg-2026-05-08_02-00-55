using Characters;
using UnityEngine;
using Unity.VisualScripting;

public class GamePlayIdleState : GameplayState
{
    TargetController _target;
    ButtonView _prevButton;

    public GamePlayIdleState(GamePlayController controller) : base(controller) { }

    public override void Enter()
    {
        ButtonEvents.GamePlayCardSelect += GamePlayCardSelect;

        Controller.UIView.IdleEnter();
        Controller.InitEnemies();
        Controller.SpawnPlayers();
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



    private void GamePlayCardSelect(ButtonView button, CharacterSO so)
    {
        foreach (var cell in Controller.Tilemap.ActiveCells)
        {
            if (!Controller.PlayerObjects.TryGetValue(cell, out var _))
            {

                if (Controller.Tilemap.TileGrid == null) return;
                var target = Controller.SpawnCharacter(so, cell);

                if (target != null)
                {
                    target.Init("Player", "Enemy");
                    Controller.PlayerObjects[cell] = target;
                    Controller.PlayerMapSO[cell] = so;

                    Controller.Targets[cell] = target.gameObject;
                }
                return;
            }
        }


        if (_prevButton != null) _prevButton.UnSelected();
        _prevButton = button;
    }
}