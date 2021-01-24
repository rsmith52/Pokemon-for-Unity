using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mapping
{
    [RequireComponent(typeof(Tilemap))]
    public class PrefabTilemap : MonoBehaviour
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
                    // Get PrefabTile for this tile
                    PrefabTile prefab_tile = map.GetTile<PrefabTile>(local_place);
                    if (!prefab_tile)
                        continue;

                    // Instantiate prefab game object
                    prefab_tile.InstantiatePrefab(map, local_place);
                }
            }
        }

        #endregion
    }
}