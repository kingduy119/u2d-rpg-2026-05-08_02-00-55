
using System.Collections.Generic;
using Characters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using VContainer;

public class PlayerSelectTilemapView : MonoBehaviour
{
    [SerializeField] private TileBase _blockTile;
    [SerializeField] private TileBase _activeTile;

    [SerializeField] private Tilemap _activeTilemaps;
    [SerializeField] private Tilemap _previewTilemaps;

    [SerializeField] private Grid _Grid;

    // private readonly HashSet<Vector3Int> _blockedCells = new();
    private readonly HashSet<Vector3Int> _activeCells = new();

    private readonly Dictionary<Vector3, ITargetController> _map = new();

    Character _character;
    CharacterSO _selectedCterSO;
    ButtonView _prevButton;

    [Inject] readonly TestMouseEvent _mouseEvent;
    [Inject] readonly FactoryService _factory;


    void Awake()
    {
        _Grid = GetComponent<Grid>();

        foreach (var cell in _activeTilemaps.cellBounds.allPositionsWithin)
        {
            if (_activeTilemaps.HasTile(cell))
                _activeCells.Add(cell);
        }

        _activeTilemaps.gameObject.SetActive(false);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !Camera.main)
            return;

        if (_selectedCterSO != null)
        {
            // Debug.Log($"_mouseEvent: {_mouseEvent.MouseWorldPosition}");
        }
    }

    void OnEnable()
    {
        ButtonEvents.GamePlayCardSelect += GamePlayCardSelect;

        GameDefenseEvents.ChangeTilemapPosition += ChangeTilemapPosition;
    }
    void OnDisable()
    {
        ButtonEvents.GamePlayCardSelect -= GamePlayCardSelect;
        GameDefenseEvents.ChangeTilemapPosition -= ChangeTilemapPosition;
    }

    private void ChangeTilemapPosition(ITargetController target)
    {
        var cell = _Grid.WorldToCell(target.GetPosition());
        if (_activeCells.Contains(cell))
        {
            if (_map.TryGetValue(cell, out ITargetController other))
            {
                Vector3 prevPosition = target.GetPrevPosition();
                target.SetPosition(other.GetPosition());
                other.SetPosition(prevPosition);

                _map[cell] = target;
                _map[prevPosition] = other;
            }
            else
            {
                var prevCell = _Grid.WorldToCell(target.GetPrevPosition());

                _map[cell] = target;
                target.SetPosition(CellToWorld(cell));
                _map.Remove(prevCell);
            }
        }
        else
        {
            target.BackToPosition();
        }
    }

    private void GamePlayCardSelect(ButtonView button, CharacterSO so)
    {
        if (_prevButton != null) _prevButton.UnSelected();

        _selectedCterSO = so;
        _prevButton = button;

        CheckActiveCellAndInitCharacter();
    }

    private void CheckActiveCellAndInitCharacter()
    {
        foreach (var cell in _activeCells)
        {
            if (!_map.TryGetValue(cell, out ITargetController other))
            {
                InitCharacter(cell);
                return;
            }
        }
    }

    private void InitCharacter(Vector3Int cell)
    {
        _character = _factory.GetPrefab(_selectedCterSO);
        _character.AddComponent<TargetController>();
        if (_character.TryGetComponent<TargetController>(out var target))
        {
            var position = CellToWorld(cell);
            target.Init(position, "Player", "Enemy");
            _map[cell] = target;
        }
    }


    private Vector3 CellToWorld(Vector3Int cell) => CellToWorld(cell, new(1, 1));
    private Vector3 CellToWorld(Vector3Int cell, Vector2Int size)
    {
        Vector3 position = _Grid.CellToWorld(cell);
        position += new Vector3(
            size.x * _Grid.cellSize.x * 0.5f,
            size.y * _Grid.cellSize.y * 0.5f,
            0);
        return position;
    }



    // private void HandlePlace(Vector3 mousePosition)
    // {
    //     Vector3Int cellPosition = _Grid.WorldToCell(mousePosition);
    //     bool actived = _activeCells.Contains(cellPosition);
    //     bool blocked = _blockedCells.Contains(cellPosition);

    //     var tile = blocked ? _blockTile : _activeTile;
    // }

    // public Vector3 Show(Vector3 position, Vector2Int? goSize = null)
    // {
    //     Vector3Int origin = _Grid.WorldToCell(position);
    //     Vector2Int size = goSize ?? new Vector2Int(1, 1);
    //     Vector3 fixPosition = _Grid.CellToWorld(origin);

    //     fixPosition += new Vector3(
    //             size.x * _Grid.cellSize.x * 0.5f,
    //             size.y * _Grid.cellSize.y * 0.5f,
    //             0);
    //     return fixPosition;
    // }
}
