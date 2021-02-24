using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Items;
using Pokemon;
using System;
using System.Text.RegularExpressions;
using Mapping;

namespace Utilities
{
    public class DataLoader : MonoBehaviour
    {
        #region Fields

        public bool data_loaded;
        public float load_time;

        public bool scene_loading;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            data_loaded = false;
            load_time = Time.time;
            scene_loading = false;

            LoadTypes();
            LoadAbilities();
            LoadMoves();
            LoadItems();
            LoadSpecies();
            LoadAltForms();
            LoadMaps();

            data_loaded = true;
        }

        private void Update()
        {
            if (data_loaded && !scene_loading)
            {
                Debug.Log("Load Time: " + (Time.time - load_time).ToString());
                StartCoroutine(SceneLoader.InitialLoadOverworldScene());
                scene_loading = true;
            }
        }

        #endregion


        #region Data Loading

        private static string FileNameToEnumEntry(string file_name)
        {
            // Special Characters
            string name = file_name.Replace('é', 'e').Replace('♀', 'F').Replace('♂', 'M');
            // Ignored Characters
            name = name.Replace('\'', ' ').Replace('-', ' ').Replace('.', ' ');
            // Remove Spaces
            return Regex.Replace(name, @"\s+", "");
        }

        private static void LoadTypes()
        {
            Debug.Log("Loading Types...");
            Dictionary<Pokemon.Types, Pokemon.Type> types = new Dictionary<Pokemon.Types, Pokemon.Type>();
            string types_path = Settings.TYPES_FILE_PATH;

            string[] type_files = Directory.GetFiles(types_path, "*.asset");
            foreach (string file in type_files)
            {
                string local_path = Path.Combine(Path.GetFileName(Path.GetDirectoryName(types_path)), Path.GetFileName(types_path));
                string file_path = Path.Combine(local_path, Path.GetFileNameWithoutExtension(file));
                Pokemon.Type type_obj = Resources.Load<Pokemon.Type>(file_path);
                string type_name = FileNameToEnumEntry(type_obj.name);
                Pokemon.Types type = (Pokemon.Types)Enum.Parse(typeof(Pokemon.Types), type_name, true);
                types[type] = type_obj;
            }

            Pokemon.Type.types = types;
            Debug.Log(Pokemon.Type.types.Count.ToString() + " Type files loaded.");
        }

        private static void LoadAbilities()
        {
            Debug.Log("Loading Abilities...");
            Dictionary<Abilities, Ability> abilities = new Dictionary<Abilities, Ability>();
            string abilities_path = Settings.ABILITIES_FILE_PATH;

            string[] ability_files = Directory.GetFiles(abilities_path, "*.asset");
            foreach (string file in ability_files)
            {
                string local_path = Path.Combine(Path.GetFileName(Path.GetDirectoryName(abilities_path)), Path.GetFileName(abilities_path));
                string file_path = Path.Combine(local_path, Path.GetFileNameWithoutExtension(file));
                Ability ability_obj = Resources.Load<Ability>(file_path);
                string ability_name = FileNameToEnumEntry(ability_obj.name);
                Abilities ability = (Abilities)Enum.Parse(typeof(Abilities), ability_name, true);
                abilities[ability] = ability_obj;
            }

            Ability.abilities = abilities;
            Debug.Log(Ability.abilities.Count.ToString() + " Ability files loaded.");
        }

        private static void LoadMoves()
        {
            Debug.Log("Loading Moves...");
            Dictionary<Moves, Move> moves = new Dictionary<Moves, Move>();
            string moves_path = Settings.MOVES_FILE_PATH;

            string[] move_files = Directory.GetFiles(moves_path, "*.asset");
            foreach (string file in move_files)
            {
                string local_path = Path.Combine(Path.GetFileName(Path.GetDirectoryName(moves_path)), Path.GetFileName(moves_path));
                string file_path = Path.Combine(local_path, Path.GetFileNameWithoutExtension(file));
                Move move_obj = Resources.Load<Move>(file_path);
                string move_name = FileNameToEnumEntry(move_obj.name);
                Moves move = (Moves)Enum.Parse(typeof(Moves), move_name, true);
                moves[move] = move_obj;
            }

            Move.moves =  moves;
            Debug.Log(Move.moves.Count.ToString() + " Move files loaded.");
        }

        private static void LoadItems()
        {
            Debug.Log("Loading Items...");
            Dictionary<Items.Items, Item> items = new Dictionary<Items.Items, Item>();
            string items_path = Settings.ITEMS_FILE_PATH;

            string[] item_files = Directory.GetFiles(items_path, "*.asset");
            foreach (string file in item_files)
            {
                string local_path = Path.Combine(Path.GetFileName(Path.GetDirectoryName(items_path)), Path.GetFileName(items_path));
                string file_path = Path.Combine(local_path, Path.GetFileNameWithoutExtension(file));
                Item item_obj = Resources.Load<Item>(file_path);
                string item_name = FileNameToEnumEntry(item_obj.name);
                Items.Items item = (Items.Items)Enum.Parse(typeof(Items.Items), item_name, true);
                items[item] = item_obj;
            }

            Item.items = items;
            Debug.Log(Item.items.Count.ToString() + " Item files loaded.");
        }

        private static void LoadSpecies()
        {
            Debug.Log("Loading Species...");
            Dictionary<Species, Specie> species = new Dictionary<Species, Specie>();
            string species_path = Settings.SPECIES_FILE_PATH;

            string[] species_files = Directory.GetFiles(species_path, "*.asset");
            foreach (string file in species_files)
            {
                string local_path = Path.Combine(Path.GetFileName(Path.GetDirectoryName(species_path)), Path.GetFileName(species_path));
                string file_path = Path.Combine(local_path, Path.GetFileNameWithoutExtension(file));
                Specie specie_obj = Resources.Load<Specie>(file_path);
                string specie_name = FileNameToEnumEntry(specie_obj.name);
                Species specie = (Species)Enum.Parse(typeof(Species), specie_name, true);
                species[specie] = specie_obj;
            }

            Specie.species = species;
            Debug.Log(Specie.species.Count.ToString() + " Species files loaded.");
        }

        private static void LoadAltForms()
        {
            Debug.Log("Loading Alt Forms...");
            int forms_loaded = 0;
            Dictionary<Species, Dictionary<uint, Form>> forms = new Dictionary<Species, Dictionary<uint, Form>>();
            string forms_path = Settings.ALT_FORMS_FILE_PATH;

            string[] forms_files = Directory.GetFiles(forms_path, "*.asset");
            foreach (string file in forms_files)
            {
                string local_path = Path.Combine(Path.GetFileName(Path.GetDirectoryName(forms_path)), Path.GetFileName(forms_path));
                string file_path = Path.Combine(local_path, Path.GetFileNameWithoutExtension(file));
                Form form_obj = Resources.Load<Form>(file_path);

                string[] species_and_form_id = form_obj.name.Split('_');
                string specie_name = FileNameToEnumEntry(species_and_form_id[0]);
                Species base_species = (Species)Enum.Parse(typeof(Species), specie_name, true);
                uint form_id = uint.Parse(species_and_form_id[1]);

                if (!forms.ContainsKey(base_species))
                    forms[base_species] = new Dictionary<uint, Form>();
                forms[base_species][form_id] = form_obj;
                forms_loaded += 1;
            }

            Form.forms = forms;
            Debug.Log(forms_loaded.ToString() + " Form files loaded.");
        }

        private static void LoadMaps()
        {
            // TODO: This
            Debug.Log("Loading Maps...");
            Dictionary<Maps, Map> maps = new Dictionary<Maps, Map>();
            string maps_path = Settings.MAPS_FILE_PATH;

            string[] map_files = Directory.GetFiles(maps_path, "*.prefab");
            foreach (string file in map_files)
            {
                string local_path = Path.Combine(Path.GetFileName(Path.GetDirectoryName(maps_path)), Path.GetFileName(maps_path));
                string file_path = Path.Combine(local_path, Path.GetFileNameWithoutExtension(file));
                GameObject map_obj = Resources.Load<GameObject>(file_path);
                string map_name = FileNameToEnumEntry(map_obj.name);
                Maps map = (Maps)Enum.Parse(typeof(Maps), map_name, true);
                maps[map] = map_obj.GetComponent<Map>();
            }

            Map.maps = maps;
            Debug.Log(Map.maps.Count.ToString() + " Map files loaded.");
        }

        #endregion

    }
}