using System.Collections.Generic;
using UnityEngine;

namespace Mapping
{
    #region Enums

    public enum Maps
    {
        TestMap
    }

    #endregion

    public class Map : MonoBehaviour
    {
        #region Fields

        public static Dictionary<Maps, Map> maps;

        #endregion


        #region Static Methods

        public static Map GetMapByID(int id)
        {
            return maps[(Maps)id];
        }

        #endregion

    }
}