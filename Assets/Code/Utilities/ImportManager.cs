using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using UnityEditor;
using Pokemon;
using Battle;
using Items;

namespace Utilities
{
    public class ImportManager : MonoBehaviour
    {
        #region Fields

        private Canvas canvas;
        private InputField[] input_fields;

        public InputField types_input;
        public InputField abilities_input;
        public InputField moves_input;
        public InputField items_input;
        public InputField species_input;
        public InputField alt_form_input;

        private string import_path;

        private List<Pokemon.Type> imported_types;
        private List<Ability> imported_abilities;
        private List<Move> imported_moves;
        private List<Item> imported_items;
        private List<Specie> imported_species;
        private List<Form> imported_forms;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            canvas = FindObjectOfType<Canvas>();
            input_fields = FindObjectsOfType<InputField>();

            foreach (InputField input_field in input_fields)
            {
                if (types_input == null && input_field.transform.parent.name == "Types Import")
                    types_input = input_field;
                else if (types_input == null && input_field.transform.parent.name == "Abilities Import")
                    abilities_input = input_field;
                else if (moves_input == null && input_field.transform.parent.name == "Moves Import")
                    moves_input = input_field;
                else if (items_input == null && input_field.transform.parent.name == "Items Import")
                    items_input = input_field;
                else if (species_input == null && input_field.transform.parent.name == "Species Import")
                    species_input = input_field;
                else if (alt_form_input == null && input_field.transform.parent.name == "Alt Forms Import")
                    alt_form_input = input_field;
            }

            import_path = Settings.DATA_IMPORT_PATH;
        }

        #endregion


        #region Import Types

        public void TypesImport()
        {
            string file_path = types_input.GetComponentsInChildren<Text>()[1].text;
            string full_path = Path.Combine(import_path, file_path);

            imported_types = new List<Pokemon.Type>();

            try
            {
                StreamReader input_stream = new StreamReader(full_path);
                Pokemon.Type current_type = null;

                while (!input_stream.EndOfStream)
                {
                    string line = Regex.Replace(input_stream.ReadLine(), @"\s+", "");

                    // Line is a comment
                    if (line[0] == '#') { }
                    // Line is start of a new type
                    else if (line[0] == '[')
                    {
                        if (current_type != null)
                            imported_types.Add(current_type);

                        current_type = ScriptableObject.CreateInstance<Pokemon.Type>();
                    }
                    // Line contains data field
                    else
                    {
                        string[] parts = line.Split('=');
                        string key = parts[0];
                        string value = parts[1];

                        // Part is name
                        if (key == "Name")
                            current_type.name = value;
                        // Part is weaknesses list
                        else if (key == "Weaknesses")
                        {
                            string[] values = value.Split(',');
                            int num_weaknesses = values.Length;
                            Pokemon.Types[] weaknesses = new Pokemon.Types[num_weaknesses];
                            for (int i = 0; i < num_weaknesses; i++)
                            {
                                weaknesses[i] = (Pokemon.Types)Enum.Parse(typeof(Pokemon.Types), values[i], true);
                            }
                            current_type.weaknesses = weaknesses;
                        }
                        // Part is resistances list
                        else if (key == "Resistances")
                        {
                            string[] values = value.Split(',');
                            int num_resistances = values.Length;
                            Pokemon.Types[] resistances = new Pokemon.Types[num_resistances];
                            for (int i = 0; i < num_resistances; i++)
                            {
                                resistances[i] = (Pokemon.Types)Enum.Parse(typeof(Pokemon.Types), values[i], true);
                            }
                            current_type.resistances = resistances;
                        }
                        // Part is immunities list
                        else if (key == "Immunities")
                        {
                            string[] values = value.Split(',');
                            int num_immunities = values.Length;
                            Pokemon.Types[] immunities = new Pokemon.Types[num_immunities];
                            for (int i = 0; i < num_immunities; i++)
                            {
                                immunities[i] = (Pokemon.Types)Enum.Parse(typeof(Pokemon.Types), values[i], true);
                            }
                            current_type.immunities = immunities;
                        }
                    }
                }

                if (current_type != null)
                    imported_types.Add(current_type);
                input_stream.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            string types_path = Settings.TYPES_FILE_PATH;

            for (int i = 0; i < imported_types.Count; i++)
            {
                Pokemon.Type new_type = imported_types[i];

                AssetDatabase.CreateAsset(new_type, Path.Combine(types_path, new_type.name) + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = new_type;
            }
        }

        #endregion


        #region Import Abilities

        public void AbilitiesImport()
        {
            string file_path = abilities_input.GetComponentsInChildren<Text>()[1].text;
            string full_path = Path.Combine(import_path, file_path);

            imported_abilities = new List<Ability>();

            try
            {
                StreamReader input_stream = new StreamReader(full_path);

                while (!input_stream.EndOfStream)
                {
                    string line = input_stream.ReadLine().Replace(@"\", "");

                    // Line is a comment
                    if (line[0] == '#') { }
                    // Line is an ability
                    else
                    {
                        Ability current_ability = ScriptableObject.CreateInstance<Ability>();
                        string[] parts = line.Split(',');
                        int line_length = parts.Length;

                        current_ability.name = parts[2];

                        // Handle commas in description
                        int description_parts = line_length - 4;
                        string description = parts[3].TrimStart('"');
                        for (int i = 0; i < description_parts; i++)
                            description += "," + parts[4 + i];
                        current_ability.description = description.TrimEnd('"');
                        
                        imported_abilities.Add(current_ability);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            string abilities_path = Settings.ABILITIES_FILE_PATH;

            for (int i = 0; i < imported_abilities.Count; i++)
            {
                Ability new_ability = imported_abilities[i];

                AssetDatabase.CreateAsset(new_ability, Path.Combine(abilities_path, new_ability.name) + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = new_ability;
            }
        }

        #endregion


        #region Import Moves

        public void MovesImport()
        {
            string file_path = moves_input.GetComponentsInChildren<Text>()[1].text;
            string full_path = Path.Combine(import_path, file_path);

            imported_moves = new List<Move>();

            try
            {
                StreamReader input_stream = new StreamReader(full_path);

                while (!input_stream.EndOfStream)
                {
                    string line = input_stream.ReadLine().Replace(@"\", "");

                    // Line is a comment
                    if (line[0] == '#') { }
                    // Line is a move
                    else
                    {
                        Move current_move = ScriptableObject.CreateInstance<Move>();
                        string[] parts = line.Split(',');
                        int line_length = parts.Length;

                        // Name
                        current_move.name = parts[2];
                        // Move Effect
                        int effect_code = int.Parse(parts[3], System.Globalization.NumberStyles.HexNumber);
                        current_move.move_effect = (MoveEffects)effect_code;
                        // Base Power
                        current_move.base_power = int.Parse(parts[4]);
                        // Type
                        current_move.type = (Pokemon.Types)Enum.Parse(typeof(Pokemon.Types), parts[5], true);
                        // Damage Category
                        current_move.damage_category = (DamageCategories)Enum.Parse(typeof(DamageCategories), parts[6], true);
                        // Accuracy
                        current_move.accuracy = int.Parse(parts[7]);
                        // Total PP
                        current_move.total_pp = int.Parse(parts[8]);
                        // Effect Chance
                        current_move.effect_chance = int.Parse(parts[9]);
                        // Target
                        current_move.target = (Targets)Enum.Parse(typeof(Targets), parts[10], true);
                        // Priority
                        current_move.priority = int.Parse(parts[11]);
                        // Move Flags
                        string flags_string = parts[12];
                        MoveFlags flags = new MoveFlags();
                        if (flags_string.Contains('a'))
                            flags.makes_physical_contact = true;
                        if (flags_string.Contains('b'))
                            flags.can_protect_against = true;
                        if (flags_string.Contains('c'))
                            flags.magic_coat_can_reflect = true;
                        if (flags_string.Contains('d'))
                            flags.snatch_can_steal = true;
                        if (flags_string.Contains('e'))
                            flags.mirror_move_can_copy = true;
                        if (flags_string.Contains('f'))
                            flags.damage_no_flinch_chance = true;
                        if (flags_string.Contains('g'))
                            flags.thaws_if_frozen = true;
                        if (flags_string.Contains('h'))
                            flags.has_high_crit_rate = true;
                        if (flags_string.Contains('i'))
                            flags.is_biting = true;
                        if (flags_string.Contains('j'))
                            flags.is_punching = true;
                        if (flags_string.Contains('k'))
                            flags.is_sound_based = true;
                        if (flags_string.Contains('l'))
                            flags.is_powder_based = true;
                        if (flags_string.Contains('m'))
                            flags.is_pulse_based = true;
                        if (flags_string.Contains('n'))
                            flags.is_bomb_based = true;
                        if (flags_string.Contains('o'))
                            flags.is_dance = true;
                        current_move.move_flags = flags;
                        // Description
                        int description_parts = line_length - 14;
                        string description = parts[13].TrimStart('"');
                        for (int i = 0; i < description_parts; i++)
                            description += "," + parts[14 + i];
                        current_move.description = description.TrimEnd('"');

                        imported_moves.Add(current_move);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            string moves_path = Settings.MOVES_FILE_PATH;

            for (int i = 0; i < imported_moves.Count; i++)
            {
                Move new_move = imported_moves[i];

                AssetDatabase.CreateAsset(new_move, Path.Combine(moves_path, new_move.name) + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = new_move;
            }
        }

        #endregion


        #region Import Items

        public void ItemsImport()
        {
            string file_path = items_input.GetComponentsInChildren<Text>()[1].text;
            string full_path = Path.Combine(import_path, file_path);

            imported_items = new List<Item>();

            try
            {
                StreamReader input_stream = new StreamReader(full_path);

                while (!input_stream.EndOfStream)
                {
                    string line = input_stream.ReadLine().Replace(@"\", "");

                    // Line is a comment
                    if (line[0] == '#') { }
                    // Line is an item
                    else
                    {
                        Item current_item = ScriptableObject.CreateInstance<Item>();
                        string[] parts = line.Split(',');
                        int line_length = parts.Length;
                        bool is_move_teaching = false;
                        if (parts[line_length - 1] != "")
                            is_move_teaching = true;

                        // ID
                        current_item.item_id = uint.Parse(parts[0]);
                        // Name
                        current_item.name = parts[2];
                        // Name Plural
                        current_item.plural_name = parts[3];
                        // Bag Pocket
                        current_item.bag_pocket = (BagPockets)(int.Parse(parts[4]) - 1);
                        // Price
                        current_item.price = int.Parse(parts[5]);
                        // Description
                        int description_parts = line_length - 6 - 4;
                        string description = parts[6].TrimStart('"');
                        for (int i = 0; i < description_parts - 1; i++)
                            description += "," + parts[7 + i];
                        current_item.description = description.TrimEnd('"');
                        // Usability Out of Battle
                        current_item.out_of_battle_use = (OutOfBattleUses)int.Parse(parts[6 + description_parts]);
                        // Usability In Battle
                        current_item.in_battle_use = (InBattleUses)int.Parse(parts[7 + description_parts]);
                        // Special Item Type
                        current_item.special_item_type = (SpecialItemTypes)int.Parse(parts[8 + description_parts]);
                        if (is_move_teaching)
                            current_item.move_taught = (Moves)Enum.Parse(typeof(Moves), parts[9 + description_parts], true);

                        imported_items.Add(current_item);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            string items_path = Settings.ITEMS_FILE_PATH;

            for (int i = 0; i < imported_items.Count; i++)
            {
                Item new_item = imported_items[i];

                AssetDatabase.CreateAsset(new_item, Path.Combine(items_path, new_item.name) + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = new_item;
            }
        }

        #endregion


        #region Import Species

        public void SpeciesImport()
        {
            string file_path = species_input.GetComponentsInChildren<Text>()[1].text;
            string full_path = Path.Combine(import_path, file_path);

            imported_species = new List<Specie>();

            try
            {
                StreamReader input_stream = new StreamReader(full_path);
                Specie current_species = null;

                while (!input_stream.EndOfStream)
                {
                    string line = input_stream.ReadLine();

                    // Line is a comment
                    if (line[0] == '#') { }
                    // Line is start of a new species, including national dex number
                    else if (line[0] == '[')
                    {
                        if (current_species != null)
                            imported_species.Add(current_species);

                        current_species = ScriptableObject.CreateInstance<Specie>();
                        current_species.national_dex = uint.Parse(line.TrimStart('[').TrimEnd(']'));
                    }
                    // Line contains data field
                    else
                    {
                        string[] parts = line.Split('=');
                        string key = parts[0].TrimEnd(' ');
                        string value = parts[1].TrimStart(' ');

                        // Part is name
                        if (key == "Name")
                            current_species.name = value;
                        // Part is type1
                        else if (key == "Type1")
                        {
                            current_species.types = new Pokemon.Types[2];
                            current_species.types[0] = (Pokemon.Types)Enum.Parse(typeof(Pokemon.Types), value, true);
                        }
                        // Part is type2
                        else if (key == "Type2")
                            current_species.types[1] = (Pokemon.Types)Enum.Parse(typeof(Pokemon.Types), value, true);
                        // Part is base stats
                        else if (key == "BaseStats")
                        {
                            string[] values = value.Split(',');
                            StatArray base_stats = new StatArray();
                            base_stats.HP = uint.Parse(values[0]);
                            base_stats.ATK = uint.Parse(values[1]);
                            base_stats.DEF = uint.Parse(values[2]);
                            base_stats.SPD = uint.Parse(values[3]);
                            base_stats.SP_ATK = uint.Parse(values[4]);
                            base_stats.SP_DEF = uint.Parse(values[5]);

                            current_species.base_stats = base_stats;
                        }
                        // Part is gender rate
                        else if (key == "GenderRate")
                            current_species.gender_rate = (GenderRates)Enum.Parse(typeof(GenderRates), value, true);
                        // Part is growth rate
                        else if (key == "GrowthRate")
                            current_species.growth_rate = (GrowthRates)Enum.Parse(typeof(GrowthRates), value, true);
                        // Part is base exp
                        else if (key == "BaseEXP")
                            current_species.base_exp = uint.Parse(value);
                        // Part is effort values
                        else if (key == "EffortPoints")
                        {
                            string[] values = value.Split(',');
                            StatArray effort_values = new StatArray();
                            effort_values.HP = uint.Parse(values[0]);
                            effort_values.ATK = uint.Parse(values[1]);
                            effort_values.DEF = uint.Parse(values[2]);
                            effort_values.SPD = uint.Parse(values[3]);
                            effort_values.SP_ATK = uint.Parse(values[4]);
                            effort_values.SP_DEF = uint.Parse(values[5]);

                            current_species.effort_values = effort_values;
                        }
                        // Part is rareness
                        else if (key == "Rareness")
                            current_species.rareness = uint.Parse(value);
                        // Part is happiness
                        else if (key == "Happiness")
                            current_species.happiness = uint.Parse(value);
                        // Part is moves
                        else if (key == "Moves")
                        {
                            string[] values = value.Split(',');
                            int num_moves = values.Length / 2;
                            LevelUpMove[] level_up_moves = new LevelUpMove[num_moves];
                            for (int i = 0; i < num_moves * 2; i += 2)
                            {
                                LevelUpMove next_move = new LevelUpMove();
                                next_move.level = uint.Parse(values[i]);
                                next_move.move = (Moves)Enum.Parse(typeof(Moves), values[i + 1], true);
                                level_up_moves[i / 2] = next_move;
                            }

                            current_species.moves = level_up_moves;
                        }
                        // Part is egg group
                        else if (key == "Compatibility")
                        {
                            string[] values = value.Split(',');
                            int num_groups = values.Length;
                            EggGroups[] egg_groups = new EggGroups[num_groups];
                            for (int i = 0; i < num_groups; i++)
                                egg_groups[i] = (EggGroups)Enum.Parse(typeof(EggGroups), values[i], true);

                            current_species.egg_groups = egg_groups;
                        }
                        // Part is steps to hatch
                        else if (key == "StepsToHatch")
                            current_species.steps_to_hatch = uint.Parse(value);
                        // Part is height
                        else if (key == "Height")
                            current_species.height = float.Parse(value);
                        // Part is weight
                        else if (key == "Weight")
                            current_species.weight = float.Parse(value);
                        // Part is color
                        else if (key == "Color")
                            current_species.color = (Colors)Enum.Parse(typeof(Colors), value, true);
                        // Part is shape
                        else if (key == "Shape")
                            current_species.shape = (Shapes)(int.Parse(value) - 1);
                        // Part is kind
                        else if (key == "Kind")
                            current_species.kind = value;
                        // Part is pokedex entry
                        else if (key == "Pokedex")
                            current_species.pokedex_entry = value;
                        // Part is abilities
                        else if (key == "Abilities")
                        {
                            string[] values = value.Split(',');
                            int num_abilities = values.Length;
                            Abilities[] abilities = new Abilities[num_abilities];
                            for (int i = 0; i < num_abilities; i++)
                                abilities[i] = (Abilities)Enum.Parse(typeof(Abilities), values[i], true);

                            current_species.abilities = abilities;
                        }
                        // Part is hidden abilities
                        else if (key == "HiddenAbility")
                        {
                            string[] values = value.Split(',');
                            int num_abilities = values.Length;
                            Abilities[] abilities = new Abilities[num_abilities];
                            for (int i = 0; i < num_abilities; i++)
                                abilities[i] = (Abilities)Enum.Parse(typeof(Abilities), values[i], true);

                            current_species.hidden_ability = abilities;
                        }
                        // Part is egg moves
                        else if (key == "EggMoves")
                        {
                            string[] values = value.Split(',');
                            int num_moves = values.Length;
                            Moves[] moves = new Moves[num_moves];
                            for (int i = 0; i < num_moves; i++)
                                moves[i] = (Moves)Enum.Parse(typeof(Moves), values[i], true);

                            current_species.egg_moves = moves;
                        }
                        // Part is habitat
                        else if (key == "Habitat")
                            current_species.habitat = (Habitats)Enum.Parse(typeof(Habitats), value, true);
                        // Part is regional dex numbers
                        else if (key == "RegionalNumbers")
                        {
                            string[] values = value.Split(',');
                            int num_dexes = values.Length;
                            uint[] numbers = new uint[num_dexes];
                            for (int i = 0; i < num_dexes; i++)
                                numbers[i] = uint.Parse(values[i]);

                            current_species.regional_dexes = numbers;
                        }
                        // Part is wild item (3 varieties)
                        else if (key == "WildItemCommon")
                            current_species.wild_item_common = (Items.Items)Enum.Parse(typeof(Items.Items), value, true);
                        else if (key == "WildItemUncommon")
                            current_species.wild_item_uncommon = (Items.Items)Enum.Parse(typeof(Items.Items), value, true);
                        else if (key == "WildItemRare")
                            current_species.wild_item_rare = (Items.Items)Enum.Parse(typeof(Items.Items), value, true);
                        // Part is an in battle sprite offset
                        else if (key == "BattlerPlayerX")
                            current_species.battler_player_x = int.Parse(value);
                        else if (key == "BattlerPlayerY")
                            current_species.battler_player_y = int.Parse(value);
                        else if (key == "BattlerEnemyX")
                            current_species.battler_enemy_x = int.Parse(value);
                        else if (key == "BattlerEnemyY")
                            current_species.battler_enemy_y = int.Parse(value);
                        else if (key == "BattlerAltitude")
                            current_species.battler_altitude = int.Parse(value);
                        else if (key == "BattlerShadowX")
                            current_species.battler_shadow_x = int.Parse(value);
                        else if (key == "BattlerShadowSize")
                            current_species.battler_shadow_size = int.Parse(value);
                        // Part is evolutions
                        else if (key == "Evolutions")
                        {
                            string[] values = value.Split(',');
                            int num_evos = values.Length / 3;
                            Evolution[] evolutions = new Evolution[num_evos];
                            for (int i = 0; i < num_evos * 3; i += 3)
                            {
                                Evolution next_evo = new Evolution();
                                next_evo.evolution = (Species)Enum.Parse(typeof(Species), values[i], true);
                                next_evo.evolution_method = (EvolutionMethods)Enum.Parse(typeof(EvolutionMethods), values[i + 1], true);
                                next_evo.parameter = values[i + 2];
                                evolutions[i / 3] = next_evo;
                            }

                            current_species.evolutions = evolutions;
                        }
                        // Part is form name
                        else if (key == "FormName")
                            current_species.form_name = value;
                        // Part is incense
                        else if (key == "Incense")
                            current_species.incense = (Items.Items)Enum.Parse(typeof(Items.Items), value, true);
                    }
                }

                if (current_species != null)
                    imported_species.Add(current_species);
                input_stream.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            string species_path = Settings.SPECIES_FILE_PATH;

            for (int i = 0; i < imported_species.Count; i++)
            {
                Specie new_species = imported_species[i];

                AssetDatabase.CreateAsset(new_species, Path.Combine(species_path, new_species.name) + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = new_species;
            }
        }

        #endregion


        #region Import Alt Forms

        public void AltFormsImport()
        {
            string file_path = alt_form_input.GetComponentsInChildren<Text>()[1].text;
            string full_path = Path.Combine(import_path, file_path);

            imported_forms = new List<Form>();

            try
            {
                StreamReader input_stream = new StreamReader(full_path);
                Form current_form = null;

                while (!input_stream.EndOfStream)
                {
                    string line = input_stream.ReadLine();

                    // Line is a comment
                    if (line[0] == '#') { }
                    // Line is start of a new species, including base species and form id
                    else if (line[0] == '[')
                    {
                        if (current_form != null)
                            imported_forms.Add(current_form);

                        current_form = ScriptableObject.CreateInstance<Form>();
                        string[] values = line.TrimStart('[').TrimEnd(']').Split(',');
                        current_form.base_species = (Species)Enum.Parse(typeof(Species), values[0], true);
                        current_form.form_id = uint.Parse(values[1]);

                        current_form.name = current_form.base_species.ToString() + "_" + values[1];
                    }
                    // Line contains data field
                    else
                    {
                        string[] parts = line.Split('=');
                        string key = parts[0].TrimEnd(' ');
                        string value = parts[1].TrimStart(' ');

                        // Part is form name
                        if (key == "FormName")
                        {
                            current_form.form_name = value;
                        }
                        // Part is type1
                        else if (key == "Type1")
                        {
                            current_form.types = new Pokemon.Types[2];
                            current_form.types[0] = (Pokemon.Types)Enum.Parse(typeof(Pokemon.Types), value, true);
                        }
                        // Part is type2
                        else if (key == "Type2")
                            current_form.types[1] = (Pokemon.Types)Enum.Parse(typeof(Pokemon.Types), value, true);
                        // Part is base stats
                        else if (key == "BaseStats")
                        {
                            string[] values = value.Split(',');
                            StatArray base_stats = new StatArray();
                            base_stats.HP = uint.Parse(values[0]);
                            base_stats.ATK = uint.Parse(values[1]);
                            base_stats.DEF = uint.Parse(values[2]);
                            base_stats.SPD = uint.Parse(values[3]);
                            base_stats.SP_ATK = uint.Parse(values[4]);
                            base_stats.SP_DEF = uint.Parse(values[5]);

                            current_form.base_stats = base_stats;
                        }
                        // Part is base exp
                        else if (key == "BaseEXP")
                            current_form.base_exp = uint.Parse(value);
                        // Part is effort values
                        else if (key == "EffortPoints")
                        {
                            string[] values = value.Split(',');
                            StatArray effort_values = new StatArray();
                            effort_values.HP = uint.Parse(values[0]);
                            effort_values.ATK = uint.Parse(values[1]);
                            effort_values.DEF = uint.Parse(values[2]);
                            effort_values.SPD = uint.Parse(values[3]);
                            effort_values.SP_ATK = uint.Parse(values[4]);
                            effort_values.SP_DEF = uint.Parse(values[5]);

                            current_form.effort_values = effort_values;
                        }
                        // Part is rareness
                        else if (key == "Rareness")
                            current_form.rareness = uint.Parse(value);
                        // Part is happiness
                        else if (key == "Happiness")
                            current_form.happiness = uint.Parse(value);
                        // Part is moves
                        else if (key == "Moves")
                        {
                            string[] values = value.Split(',');
                            int num_moves = values.Length / 2;
                            LevelUpMove[] level_up_moves = new LevelUpMove[num_moves];
                            for (int i = 0; i < num_moves * 2; i += 2)
                            {
                                LevelUpMove next_move = new LevelUpMove();
                                next_move.level = uint.Parse(values[i]);
                                next_move.move = (Moves)Enum.Parse(typeof(Moves), values[i + 1], true);
                                level_up_moves[i / 2] = next_move;
                            }

                            current_form.moves = level_up_moves;
                        }
                        // Part is egg group
                        else if (key == "Compatibility")
                        {
                            string[] values = value.Split(',');
                            int num_groups = values.Length;
                            EggGroups[] egg_groups = new EggGroups[num_groups];
                            for (int i = 0; i < num_groups; i++)
                                egg_groups[i] = (EggGroups)Enum.Parse(typeof(EggGroups), values[i], true);

                            current_form.egg_groups = egg_groups;
                        }
                        // Part is steps to hatch
                        else if (key == "StepsToHatch")
                            current_form.steps_to_hatch = uint.Parse(value);
                        // Part is height
                        else if (key == "Height")
                            current_form.height = float.Parse(value);
                        // Part is weight
                        else if (key == "Weight")
                            current_form.weight = float.Parse(value);
                        // Part is color
                        else if (key == "Color")
                            current_form.color = (Colors)Enum.Parse(typeof(Colors), value, true);
                        // Part is shape
                        else if (key == "Shape")
                            current_form.shape = (Shapes)(int.Parse(value) - 1);
                        // Part is kind
                        else if (key == "Kind")
                            current_form.kind = value;
                        // Part is pokedex entry
                        else if (key == "Pokedex")
                            current_form.pokedex_entry = value;
                        // Part is abilities
                        else if (key == "Abilities")
                        {
                            string[] values = value.Split(',');
                            int num_abilities = values.Length;
                            Abilities[] abilities = new Abilities[num_abilities];
                            for (int i = 0; i < num_abilities; i++)
                                abilities[i] = (Abilities)Enum.Parse(typeof(Abilities), values[i], true);

                            current_form.abilities = abilities;
                        }
                        // Part is hidden abilities
                        else if (key == "HiddenAbility")
                        {
                            string[] values = value.Split(',');
                            int num_abilities = values.Length;
                            Abilities[] abilities = new Abilities[num_abilities];
                            for (int i = 0; i < num_abilities; i++)
                                abilities[i] = (Abilities)Enum.Parse(typeof(Abilities), values[i], true);

                            current_form.hidden_ability = abilities;
                        }
                        // Part is egg moves
                        else if (key == "EggMoves")
                        {
                            string[] values = value.Split(',');
                            int num_moves = values.Length;
                            Moves[] moves = new Moves[num_moves];
                            for (int i = 0; i < num_moves; i++)
                                moves[i] = (Moves)Enum.Parse(typeof(Moves), values[i], true);

                            current_form.egg_moves = moves;
                        }
                        // Part is habitat
                        else if (key == "Habitat")
                            current_form.habitat = (Habitats)Enum.Parse(typeof(Habitats), value, true);
                        // Part is wild item (3 varieties)
                        else if (key == "WildItemCommon")
                            current_form.wild_item_common = (Items.Items)Enum.Parse(typeof(Items.Items), value, true);
                        else if (key == "WildItemUncommon")
                            current_form.wild_item_uncommon = (Items.Items)Enum.Parse(typeof(Items.Items), value, true);
                        else if (key == "WildItemRare")
                            current_form.wild_item_rare = (Items.Items)Enum.Parse(typeof(Items.Items), value, true);
                        // Part is an in battle sprite offset
                        else if (key == "BattlerPlayerX")
                            current_form.battler_player_x = int.Parse(value);
                        else if (key == "BattlerPlayerY")
                            current_form.battler_player_y = int.Parse(value);
                        else if (key == "BattlerEnemyX")
                            current_form.battler_enemy_x = int.Parse(value);
                        else if (key == "BattlerEnemyY")
                            current_form.battler_enemy_y = int.Parse(value);
                        else if (key == "BattlerAltitude")
                            current_form.battler_altitude = int.Parse(value);
                        else if (key == "BattlerShadowX")
                            current_form.battler_shadow_x = int.Parse(value);
                        else if (key == "BattlerShadowSize")
                            current_form.battler_shadow_size = int.Parse(value);
                        // Part is evolutions
                        else if (key == "Evolutions")
                        {
                            string[] values = value.Split(',');
                            int num_evos = values.Length / 3;
                            Evolution[] evolutions = new Evolution[num_evos];
                            for (int i = 0; i < num_evos * 3; i += 3)
                            {
                                Evolution next_evo = new Evolution();
                                next_evo.evolution = (Species)Enum.Parse(typeof(Species), values[i], true);
                                next_evo.evolution_method = (EvolutionMethods)Enum.Parse(typeof(EvolutionMethods), values[i + 1], true);
                                next_evo.parameter = values[i + 2];
                                evolutions[i / 3] = next_evo;
                            }

                            current_form.evolutions = evolutions;
                        }
                        // Part is mega stone
                        else if (key == "MegaStone")
                            current_form.mega_stone = (Items.Items)Enum.Parse(typeof(Items.Items), value, true);
                        // Part is mega move
                        else if (key == "MegaMove")
                            current_form.mega_move = (Moves)Enum.Parse(typeof(Moves), value, true);
                        // Part is mega message
                        else if (key == "MegaMessage")
                            current_form.mega_message = value;
                        // Part is unmega form id
                        else if (key == "UnmegaForm")
                            current_form.unmega_form_id = uint.Parse(value);
                        // Part is pokedex form id
                        else if (key == "PokedexForm")
                            current_form.pokedex_form_id = uint.Parse(value);
                    }
                }

                if (current_form != null)
                    imported_forms.Add(current_form);
                input_stream.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            string forms_path = Settings.ALT_FORMS_FILE_PATH;

            for (int i = 0; i < imported_forms.Count; i++)
            {
                Form new_form = imported_forms[i];

                AssetDatabase.CreateAsset(new_form, Path.Combine(forms_path, new_form.name) + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = new_form;
            }
        }

        #endregion

    }
}