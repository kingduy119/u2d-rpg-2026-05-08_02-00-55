using UnityEngine;
using UnityEngine.Tilemaps;

namespace TDGame
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;

        private void Start()
        {
            BoundsInt bounds = tilemap.cellBounds;

            foreach (Vector3Int cellPos in bounds.allPositionsWithin)
            {
                TileBase tile = tilemap.GetTile(cellPos);

                if (tile != null)
                {
                    // Debug.Log($"Cell: {cellPos.x}, {cellPos.y}");
                }
            }
        }
    }

}