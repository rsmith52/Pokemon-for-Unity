using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    #region Enums

    public enum Types
    {
        None,
        Normal,
        Fighting,
        Flying,
        Poison,
        Ground,
        Rock,
        Bug,
        Ghost,
        Steel,
        Fire,
        Water,
        Grass,
        Electric,
        Psychic,
        Ice,
        Dragon,
        Dark,
        // Generation 6
        Fairy
    }

    #endregion

    [CreateAssetMenu(fileName = "Type", menuName = "Pokemon/Type")]
    [Serializable]
    public class Type: ScriptableObject
    {
        #region Fields

        public static Dictionary<Types, Type> types;

        #endregion


        #region Scriptable Object

        public bool is_special_type;
        public Types[] weaknesses;
        public Types[] resistances;
        public Types[] immunities;

        #endregion

    }
}