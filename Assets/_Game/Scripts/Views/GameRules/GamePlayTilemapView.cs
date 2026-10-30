
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GamePlayTilemapView : MonoBehaviour
{
    [SerializeField] private TileBase _blockTile;
    [SerializeField] private TileBase _activeTile;

    private Grid _TileGrid;
    [SerializeField] private Tilemap _activeTilemap;
    [SerializeField] private Tilemap _previewTilemap;
    [SerializeField] private Tilemap _enemyTilemap;



    public List<GameObject> _enemies;
    public Grid TileGrid => _TileGrid;

    public readonly HashSet<Vector3Int> EnemyCells = new();
    public readonly HashSet<Vector3Int> ActiveCells = new();
    private readonly Dictionary<Vector3, ITargetController> _map = new();

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
                EnemyCells.Add(cell);
        }
        HideTilemaps();
    }

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
