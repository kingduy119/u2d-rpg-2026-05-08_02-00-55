using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace TDGame
{

    public class WorldMap : MonoBehaviour
    {

        [SerializeField] private Grid grid;
        [SerializeField] private TileBase blockTile;
        [SerializeField] private TileBase activeTile;
        [SerializeField] private Tilemap blockedTilemap;
        [SerializeField] private Tilemap previewTilemap;

        [SerializeField] private Tilemap[] blockTilemaps;


        private readonly HashSet<Vector3Int> blockedCells = new();
        private HashSet<Vector3Int> m_previewCells = new();
        private bool m_canBuild = false;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            TowerSelectUI.OnTowerSelecting += HandleTowerSelecting;
            TowerSelectUI.OnTowerSelectAccepted += HandleTowerSelectAccepted;
            TowerSelectUI.OnTowerDeselected += HiddenPreview;
        }

        private void OnDisable()
        {
            TowerSelectUI.OnTowerSelecting -= HandleTowerSelecting;
            TowerSelectUI.OnTowerSelectAccepted -= HandleTowerSelectAccepted;
            TowerSelectUI.OnTowerDeselected -= HiddenPreview;
        }

        public void RegisterTower(Vector3Int origin, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    blockedCells.Add(origin + new Vector3Int(x, y, 0));
                }
            }
        }

        private void Initialize()
        {
            blockedCells.Clear();

            foreach (Tilemap tilemap in blockTilemaps)
            {
                if (tilemap == null)
                    continue;

                CompressBlockedTile(tilemap);
            }
            blockedTilemap.gameObject.SetActive(false);
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
            DisplayPreview();
            previewTilemap.ClearAllTiles();

            bool isCanBuild = true;
            m_previewCells.Clear();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int cell = origin + new Vector3Int(x, y, 0);
                    bool isBlocked = blockedCells.Contains(cell);

                    m_previewCells.Add(cell);
                    previewTilemap.SetTile(cell,
                    isBlocked ? blockTile : activeTile
                    );
                    if (isCanBuild && isBlocked) isCanBuild = false;
                }
            }

            m_canBuild = isCanBuild;
        }

        private void HandleTowerSelectAccepted()
        {
            if (m_canBuild)
            {
                foreach (Vector3Int cell in m_previewCells)
                {
                    blockedCells.Add(cell);
                    blockedTilemap.SetTile(cell, blockTile);
                }
            }
            HiddenPreview();
        }

        private void DisplayPreview()
        {
            blockedTilemap.gameObject.SetActive(true);
            previewTilemap.gameObject.SetActive(true);
        }

        private void HiddenPreview()
        {
            blockedTilemap.gameObject.SetActive(false);
            previewTilemap.gameObject.SetActive(false);
        }

    }
}