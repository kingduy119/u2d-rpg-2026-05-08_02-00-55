using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace TDGame
{

    public class WorldMap : MonoBehaviour
    {

        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap buildTilemap;
        [SerializeField] private Tilemap previewTilemap;
        [SerializeField] private Tilemap obstacleTilemap;

        [SerializeField] private TileBase greenTile;
        [SerializeField] private TileBase redTile;

        // private Vector3Int lastCell;
        // private readonly HashSet<Vector3Int> occupiedCells = new();
        // private readonly HashSet<Vector3Int> obstacleCells = new();

        private readonly HashSet<Vector3Int> blockedCells = new();


        private void Awake()
        {
            CacheObstacleCells();
        }

        private void OnEnable()
        {
            TowerSelectUI.OnTowerSelecting += ShowPreview;
        }

        private void OnDisable()
        {
            TowerSelectUI.OnTowerSelecting -= ShowPreview;
        }

        private void CacheObstacleCells()
        {
            blockedCells.Clear();

            BoundsInt bounds = obstacleTilemap.cellBounds;

            foreach (Vector3Int cell in bounds.allPositionsWithin)
            {
                if (obstacleTilemap.HasTile(cell))
                {
                    blockedCells.Add(cell);
                }
            }
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

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int cell = origin + new Vector3Int(x, y, 0);

                    // bool hasObstacle = obstacleTilemap.HasTile(cell);
                    bool hasObstacle = blockedCells.Contains(cell);

                    previewTilemap.SetTile(cell,
                    hasObstacle ? redTile : greenTile
                    );
                }
            }
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