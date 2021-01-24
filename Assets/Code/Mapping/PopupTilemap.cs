using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mapping
{
    [RequireComponent(typeof(Tilemap))]
    public class PopupTilemap : MonoBehaviour
    {
        #region Monobehavior

        void Start()
        {
            Tilemap map = GetComponent<Tilemap>();

            foreach (Vector3Int pos in map.cellBounds.allPositionsWithin)
            {
                Vector3Int local_place = new Vector3Int(pos.x, pos.y, pos.z);

                if (map.HasTile(local_place))
                {
                    // Get PopupTile for this tile
                    PopupTile popup_tile = map.GetTile<PopupTile>(local_place);
                    if (!popup_tile)
                        continue;

                    // Create Helper tiles for this tile
                    HelperTile[] helper_tiles = popup_tile.helper_tiles;
                    for (int i = 0; i < helper_tiles.Length; i++)
                    {
                        BasicTile tile = helper_tiles[i].tile;
                        Vector2Int offsets = helper_tiles[i].offsets;

                        Vector3Int helper_tile_pos = new Vector3Int(pos.x + offsets.x, pos.y + offsets.y, pos.z);
                        map.SetTile(helper_tile_pos, tile);
                    }

                    // Set original tile to be transparent (not shown)
                    map.SetColor(pos, Color.clear);
                }
            }
        }

        #endregion
    }
}