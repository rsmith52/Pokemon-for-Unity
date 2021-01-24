using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mapping
{
    [CreateAssetMenu(menuName = "Tiles/Prefab Tile", fileName = "Prefab Tile")]
    public class PrefabTile : AdvancedTileBase
    {

        #region Editor

        [Header("Prefab Tile Settings")]
        public bool unparent;
        public Sprite preview_sprite;
        public GameObject prefab;
        public Vector2 prefab_offset = new Vector2(0.5f, 0.5f);
        public float prefab_local_z = 0;

        #endregion


        #region Monobehavior

        public override bool StartUp(Vector3Int pos, ITilemap tilemap, GameObject go)
        {
            return false;
        }

        public override void GetTileData(Vector3Int pos, ITilemap tilemap, ref TileData tile_data)
        {
            tile_data.sprite = Application.isPlaying ? null : preview_sprite;
        }

        public void InstantiatePrefab(Tilemap map, Vector3Int pos)
        {
            if (map.gameObject.layer == 31) return;
            if (!Application.isPlaying) return;


            if (prefab)
            {
                GameObject instance = Instantiate(prefab);
                instance.transform.SetParent(map.transform);

                // Modify position of object to match middle of tile sprite
                instance.transform.position = new Vector3(
                    pos.x + prefab_offset.x,
                    pos.y + prefab_offset.y,
                    pos.z);
                // Set proper Z Offset
                instance.transform.localPosition = new Vector3(instance.transform.localPosition.x,
                    instance.transform.localPosition.y, prefab_local_z);

            }

            // Set original tile to be transparent (not shown)
            map.SetColor(pos, Color.clear);
        }

        #endregion
    }
}