using System;
using UnityEngine;
using UnityEngine.UI;
using Trainers;
using Eventing;
using Utilities;
using System.Collections.Generic;
using System.IO;

namespace UI
{
    #region Enums

    public enum TextCodes
    {
        PN, // Player Name
        MONEY // Player Money
    }

    #endregion

    public class UIManager : MonoBehaviour
    {
        #region Fields

        private PlayerTrainer player_trainer;

        private Canvas canvas;
        private GameObject message_panel;
        private GameObject choice_panel;
        public ChoicePackage choice_package;

        private GameObject pause_menu;
        public Nullable<PauseMenu.PauseMenuOptions> pause_menu_selection;
        private bool in_options_menu;

        public TextSpeeds text_speed;
        public MenuFrames menu_frame;
        public MessageFrames message_frame;

        private List<string> message_frame_files;
        private List<string> menu_frame_files;

        #endregion


        #region Menu Scenes

        private GameObject pokedex_menu;
        private GameObject pokemon_menu;
        private GameObject summary_screen;
        private GameObject bag_menu;
        private GameObject trainer_card_menu;
        private GameObject options_menu;

        #endregion


        #region Battle Scene
        #endregion


        #region References

        [Header("General UI")]
        public GameObject prefab_message_panel;
        public GameObject prefab_choice_panel;

        [Header("Menu Prefabs")]
        public GameObject prefab_pause_menu;
        public GameObject prefab_pokemon_menu;
        public GameObject prefab_summary_screen;
        public GameObject prefab_options_menu;        

        #endregion


        #region Mono Behavior

        private void Start()
        {
            player_trainer = FindObjectOfType<PlayerTrainer>();
            canvas = FindObjectOfType<Canvas>();
            GetWindowskins();
        }

        #endregion


        #region Player Referencing

        public PlayerTrainer GetPlayerTrainer()
        {
            return player_trainer;
        }

        public PlayerController GetPlayerController()
        {
            return player_trainer.GetComponent<PlayerController>();
        }

        public void DisablePlayerControl()
        {
            player_trainer.GetComponent<PlayerController>().enabled = false;
        }

        public void EnablePlayerControl()
        {
            player_trainer.GetComponent<PlayerController>().enabled = true;
        }

        #endregion


        #region UI Utility Methods

        public TextSpeeds GetTextSpeed()
        {
            if (in_options_menu)
                return options_menu.GetComponent<OptionsMenu>().options.text_speed;
            else
                return text_speed;
        }

        public Color32 GetBestTextColor(Image panel)
        {
            Texture2D windowskin = panel.sprite.texture;

            int x = windowskin.width / 2;
            int y = windowskin.height / 2;
            float lightness = windowskin.GetPixel(x, y).grayscale;

            if (lightness >= 0.5f)
                return Constants.LIGHT_BG_TEXT_COLOR;
            else return Constants.DARK_BG_TEXT_COLOR;
        }

        public bool IsDarkBG(Image panel)
        {
            Texture2D windowskin = panel.sprite.texture;

            int x = windowskin.width / 2;
            int y = windowskin.height / 2;
            float lightness = windowskin.GetPixel(x, y).grayscale;

            return lightness < 0.5f;
        }

        public string ReplaceTextCode(string text)
        {
            if (text.Contains("{") && text.Contains("}"))
            {
                string before = text.Split('{')[0];
                string after = text.Split('}')[1];
                string code = text.Split('{')[1].Split('}')[0];

                TextCodes text_code = (TextCodes)Enum.Parse(typeof(TextCodes), code, true);
                switch (text_code)
                {
                    case TextCodes.PN:
                        code = player_trainer.name; break;
                    case TextCodes.MONEY:
                        code = "$" + player_trainer.money; break;
                    default:
                        code = text; break;
                }

                return before + code + after;
            }
            else return text;
        }

        public Sprite GetCurrentMessageSkin()
        {
            if (in_options_menu)
                return Resources.Load<Sprite>(message_frame_files[(int)options_menu.GetComponent<OptionsMenu>().options.message_frame]);
            else
                return Resources.Load<Sprite>(message_frame_files[(int)message_frame]);
        }

        public Sprite GetCurrentMenuSkin()
        {
            if (in_options_menu)
                return Resources.Load<Sprite>(menu_frame_files[(int)options_menu.GetComponent<OptionsMenu>().options.menu_frame]);
            else
                return Resources.Load<Sprite>(menu_frame_files[(int)menu_frame]);
        }

        public Sprite GetMessageSkin(MessageFrames frame)
        {
            return Resources.Load<Sprite>(message_frame_files[(int)frame]);
        }

        public Sprite GetMenuSkin(MenuFrames frame)
        {
            return Resources.Load<Sprite>(menu_frame_files[(int)frame]);
        }

        #endregion


        #region UI Creation Methods

        public Message ShowMessage(string text)
        {
            // Create text panel
            message_panel = GameObject.Instantiate(prefab_message_panel, canvas.transform);

            // Show text
            Text text_field = message_panel.GetComponentInChildren<Text>();
            text_field.text = text;

            return message_panel.GetComponent<Message>();
        }

        public Choice ShowChoices(ChoicePackage choice_package)
        {
            // Create choice panel
            choice_panel = GameObject.Instantiate(prefab_choice_panel, canvas.transform);
            this.choice_package = choice_package;

            // Show choices text
            Text text_field = choice_panel.GetComponentInChildren<Text>();
            text_field.text = string.Join("\n", choice_package.choices);

            return choice_panel.GetComponent<Choice>();
        }

        public PauseMenu ShowPauseMenu()
        {
            // Create pause menu
            pause_menu = GameObject.Instantiate(prefab_pause_menu, canvas.transform);

            return pause_menu.GetComponent<PauseMenu>();
        }

        public PokemonMenu ShowPokemonMenu()
        {
            // Create pokemon menu
            pokemon_menu = GameObject.Instantiate(prefab_pokemon_menu, canvas.transform);

            return pokemon_menu.GetComponent<PokemonMenu>();
        }

        public SummaryScreen ShowSummaryScreen()
        {
            // Create summary screen
            summary_screen = GameObject.Instantiate(prefab_summary_screen, canvas.transform);

            return summary_screen.GetComponent<SummaryScreen>();
        }

        public OptionsMenu ShowOptionsMenu()
        {
            // Create options menu
            options_menu = GameObject.Instantiate(prefab_options_menu, canvas.transform);
            in_options_menu = true;

            return options_menu.GetComponent<OptionsMenu>();
        }
        public void ClosedOptionsMenu()
        {
            in_options_menu = false;
        }

        #endregion


        #region UI Manager Methods

        private void GetWindowskins()
        {
            message_frame_files = new List<string>();
            menu_frame_files = new List<string>();

            string windowskins_path = Settings.WINDOWSKINS_PATH;
            string[] windowskin_files = Directory.GetFiles(windowskins_path, "*.png");
            foreach (string file in windowskin_files)
            {
                string file_name = Path.GetFileNameWithoutExtension(file);
                string local_path = Path.GetFileName(windowskins_path);
                string file_path = Path.Combine(local_path, file_name);

                string[] parts = file_name.Split('_');
                if (parts[0] == Settings.MESSAGE_SKIN_PREFIX)
                    message_frame_files.Add(file_path);
                else if (parts[0] == Settings.MENU_SKIN_PREFIX)
                    menu_frame_files.Add(file_path);
            }
        }

        #endregion

    }
}