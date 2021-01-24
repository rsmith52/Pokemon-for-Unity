using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mapping
{
    [CreateAssetMenu(menuName = "Tiles/Basic Tile", fileName = "Basic Tile")]
    public class BasicTile : AdvancedTileBase
    {
        #region Editor

        [Header("Basic Tile Settings")]
        public Sprite sprite;

        #endregion


        #region MonoBehavior

        public override void GetTileData(Vector3Int pos, ITilemap tilemap, ref TileData tile_data)
        {
            tile_data.sprite = Application.isPlaying ? null : sprite;
        }

        #endregion
    }
}