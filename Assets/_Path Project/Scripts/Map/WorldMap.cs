using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;


namespace TDGame
{

    [RequireComponent(typeof(Grid))]
    public class WorldMap : MonoBehaviour
    {

        [SerializeField] private Grid _Grid;
        [SerializeField] private TileBase blockTile;
        [SerializeField] private TileBase activeTile;

        [SerializeField] private Tilemap _blockedTilemap;
        [SerializeField] private Tilemap _previewTilemap;
        [SerializeField] private Tilemap[] _blockTilemaps;

        private readonly HashSet<Vector3Int> _blockedCells = new();
        private readonly HashSet<Vector3Int> _PreviewCells = new();

        public Vector3 WorldPosition { get; private set; }
        public Vector3 MousePosition { get; private set; }
        public bool CanBuild = false;

        public PointerStateMachine PointerStateMachine { get; private set; }

        private void Awake()
        {
            _Grid = GetComponent<Grid>();

            _blockedCells.Clear();

            foreach (Tilemap tilemap in _blockTilemaps)
            {
                if (tilemap == null)
                    continue;

                CompressBlockedTile(tilemap);
            }
            _blockedTilemap.gameObject.SetActive(false);

            PointerStateMachine = new(this);
        }

        private void OnEnable()
        {
            PointerStateMachine.Enable();
            TowerEvent.OnTowerPlace += HandleTowerPlace;
            // TowerEvent.OnAcceptBuild += HandleAcceptBuild;
        }

        private void OnDisable()
        {
            PointerStateMachine.Disable();
            TowerEvent.OnTowerPlace -= HandleTowerPlace;
            // TowerEvent.OnAcceptBuild -= HandleAcceptBuild;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject() || !Camera.main)
                return;

            UpdateWorldPosition();
            PointerStateMachine.Execute();
        }

        private void CompressBlockedTile(Tilemap tilemap)
        {
            tilemap.CompressBounds();
            foreach (Vector3Int cell in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(cell))
                    continue;

                _blockedCells.Add(cell);
                _blockedTilemap.SetTile(cell, blockTile);
            }
        }

        // private void ShowTowerAndCellPreview()
        // {
        //     if (m_selectedTower == null) return;

        //     Vector3Int origin = _Grid.WorldToCell(WorldPosition);
        //     Vector2Int size = m_selectedTower.TowerSO.Size;

        //     Vector3 pos = _Grid.CellToWorld(origin);

        //     pos += new Vector3(
        //         size.x * _Grid.cellSize.x * 0.5f,
        //         size.y * _Grid.cellSize.y * 0.5f,
        //         0);

        //     m_selectedTower.transform.position = pos;
        // }



        public void HandleTowerPlace(TowerBase tower)
        {
            Vector2Int size = tower.TowerSO.Size;
            Vector3Int origin = _Grid.WorldToCell(tower.transform.position);
            Vector3 position = _Grid.CellToWorld(origin);

            position += new Vector3(
                size.x * _Grid.cellSize.x * 0.5f,
                size.y * _Grid.cellSize.y * 0.5f,
                0);

            tower.transform.position = position;

            CheckAndDisplayTile(origin, size);
        }



        private void CheckAndDisplayTile(Vector3Int origin, Vector2Int size)
        {
            _PreviewCells.Clear();
            _previewTilemap.ClearAllTiles();
            DisplayTilemapPreview();

            bool isCanBuild = true;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int cell = origin + new Vector3Int(x, y, 0);
                    bool isBlocked = _blockedCells.Contains(cell);
                    var tile = isBlocked ? blockTile : activeTile;

                    _PreviewCells.Add(cell);
                    _previewTilemap.SetTile(cell, tile);

                    if (isCanBuild && isBlocked) isCanBuild = false;
                }
            }

            CanBuild = isCanBuild;
        }

        public void AcceptBuild()
        {
            foreach (Vector3Int cell in _PreviewCells)
            {
                _blockedCells.Add(cell);
                _blockedTilemap.SetTile(cell, blockTile);
            }
            CanBuild = false;
            _PreviewCells.Clear();
            _previewTilemap.ClearAllTiles();
        }



        public void DisplayTilemapPreview()
        {
            _blockedTilemap.gameObject.SetActive(true);
            _previewTilemap.gameObject.SetActive(true);
        }

        public void HiddenTilemapPreview()
        {
            _blockedTilemap.gameObject.SetActive(false);
            _previewTilemap.gameObject.SetActive(false);
        }

        public Collider2D GetColider(LayerMask layer)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                WorldPosition,
                Vector2.zero, Mathf.Infinity, layer);
            return hit.collider;
        }

        private void UpdateWorldPosition()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            WorldPosition = mousePosition;
        }
    }
}