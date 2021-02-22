using System.Collections.Generic;
using UnityEngine;

namespace Mapping
{
    #region Enums

    public enum Maps
    {

    }

    #endregion

    [CreateAssetMenu(fileName = "Map", menuName = "Pokemon/Map")]
    public class Map : ScriptableObject
    {
        #region Fields

        public Dictionary<Maps, Map> maps;

        #endregion


        #region Scriptable Object

        new public string name;
        public int map_id;
        public GameObject map;

        #endregion


        public static Map GetMapByID(int id)
        {
            // TODO: iterate through maps to get map with matching id

            return null;
        }

    }
}