using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace TDGame
{

    public class WorldMap : MonoBehaviour
    {

        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap obstacleTilemap;
        [SerializeField] private Tilemap buildTilemap;
        [SerializeField] private Tilemap previewTilemap;

        [SerializeField] private TileBase greenTile;
        [SerializeField] private TileBase redTile;

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
            TowerSelectUI.OnTowerDeselected += HandleTowerDeselected;
        }

        private void OnDisable()
        {
            TowerSelectUI.OnTowerSelecting -= HandleTowerSelecting;
            TowerSelectUI.OnTowerSelectAccepted -= HandleTowerSelectAccepted;
            TowerSelectUI.OnTowerDeselected -= HandleTowerDeselected;
        }

        private void Initialize()
        {
            blockedCells.Clear();
            BoundsInt bounds = obstacleTilemap.cellBounds;
            foreach (Vector3Int cell in bounds.allPositionsWithin)
            {
                if (obstacleTilemap.HasTile(cell))
                {
                    blockedCells.Add(cell);
                    buildTilemap.SetTile(cell, redTile);
                }
            }
            buildTilemap.gameObject.SetActive(false);
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

        public void ShowPreview(Vector3Int origin, Vector2Int size)
        {
            previewTilemap.ClearAllTiles();

            foreach (Vector3Int cell in blockedCells)
            {
                previewTilemap.SetTile(cell, redTile);
            }

            bool canBuild = true;
            HashSet<Vector3Int> previewCells = new();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int cell = origin + new Vector3Int(x, y, 0);

                    bool hasObstacle = blockedCells.Contains(cell);

                    previewTilemap.SetTile(cell,
                    hasObstacle ? redTile : greenTile
                    );

                    if (hasObstacle && canBuild)
                        canBuild = false;
                }
            }

            if (canBuild)
            {
                m_previewCells = previewCells;
                m_canBuild = true;
            }
        }

        private void HandleTowerSelecting(Vector3Int origin, Vector2Int size)
        {
            buildTilemap.gameObject.SetActive(true);
            previewTilemap.gameObject.SetActive(true);
            previewTilemap.ClearAllTiles();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int cell = origin + new Vector3Int(x, y, 0);
                    bool hasObstacle = blockedCells.Contains(cell);

                    previewTilemap.SetTile(cell,
                    hasObstacle ? redTile : greenTile
                    );
                }
            }
        }

        private void HandleTowerSelectAccepted()
        {
            if (!m_canBuild)
            {
                foreach (var cell in m_previewCells)
                {
                    blockedCells.Add(cell);
                    buildTilemap.SetTile(cell, redTile);
                }
            }
            buildTilemap.gameObject.SetActive(false);
            previewTilemap.gameObject.SetActive(false);
        }

        private void HandleTowerDeselected()
        {
            buildTilemap.gameObject.SetActive(false);
            previewTilemap.gameObject.SetActive(false);
        }


    }
}


// public void UnregisterTower(Vector3Int origin, Vector2Int size)
// {
//     for (int x = 0; x < size.x; x++)
//     {
//         for (int y = 0; y < size.y; y++)
//         {
//             blockedCells.Remove(origin + new Vector3Int(x, y, 0));
//         }
//     }
// }


// if (worldMap.CanBuild(cell))
// {
//     worldMap.RegisterTower(cell, towerSO.Footprint);
// }