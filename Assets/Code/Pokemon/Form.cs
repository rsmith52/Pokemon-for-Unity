using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    [CreateAssetMenu(fileName = "Alt Form", menuName = "Pokemon/Alt Form")]
    public class Form : ScriptableObject
    {
        #region Fields

        public static Dictionary<Species, Dictionary<uint, Form>> forms;

        #endregion


        #region Scriptable Object

        [Header("Basic Info")]
        public Species base_species;
        public string form_name;
        public uint form_id;
        public Types[] types;
        public StatArray base_stats;
        public uint base_exp;
        public StatArray effort_values;
        public uint rareness;
        public uint happiness;
        public Abilities[] abilities;
        public Abilities[] hidden_ability;
        public LevelUpMove[] moves;
        public Evolution[] evolutions;

        [Header("Breeding Info")]
        public Moves[] egg_moves;
        public EggGroups[] egg_groups;
        public uint steps_to_hatch;

        [Header("Pokedex Entry")]
        public float height;
        public float weight;
        public Colors color;
        public Shapes shape;
        public Habitats habitat;
        public string kind;
        public string pokedex_entry;

        [Header("Optional Info")]
        public Items.Items wild_item_common;
        public Items.Items wild_item_uncommon;
        public Items.Items wild_item_rare;

        [Header("Battler Sprite Positioning")]
        public int battler_player_x;
        public int battler_player_y;
        public int battler_enemy_x;
        public int battler_enemy_y;
        public int battler_altitude;
        public int battler_shadow_x;
        public int battler_shadow_size;

        [Header("Mega Form Properties")]
        public Items.Items mega_stone;
        public Moves mega_move;
        public string mega_message;
        public uint unmega_form_id;
        public uint pokedex_form_id;

        #endregion


        #region Static Methods

        public static Form GetFormData(Species species, uint form_id)
        {
            if (forms.ContainsKey(species) && forms[species].ContainsKey(form_id))
                return forms[species][form_id];
            else return null;
        }

        #endregion

    }
}