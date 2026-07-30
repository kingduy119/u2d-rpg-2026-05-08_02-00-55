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

        [SerializeField] private Tilemap blockedTilemap;
        [SerializeField] private Tilemap previewTilemap;
        [SerializeField] private Tilemap[] blockTilemaps;

        private readonly HashSet<Vector3Int> blockedCells = new();
        private readonly HashSet<Vector3Int> _PreviewCells = new();

        public Vector3 WorldPosition { get; private set; }
        public Vector3 MousePosition { get; private set; }
        public bool CanBuild = false;

        public PointerStateMachine PointerStateMachine { get; private set; }

        private void Awake()
        {
            _Grid = GetComponent<Grid>();

            blockedCells.Clear();

            foreach (Tilemap tilemap in blockTilemaps)
            {
                if (tilemap == null)
                    continue;

                CompressBlockedTile(tilemap);
            }
            blockedTilemap.gameObject.SetActive(false);

            PointerStateMachine = new(this);
        }

        private void OnEnable()
        {
            PointerStateMachine.Enable();
            TowerEvent.OnTowerPlace += HandleTowerPlace;
        }

        private void OnDisable()
        {
            PointerStateMachine.Disable();
            TowerEvent.OnTowerPlace -= HandleTowerPlace;
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

                blockedCells.Add(cell);
                blockedTilemap.SetTile(cell, blockTile);
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
            DisplayTilemapPreview();
            previewTilemap.ClearAllTiles();
            _PreviewCells.Clear();

            bool isCanBuild = true;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int cell = origin + new Vector3Int(x, y, 0);
                    bool isBlocked = blockedCells.Contains(cell);
                    var tile = isBlocked ? blockTile : activeTile;

                    _PreviewCells.Add(cell);
                    previewTilemap.SetTile(cell, tile);

                    if (isCanBuild && isBlocked) isCanBuild = false;
                }
            }

            CanBuild = isCanBuild;
        }



        public void DisplayTilemapPreview()
        {
            blockedTilemap.gameObject.SetActive(true);
            previewTilemap.gameObject.SetActive(true);
        }

        public void HiddenTilemapPreview()
        {
            blockedTilemap.gameObject.SetActive(false);
            previewTilemap.gameObject.SetActive(false);
        }

        public Collider2D GetColider(LayerMask layer)
        {
            Vector3 mousePosition = GetMouseToWorldPoint();
            RaycastHit2D hit = Physics2D.Raycast(
                mousePosition,
                Vector2.zero, Mathf.Infinity, layer);
            return hit.collider;
        }

        private Vector3 GetMouseToWorldPoint()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            return mousePosition;
        }

        private void UpdateWorldPosition()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            WorldPosition = mousePosition;
        }
    }
}