using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mapping
{
    public class AdvancedTileBase : TileBase
    {
        #region Editor

        [Header("Advanced Tile Settings")]
        // Movement
        public bool allow_passage = true;
        public bool up_passage = true;
        public bool left_passage = true;
        public bool right_passage = true;
        public bool down_passage = true;

        // Flags
        public bool is_bush = false;
        public bool is_counter = false;

        // Tile type
        public TerrainTags terrain_tag;

        #endregion
    }
}