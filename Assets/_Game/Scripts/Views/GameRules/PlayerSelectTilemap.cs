
using System.Collections.Generic;
using System.Linq;
using Characters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

[RequireComponent(typeof(Grid))]
public class PlayerSelectTilemapView : MonoBehaviour
{
    [SerializeField] private TileBase _blockTile;
    [SerializeField] private TileBase _activeTile;

    private Grid _TileGrid;
    [SerializeField] private Tilemap _activeTilemap;
    [SerializeField] private Tilemap _previewTilemap;
    [SerializeField] private Tilemap _enemyTilemap;



    public List<GameObject> _enemies;
    public Grid TileGrid => _TileGrid;

    public readonly HashSet<Vector3Int> _enemyCells = new();
    public readonly HashSet<Vector3Int> ActiveCells = new();
    private readonly Dictionary<Vector3, ITargetController> _map = new();

    Character _character;
    CharacterSO _selectedCterSO;
    ButtonView _prevButton;

    [Inject] readonly FactoryService _factory;


    void Awake()
    {
        _TileGrid = GetComponent<Grid>();

        foreach (var cell in _activeTilemap.cellBounds.allPositionsWithin)
        {
            if (_activeTilemap.HasTile(cell))
                ActiveCells.Add(cell);
        }
        foreach (var cell in _enemyTilemap.cellBounds.allPositionsWithin)
        {
            if (_enemyTilemap.HasTile(cell))
                _enemyCells.Add(cell);
        }
        HideTilemaps();
    }

    void Start()
    {
        InitEnemies();
    }

    // public void ChangeTilemapPosition(ITargetController target)
    // {
    //     var cell = _TileGrid.WorldToCell(target.GetPosition());
    //     if (ActiveCells.Contains(cell))
    //     {
    //         if (_map.TryGetValue(cell, out ITargetController other))
    //         {
    //             Vector3 prevPosition = target.GetPrevPosition();
    //             target.SetPosition(other.GetPosition());
    //             other.SetPosition(prevPosition);

    //             _map[cell] = target;
    //             _map[prevPosition] = other;
    //         }
    //         else
    //         {
    //             var prevCell = _TileGrid.WorldToCell(target.GetPrevPosition());

    //             _map[cell] = target;

    //             var position = CellToWorld(cell);
    //             target.SetPosition(position);
    //             _map.Remove(prevCell);
    //         }
    //     }
    //     else
    //     {
    //         target.BackToPosition();
    //     }

    //     HideTilemaps();
    // }

    public void ShowPreviewTile(Vector3 position)
    {
        var cell = _TileGrid.WorldToCell(position);
        if (!_previewTilemap.HasTile(cell))
        {
            _previewTilemap.ClearAllTiles();
            _previewTilemap.SetTile(cell, _blockTile);
        }

        ShowTilemaps();
    }

    // public void GamePlayCardSelect(ButtonView button, CharacterSO so)
    // {
    //     _selectedCterSO = so;
    //     if (_TileGrid == null) return;

    //     foreach (var cell in ActiveCells)
    //     {
    //         if (!_map.ContainsKey(cell))
    //         {
    //             InitCharacter(cell);
    //             return;
    //         }
    //     }

    //     if (_prevButton != null) _prevButton.UnSelected();
    //     _prevButton = button;
    // }

    // private void InitCharacter(Vector3Int cell)
    // {
    //     _character = _factory.GetPrefab(_selectedCterSO);
    //     if (_character == null) return;

    //     var position = CellToWorld(cell);

    //     var target = _character.AddComponent<TargetController>();
    //     target.Init(position, "Player", "Enemy");

    //     _map[cell] = target;
    //     GamePlayEvents.AddTarget?.Invoke("Player", target);
    // }

    public void InitEnemies()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            var cell = _enemyCells.ElementAt(i);
            var position = CellToWorld(cell);

            var target = _enemies[i].AddComponent<TargetController>();
            target.Init(position, "Enemy", "Player");

            _map[cell] = target;
            GamePlayEvents.AddTarget?.Invoke("Enemy", target);
        }
    }

    public Vector3 CellToWorld(Vector3Int cell) => CellToWorld(cell, new(1, 1));
    public Vector3 CellToWorld(Vector3Int cell, Vector2Int size)
    {
        Vector3 position = _TileGrid.CellToWorld(cell);
        position += new Vector3(
            size.x * _TileGrid.cellSize.x * 0.5f,
            size.y * _TileGrid.cellSize.y * 0.5f,
            0);
        return position;
    }
    public Vector3Int WorldToCell(Vector3 position) => _TileGrid.WorldToCell(position);

    public void ShowTilemaps()
    {
        _activeTilemap.gameObject.SetActive(true);
        _previewTilemap.gameObject.SetActive(true);
    }
    public void HideTilemaps()
    {
        _enemyTilemap.gameObject.SetActive(false);
        _activeTilemap.gameObject.SetActive(false);
        _previewTilemap.gameObject.SetActive(false);
    }

}
