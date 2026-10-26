
using System.Collections.Generic;
using System.Linq;
using Characters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

// [RequireComponent(typeof(Grid))]
public class PlayerSelectTilemapView : MonoBehaviour
{
    [SerializeField] private TileBase _blockTile;
    [SerializeField] private TileBase _activeTile;

    [SerializeField] private Tilemap _activeTilemap;
    [SerializeField] private Tilemap _previewTilemap;
    [SerializeField] private Tilemap _enemyTilemap;

    [SerializeField] private Grid _Grid;

    public List<CharacterSO> _enemySOList;

    private readonly HashSet<Vector3Int> _enemyCells = new();
    private readonly HashSet<Vector3Int> _activeCells = new();
    private readonly Dictionary<Vector3, ITargetController> _map = new();

    Character _character;
    CharacterSO _selectedCterSO;
    ButtonView _prevButton;

    [Inject] readonly FactoryService _factory;


    void Awake()
    {
        foreach (var cell in _activeTilemap.cellBounds.allPositionsWithin)
        {
            if (_activeTilemap.HasTile(cell))
                _activeCells.Add(cell);
        }

        foreach (var cell in _enemyTilemap.cellBounds.allPositionsWithin)
        {
            if (_enemyTilemap.HasTile(cell))
                _enemyCells.Add(cell);
        }

        HideTilemaps();
    }

    public void ChangeTilemapPosition(ITargetController target)
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

        HideTilemaps();
    }

    public void ShowPreviewTile(Vector3 position)
    {
        var cell = _Grid.WorldToCell(position);
        if (!_previewTilemap.HasTile(cell))
        {
            _previewTilemap.ClearAllTiles();
            _previewTilemap.SetTile(cell, _blockTile);
        }

        ShowTilemaps();
    }

    public void GamePlayCardSelect(ButtonView button, CharacterSO so)
    {
        if (_prevButton != null) _prevButton.UnSelected();

        _selectedCterSO = so;
        _prevButton = button;

        foreach (var cell in _activeCells)
        {
            if (!_map.ContainsKey(cell))
            {
                InitCharacter(cell);
                return;
            }
        }
    }

    private void InitCharacter(Vector3Int cell)
    {
        _character = _factory.GetPrefab(_selectedCterSO);
        if (_character == null) return;

        _character.AddComponent<TargetController>();
        if (_character.TryGetComponent<TargetController>(out var target))
        {
            var position = CellToWorld(cell);
            target.Init(position, "Player", "Enemy");
            _map[cell] = target;

            GamePlayEvents.AddTarget?.Invoke(target);
        }
    }

    public void InitEnemies()
    {
        List<ITargetController> enemies = new();
        foreach (var so in _enemySOList)
        {
            _character = _factory.GetPrefab(so);
            if (_character == null) return;

            _character.AddComponent<TargetController>();
            if (_character.TryGetComponent<TargetController>(out var target))
            {
                enemies.Add(target);
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            var cell = _enemyCells.ElementAt(i);
            var position = CellToWorld(cell);
            var target = enemies[i];

            target.Init(position, "Enemy", "Player");
            _map[cell] = target;
            GamePlayEvents.AddTarget?.Invoke(target);
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

    private void ShowTilemaps()
    {
        _activeTilemap.gameObject.SetActive(true);
        _previewTilemap.gameObject.SetActive(true);
    }
    private void HideTilemaps()
    {
        _enemyTilemap.gameObject.SetActive(false);
        _activeTilemap.gameObject.SetActive(false);
        _previewTilemap.gameObject.SetActive(false);
    }

}
