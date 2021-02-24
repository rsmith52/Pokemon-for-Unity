using System.IO;
using UnityEngine;

namespace Utilities
{
    public static class Settings
    {
        #region Game

        public static readonly string START_SCENE = Constants.MAP_MAKING_SCENE;
        public static readonly int START_MAP_ID = 0;

        #endregion


        #region Pokemon

        public static readonly float SHINY_CHANCE = 16; // SHINY_CHANCE / 65536
        public static readonly bool ANIMATED_SPRITES = true;

        #endregion


        #region File Paths

        // Main UI Files
        public static readonly string WINDOWSKINS_PATH = Path.Combine("Assets", "Resources", "Windowskins");
        public static readonly string MESSAGE_SKIN_PREFIX = "Message";
        public static readonly string MENU_SKIN_PREFIX = "Choice";

        // Sprite Files
        public static readonly string ITEM_ICON_PATH = Path.Combine("Assets", "Resources", "Item_Icons");
        public static readonly string ITEM_ICON_PREFIX = "item";
        public static readonly string ITEM_MISSING_ICON = "item000";
        public static readonly string POKEMON_ICON_PATH = Path.Combine("Assets", "Resources", "Pokemon_Icons");
        public static readonly string POKEMON_ICON_PREFIX = "icon";
        public static readonly string POKEMON_MISSING_ICON = "icon000";
        public static readonly string POKEMON_SPRITE_PATH = Path.Combine("Assets", "Resources", "Pokemon_Sprites");
        public static readonly string POKEMON_MISSING_SPRITE = "000";
        public static readonly string POKEMON_ANIM_SPRITE_PATH = Path.Combine("Assets", "Resources", "Pokemon_Anim_Sprites");

        // Audio Files
        public static readonly string POKEMON_CRIES_PATH = Path.Combine("Assets", "Resources", "Pokemon_Cries");
        public static readonly string POKEMON_CRY_SUFFIX = "Cry";

        // Data Files
        public static readonly string DATA_IMPORT_PATH = Path.Combine(Application.dataPath, "Resources", "Imports");
        public static readonly string ABILITIES_FILE_PATH = Path.Combine("Assets", "Resources", "Data", "Abilities");
        public static readonly string ALT_FORMS_FILE_PATH = Path.Combine("Assets", "Resources", "Data", "Alt_Forms");
        public static readonly string ITEMS_FILE_PATH = Path.Combine("Assets", "Resources", "Data", "Items");
        public static readonly string MOVES_FILE_PATH = Path.Combine("Assets", "Resources", "Data", "Moves");
        public static readonly string SPECIES_FILE_PATH = Path.Combine("Assets", "Resources", "Data", "Species");
        public static readonly string TYPES_FILE_PATH = Path.Combine("Assets", "Resources", "Data", "Types");
        public static readonly string MAPS_FILE_PATH = Path.Combine("Assets", "Resources", "Data", "Maps");

        #endregion
    }
}