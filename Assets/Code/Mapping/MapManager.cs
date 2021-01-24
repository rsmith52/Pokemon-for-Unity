using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Eventing;

namespace Mapping
{
    public class MapManager : MonoBehaviour
    {
        #region Fields

        public List<Tilemap> tilemaps;
        public Map current_map;

        private Grid map_grid;

        #endregion


        #region Relations

        public GameObject prefab_grass_rustle;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            Tilemap[] all_tilemaps = FindObjectsOfType<Tilemap>();
            foreach (Tilemap t in all_tilemaps)
                tilemaps.Add(t);
            map_grid = FindObjectOfType<Grid>();
        }

        #endregion


        #region Map Awareness

        public Tilemap[] GetNeighborTilemaps(Vector3 pos)
        {
            Tilemap ground = null;
            Tilemap level_up = null;
            Tilemap objects = null;
            Tilemap level_up_objects = null;

            for (int i = 0; i < tilemaps.Count; i++)
            {
                if (ground == null && tilemaps[i].transform.position.z == pos.z)
                    ground = tilemaps[i];
                if (level_up == null && tilemaps[i].transform.position.z == pos.z - 0.5f)
                    level_up = tilemaps[i];
            }

            objects = ground.GetComponentsInChildren<Tilemap>()[1];
            if (level_up != null)
                level_up_objects = level_up.GetComponentsInChildren<Tilemap>()[1];

            return new Tilemap[] { ground, objects, level_up, level_up_objects };
        }

        public AdvancedTileBase[] GetNeighborTiles(MoveableCharacter character)
        {
            Tilemap[] maps = GetNeighborTilemaps(character.transform.position);
            Tilemap ground = maps[0];
            Tilemap objects = maps[1];
            Tilemap level_up = maps[2];
            Tilemap level_up_objects = maps[3];

            Vector3Int pos = new Vector3Int(
                (int)character.transform.position.x,
                (int)character.transform.position.y - 1, 0);

            TileBase on_tile = null;
            TileBase up_tile = null;
            TileBase left_tile = null;
            TileBase right_tile = null;
            TileBase down_tile = null;

            // Current Tile
            if (level_up != null)
            {
                on_tile = level_up_objects.GetTile(pos);
                if (on_tile == null)
                    on_tile = level_up.GetTile(pos);
            }
            if (on_tile == null)
                on_tile = objects.GetTile(pos);
            if (on_tile == null)
                on_tile = ground.GetTile(pos);

            // Up Tile
            if (level_up != null)
            {
                up_tile = level_up_objects.GetTile(pos + Vector3Int.up);
                if (up_tile == null)
                    up_tile = level_up.GetTile(pos + Vector3Int.up);
            }
            if (up_tile == null)
                up_tile = objects.GetTile(pos + Vector3Int.up);
            if (up_tile == null)
                up_tile = ground.GetTile(pos + Vector3Int.up);

            // Left Tile
            if (level_up != null)
            {
                left_tile = level_up_objects.GetTile(pos + Vector3Int.left);
                if (left_tile == null)
                    left_tile = level_up.GetTile(pos + Vector3Int.left);
            }
            if (left_tile == null)
                left_tile = objects.GetTile(pos + Vector3Int.left);
            if (left_tile == null)
                left_tile = ground.GetTile(pos + Vector3Int.left);

            // Right Tile
            if (level_up != null)
            {
                right_tile = level_up_objects.GetTile(pos + Vector3Int.right);
                if (right_tile == null)
                    right_tile = level_up.GetTile(pos + Vector3Int.right);
            }
            if (right_tile == null)
                right_tile = objects.GetTile(pos + Vector3Int.right);
            if (right_tile == null)
                right_tile = ground.GetTile(pos + Vector3Int.right);

            // Down Tile
            if (level_up != null)
            {
                down_tile = level_up_objects.GetTile(pos + Vector3Int.down);
                if (down_tile == null)
                    down_tile = level_up.GetTile(pos + Vector3Int.down);
            }
            if (down_tile == null)
                down_tile = objects.GetTile(pos + Vector3Int.down);
            if (down_tile == null)
                down_tile = ground.GetTile(pos + Vector3Int.down);

            return new AdvancedTileBase[] { (AdvancedTileBase)on_tile, (AdvancedTileBase)up_tile,
                (AdvancedTileBase)left_tile, (AdvancedTileBase)right_tile, (AdvancedTileBase)down_tile };
        }

        #endregion


        #region Map Effects

        public IEnumerator GrassRustle(Vector3 pos)
        {
            GameObject grass_rustle = GameObject.Instantiate(prefab_grass_rustle, map_grid.transform);
            grass_rustle.transform.position += pos;

            yield return new WaitForSeconds(0.5f);

            GameObject.Destroy(grass_rustle.gameObject);
        }

        #endregion

    }
}