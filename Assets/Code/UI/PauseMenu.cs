using System;
using System.Collections.Generic;
using System.Linq;
using Eventing;
using Trainers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        #region Enums

        public enum PauseMenuOptions
        {
            Pokedex,
            Pokemon,
            Bag,
            TrainerCard,
            Save,
            Options,
            Exit
        }

        #endregion


        #region Constants

        private static readonly string[] MENU_OPTIONS = new string[]
        {
            "Pokédex",
            "Pokémon",
            "Bag",
            "{PN}",
            "Save",
            "Options",
            "Exit"
        };
        private static readonly float ICON_SPACING = 70f;

        #endregion


        #region Fields

        private UIManager ui_manager;

        [Header("Menu Components")]
        public Image panel;
        public Text text_field;
        public Image[] icons = new Image[MENU_OPTIONS.Length];

        [Header("Menu Icons")]
        public Sprite[] non_selected = new Sprite[MENU_OPTIONS.Length];
        public Sprite[] selected = new Sprite[MENU_OPTIONS.Length];
        public Sprite[] f_selected = new Sprite[MENU_OPTIONS.Length];

        private EventManager event_manager;
        private PlayerTrainer player;

        private List<PauseMenuOptions> menu_options;
        private List<Image> menu_icons;
        public int current_selection;
        private bool interactable;

        private MonoBehaviour open_menu;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            // Get references
            ui_manager = FindObjectOfType<UIManager>();
            event_manager = FindObjectOfType<EventManager>();
            player = ui_manager.GetPlayerTrainer();

            // Pause Game
            ui_manager.DisablePlayerControl();
            // TODO Pause all Events on Map... something in EventManager for sure

            // Set Windowskin and Text Color
            panel.sprite = ui_manager.GetCurrentMenuSkin();
            text_field.color = ui_manager.GetBestTextColor(panel);

            // Build Menu
            BuildMenuOptions();
            if (ui_manager.pause_menu_selection != null)
                current_selection = menu_options.IndexOf((PauseMenuOptions)ui_manager.pause_menu_selection);
            else
                current_selection = 0;
            BuildMenuText();
            BuildMenuIcons();
            interactable = true;
        }

        private void Update()
        {
            // Get player input
            if (interactable)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (current_selection == menu_options.Count - 1)
                        current_selection = 0;
                    else
                        current_selection += 1;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (current_selection == 0)
                        current_selection = menu_options.Count - 1;
                    else
                        current_selection -= 1;
                }
                else if (Input.GetKeyDown(KeyCode.X))
                    CloseMenu();
                else if (Input.GetKeyDown(KeyCode.Z))
                    OpenMenuOption(current_selection);
            }

            // Check if open menu scene was closed
            if (!interactable && open_menu == null)
            {
                panel.sprite = ui_manager.GetCurrentMenuSkin();
                text_field.color = ui_manager.GetBestTextColor(panel);
                interactable = true;
            }

            // Update icons based on selection and gender
            for (int i = 0; i < menu_options.Count; i++)
            {
                if (i == current_selection && player.gender == Pokemon.Genders.Female && f_selected[(int)menu_options[i]] != null)
                    menu_icons[i].sprite = f_selected[(int)menu_options[i]];
                else if (i == current_selection)
                    menu_icons[i].sprite = selected[(int)menu_options[i]];
                else
                    menu_icons[i].sprite = non_selected[(int)menu_options[i]];
            }
        }

        #endregion


        #region Pause Menu Methods

        private void BuildMenuOptions()
        {
            menu_options = new List<PauseMenuOptions>();

            if (player.has_pokedex)
                menu_options.Add(PauseMenuOptions.Pokedex);
            if (!player.party.is_empty)
                menu_options.Add(PauseMenuOptions.Pokemon);
            menu_options.Add(PauseMenuOptions.Bag);
            menu_options.Add(PauseMenuOptions.TrainerCard);
            menu_options.Add(PauseMenuOptions.Save);
            menu_options.Add(PauseMenuOptions.Options);
            menu_options.Add(PauseMenuOptions.Exit);
        }

        private void BuildMenuText()
        {
            text_field.text = "";
            for (int i = 0; i < menu_options.Count; i++)
            {
                if (i == menu_options.Count - 1)
                    text_field.text += ui_manager.ReplaceTextCode(MENU_OPTIONS[(int)menu_options[i]]);
                else
                    text_field.text += ui_manager.ReplaceTextCode(MENU_OPTIONS[(int)menu_options[i]]) + '\n';
            }

            float text_height = text_field.preferredHeight;
        }

        private void BuildMenuIcons()
        {
            menu_icons = new List<Image>();
            float offset = 0;

            for (int i = 0; i < icons.Length; i++)
            {
                if (!menu_options.Contains((PauseMenuOptions)i))
                {
                    GameObject.Destroy(icons[i]);
                    offset += ICON_SPACING;
                    float cur_height = panel.rectTransform.rect.height;
                    panel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cur_height - ICON_SPACING);
                    panel.rectTransform.Translate(Vector3.up * ICON_SPACING / 2);
                }
                else
                {
                    icons[i].rectTransform.Translate(Vector3.up * offset);
                    menu_icons.Add(icons[i]);
                }
                    
            }
        }

        public void CloseMenu()
        {
            ui_manager.pause_menu_selection = menu_options[current_selection];
            ui_manager.EnablePlayerControl();
            // TODO: reenable map events, something in EventManager
            GameObject.Destroy(this.gameObject);
        }

        public void OpenMenuOption(int selection)
        {
            interactable = false;

            PauseMenuOptions menu_selection = (PauseMenuOptions)menu_options[selection];
            switch (menu_selection)
            {
                case PauseMenuOptions.Pokedex:
                    break;
                case PauseMenuOptions.Pokemon:
                    open_menu = ui_manager.ShowPokemonMenu();
                    break;
                case PauseMenuOptions.Bag:
                    break;
                case PauseMenuOptions.TrainerCard:
                    break;
                case PauseMenuOptions.Save:
                    break;
                case PauseMenuOptions.Options:
                    open_menu = ui_manager.ShowOptionsMenu();
                    break;
                case PauseMenuOptions.Exit:
                    current_selection = 0;
                    CloseMenu();
                    break;
                default:
                    break;
            }
        }

        #endregion

    }
}