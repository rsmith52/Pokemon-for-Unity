using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mapping
{
    #region Structs

    [Serializable]
    public struct HelperTile
    {
        public BasicTile tile;
        public Vector2Int offsets;
    }

    #endregion

    [CreateAssetMenu(menuName = "Tiles/Popup Tile", fileName = "Popup Tile")]
    public class PopupTile : AdvancedTileBase
    {
        #region Editor

        [Header("Popup Tile Settings")]
        public bool unparent;
        public Sprite preview_sprite;
        public GameObject prefab;
        public Vector2 prefab_offset = new Vector2(0.5f, 0.5f);
        public float prefab_local_z = 0;

        [Header("Helper Tiles")]
        public HelperTile[] helper_tiles;

        #endregion


        #region Monobehavior

        public override bool StartUp(Vector3Int pos, ITilemap tilemap, GameObject go)
        {

            //This prevents rogue prefab objects from appearing when the Tile palette is present
#if UNITY_EDITOR
            if (go != null)
            {
                if (go.scene.name == null)
                {
                    DestroyImmediate(go);
                }
            }
#endif

            if (go != null)
            {
                // Modify position of GO to match middle of Tile sprite
                go.transform.position = new Vector3(
                    pos.x + prefab_offset.x
                    , pos.y + prefab_offset.y
                    , pos.z);
                // Set proper Z Offset
                go.transform.localPosition = new Vector3(go.transform.localPosition.x,
                    go.transform.localPosition.y, prefab_local_z);
            }

            return true;
        }

        public override void GetTileData(Vector3Int pos, ITilemap tilemap, ref TileData tile_data)
        {
            tile_data.sprite = preview_sprite;

            if (prefab && tile_data.gameObject == null)
            {
                tile_data.gameObject = prefab;
            }
        }

        #endregion
    }
}