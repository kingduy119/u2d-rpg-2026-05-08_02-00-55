using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;


namespace TDGame
{

    [RequireComponent(typeof(Grid))]
    public class WorldMap : MonoBehaviour
    {

        [SerializeField] private Grid m_grid;
        [SerializeField] private TileBase blockTile;
        [SerializeField] private TileBase activeTile;
        [SerializeField] private Tilemap blockedTilemap;
        [SerializeField] private Tilemap previewTilemap;
        [SerializeField] private Tilemap[] blockTilemaps;

        private InGameState InGameState;

        private readonly HashSet<Vector3Int> blockedCells = new();
        private readonly HashSet<Vector3Int> m_previewCells = new();
        private TowerBase m_selectedTower;
        private Vector3 m_wordPos;
        private bool CanBuild = false;

        public static event Action<bool> OnAcceptBuildResult;

        private void Awake()
        {
            m_grid = GetComponent<Grid>();

            blockedCells.Clear();

            foreach (Tilemap tilemap in blockTilemaps)
            {
                if (tilemap == null)
                    continue;

                CompressBlockedTile(tilemap);
            }
            blockedTilemap.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GameEvent.OnTowerSelected += HandleTowerSelect;

            TowerSelectUI.OnAcceptBuild += HandleAcceptBuild;
            TowerSelectUI.OnCancelBuild += HandleCancelBuild;
        }

        private void OnDisable()
        {
            GameEvent.OnTowerSelected -= HandleTowerSelect;

            TowerSelectUI.OnAcceptBuild -= HandleAcceptBuild;
            TowerSelectUI.OnCancelBuild -= HandleCancelBuild;
        }

        private void Start()
        {
            InGameState = GameManager.Instance.InGameState;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Camera main = Camera.main;
            if (!main) return;

            Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            m_wordPos = mousePos;

            if (Input.GetMouseButtonDown(0))
            {
                ShowTowerAndCellPreview();
            }
        }

        private void HandleTowerSelect(TowerSO data)
        {
            if (m_selectedTower != null)
            {
                m_selectedTower.Deactivate();
            }
            m_selectedTower = GameManager.Instance.FactoryManager.TowerFactory.GetObject(data.towerType);

            ShowTowerAndCellPreview();
        }

        private void ShowTowerAndCellPreview()
        {
            if (m_selectedTower == null) return;

            Vector3Int origin = m_grid.WorldToCell(m_wordPos);
            Vector2Int size = m_selectedTower.Size;

            Vector3 pos = m_grid.CellToWorld(origin);

            pos += new Vector3(
                size.x * m_grid.cellSize.x * 0.5f,
                size.y * m_grid.cellSize.y * 0.5f,
                0);

            m_selectedTower.transform.position = pos;

            HandleTowerSelecting(origin, size);
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

        private void HandleTowerSelecting(Vector3Int origin, Vector2Int size)
        {
            DisplayTilemapPreview();
            previewTilemap.ClearAllTiles();
            m_previewCells.Clear();

            bool isCanBuild = true;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int cell = origin + new Vector3Int(x, y, 0);
                    bool isBlocked = blockedCells.Contains(cell);
                    var tile = isBlocked ? blockTile : activeTile;

                    m_previewCells.Add(cell);
                    previewTilemap.SetTile(cell, tile);

                    if (isCanBuild && isBlocked) isCanBuild = false;
                }
            }

            CanBuild = isCanBuild;
        }

        private void HandleAcceptBuild()
        {
            if (CanBuild)
            {
                if (!InGameState.CheckAndSpendResource(m_selectedTower.TowerSO))
                {
                    // Debug.Log("Not Enough Gold");
                    return;
                }

                foreach (Vector3Int cell in m_previewCells)
                {
                    blockedCells.Add(cell);
                    blockedTilemap.SetTile(cell, blockTile);
                }
                m_selectedTower = null;
                HiddenTilemapPreview();
                // GameManager.Audio.PlayTowerPlacedSound();
            }
            else
            {
                // GameManager.Audio.PlayTowerCantBuild();
                Debug.Log("Cant Build Tower");
            }

            OnAcceptBuildResult?.Invoke(CanBuild);
        }

        private void HandleCancelBuild()
        {
            m_selectedTower.Deactivate();
            m_selectedTower = null;
            HiddenTilemapPreview();
        }

        private void DisplayTilemapPreview()
        {
            blockedTilemap.gameObject.SetActive(true);
            previewTilemap.gameObject.SetActive(true);
        }

        private void HiddenTilemapPreview()
        {
            blockedTilemap.gameObject.SetActive(false);
            previewTilemap.gameObject.SetActive(false);
        }
    }
}