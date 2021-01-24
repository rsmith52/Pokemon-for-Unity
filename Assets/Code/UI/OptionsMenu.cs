using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    #region Enums

    public enum TextSpeeds
    {
        Slow,
        Medium,
        Fast
    }

    public enum BattleEffects
    {
        On,
        Off
    }

    public enum BattleStyles
    {
        Switch,
        Set
    }

    public enum MessageFrames
    {
        Gold,
        Dark
    }

    public enum MenuFrames
    {
        Gold,
        Dark,
        Platinum
    }

    #endregion


    #region Structs

    public struct Options
    {
        public TextSpeeds text_speed;
        public BattleEffects battle_effect;
        public BattleStyles battle_style;
        public MessageFrames message_frame;
        public MenuFrames menu_frame;
    }

    #endregion

    public class OptionsMenu : MonoBehaviour
    {
        #region Constants

        private static readonly string MENU_TITLE = "Options";
        private static readonly string CLOSE_MESSAGE = "Would you like to save these changes?";
        private static readonly string SAVE_MESSAGE = "Changes saved.";
        private static readonly string[][] MENU_OPTIONS = new string[][]
        {
            new string[] { "Text Speed", "Set the speed at which text appears", "Slow", "Medium", "Fast" },
            new string[] { "Battle Effects", "Set whether battle animations should be shown", "On", "Off" },
            new string[] { "Battle Style", "Set the battle style rules", "Switch", "Set" },
            new string[] { "Message Frame", "Set the frame used for messages" },
            new string[] { "Menu Frame", "Set the frame used for menus and choices" }
        };
        private static readonly int MESSAGE_FRAME_OPTION_INDEX = 3;
        private static readonly int MENU_FRAME_OPTION_INDEX = 4;

        private static readonly float LINE_SPACING = 43.5f;
        

        #endregion


        #region Fields

        private UIManager ui_manager;

        [Header("Menu Components")]
        public Image title_panel;
        public Text title_text_field;
        public Image options_panel;
        public Text options_text_field;
        public Text options_selection_text_field;
        public Image left_arrow;
        public Image right_arrow;
        public Image message_panel;
        public Text message_text_field;

        public Options options;
        private int num_options;
        private int current_selection;
        private bool setting_changed;
        private bool awaiting_input;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            // Get References
            ui_manager = FindObjectOfType<UIManager>();

            // Get Current Options Settings
            options = new Options();
            options.text_speed = ui_manager.text_speed;
            // TODO get other current option settings
            options.menu_frame = ui_manager.menu_frame;
            options.message_frame = ui_manager.message_frame;

            // Set Windowskin and Text Color
            title_panel.sprite = ui_manager.GetCurrentMenuSkin();
            title_text_field.color = ui_manager.GetBestTextColor(title_panel);
            options_panel.sprite = ui_manager.GetCurrentMenuSkin();
            options_text_field.color = ui_manager.GetBestTextColor(options_panel);
            options_selection_text_field.color = ui_manager.GetBestTextColor(options_panel);
            message_panel.sprite = ui_manager.GetCurrentMessageSkin();
            message_text_field.color = ui_manager.GetBestTextColor(message_panel);

            // Build Menu
            num_options = MENU_OPTIONS.Length;
            current_selection = 0;
            BuildMenuText();
            message_text_field.text = GetMessageText(current_selection);
            setting_changed = false;
            awaiting_input = true;
        }

        private void Update()
        {
            if (awaiting_input)
            {
                // Get player inputs
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (current_selection == num_options - 1)
                    {
                        current_selection = 0;
                        MoveArrowsToTop();
                    }
                    else
                    {
                        current_selection += 1;
                        MoveArrowsDown();
                    }
                    message_text_field.text = GetMessageText(current_selection);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (current_selection == 0)
                    {
                        current_selection = num_options - 1;
                        MoveArrowsToBottom();
                    }
                    else
                    {
                        current_selection -= 1;
                        MoveArrowsUp();
                    }
                    message_text_field.text = GetMessageText(current_selection);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    ChangeSelectionLeft(current_selection);
                    setting_changed = true;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    ChangeSelectionRight(current_selection);
                    setting_changed = true;
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    awaiting_input = false;
                    if (setting_changed)
                        StartCoroutine(CloseConfirmation());
                    else
                        CloseMenu();
                }
            }
        }

        #endregion


        #region Options Menu Methods

        private void ChangeSelectionLeft(int index)
        {
            if (MENU_OPTIONS[index].Length > 2)
            {
                switch (index)
                {
                    case 0: // Text Speed
                        if (options.text_speed != TextSpeeds.Slow)
                            options.text_speed -= 1;
                        break;
                    case 1: // Battle Effects
                        if (options.battle_effect != BattleEffects.On)
                            options.battle_effect -= 1;
                        break;
                    case 2: // Battle Style
                        if (options.battle_style != BattleStyles.Switch)
                            options.battle_style -= 1;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (index == MESSAGE_FRAME_OPTION_INDEX)
                {
                    if (options.message_frame == 0)
                        options.message_frame = (MessageFrames)(Enum.GetValues(typeof(MessageFrames)).Length - 1);
                    else
                        options.message_frame -= 1;
                    message_panel.sprite = ui_manager.GetMessageSkin(options.message_frame);
                    message_text_field.color = ui_manager.GetBestTextColor(message_panel);
                }
                else if (index == MENU_FRAME_OPTION_INDEX)
                {
                    if (options.menu_frame == 0)
                        options.menu_frame = (MenuFrames)(Enum.GetValues(typeof(MenuFrames)).Length - 1);
                    else
                        options.menu_frame -= 1;
                    title_panel.sprite = ui_manager.GetMenuSkin(options.menu_frame);
                    title_text_field.color = ui_manager.GetBestTextColor(title_panel);
                    options_panel.sprite = ui_manager.GetMenuSkin(options.menu_frame);
                    options_text_field.color = ui_manager.GetBestTextColor(options_panel);
                    options_selection_text_field.color = ui_manager.GetBestTextColor(options_panel);
                }
            }

            BuildMenuText();
        }

        private void ChangeSelectionRight(int index)
        {
            if (MENU_OPTIONS[index].Length > 2)
            {
                switch (index)
                {
                    case 0: // Text Speed
                        if (options.text_speed != TextSpeeds.Fast)
                            options.text_speed += 1;
                        break;
                    case 1: // Battle Effects
                        if (options.battle_effect != BattleEffects.Off)
                            options.battle_effect += 1;
                        break;
                    case 2: // Battle Style
                        if (options.battle_style != BattleStyles.Set)
                            options.battle_style += 1;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (index == MESSAGE_FRAME_OPTION_INDEX)
                {
                    if (options.message_frame == (MessageFrames)(Enum.GetValues(typeof(MessageFrames)).Length - 1))
                        options.message_frame = 0;
                    else
                        options.message_frame += 1;
                    message_panel.sprite = ui_manager.GetMessageSkin(options.message_frame);
                    message_text_field.color = ui_manager.GetBestTextColor(message_panel);
                }
                else if (index == MENU_FRAME_OPTION_INDEX)
                {
                    if (options.menu_frame == (MenuFrames)(Enum.GetValues(typeof(MenuFrames)).Length - 1))
                        options.menu_frame = 0;
                    else
                        options.menu_frame += 1;
                    title_panel.sprite = ui_manager.GetMenuSkin(options.menu_frame);
                    title_text_field.color = ui_manager.GetBestTextColor(title_panel);
                    options_panel.sprite = ui_manager.GetMenuSkin(options.menu_frame);
                    options_text_field.color = ui_manager.GetBestTextColor(options_panel);
                    options_selection_text_field.color = ui_manager.GetBestTextColor(options_panel);
                }
            }

            BuildMenuText();
        }

        private IEnumerator CloseConfirmation()
        {
            Message message = ui_manager.ShowMessage(CLOSE_MESSAGE);

            yield return new WaitUntil(() => message.on_final_line && !message.writing);

            Choice choice = ui_manager.ShowChoices(Choice.YES_NO_CHOICE);

            yield return new WaitUntil(() => choice.finished);
            int selection = choice.chosen_selection;

            GameObject.Destroy(message.gameObject);
            GameObject.Destroy(choice.gameObject);

            if (selection == 0)
            {
                message = ui_manager.ShowMessage(SAVE_MESSAGE);
                yield return new WaitUntil(() => message.finished);
                GameObject.Destroy(message.gameObject);
                CloseMenuAndSaveOptions();
            }
            else
                CloseMenu();
        }

        public void CloseMenu()
        {
            ui_manager.ClosedOptionsMenu();
            GameObject.Destroy(this.gameObject);
        }

        public void CloseMenuAndSaveOptions()
        {
            ui_manager.ClosedOptionsMenu();
            ui_manager.text_speed = options.text_speed;
            ui_manager.menu_frame = options.menu_frame;
            ui_manager.message_frame = options.message_frame;
            // TODO: Save options settings everywhere needed
            GameObject.Destroy(this.gameObject);
        }

        private void BuildMenuText()
        {
            title_text_field.text = MENU_TITLE;
            options_text_field.text = "";
            options_selection_text_field.text = "";

            for (int i = 0; i < num_options; i++)
            {
                string[] options_line = BuildMenuLine(i);
                if (i == num_options - 1)
                {
                    options_text_field.text += options_line[0];
                    options_selection_text_field.text += options_line[1];
                }
                else
                {
                    options_text_field.text += options_line[0] + '\n';
                    options_selection_text_field.text += options_line[1] + '\n';
                }
            }
        }

        private string[] BuildMenuLine(int index)
        {
            string option_line = "";
            string option_selection_line = "";

            // All lines
            option_line = MENU_OPTIONS[index][0];

            // Typical line with set options
            if (MENU_OPTIONS[index].Length > 2)
            {
                int num_options = MENU_OPTIONS[index].Length - 2;
                int option = 0;
                switch (index)
                {
                    case 0: // Text Speed
                        option = (int)options.text_speed;  break;
                    case 1: // Battle Effects
                        option = (int)options.battle_effect; break;
                    case 2: // Battle Style
                        option = (int)options.battle_style; break;
                    default:
                        break;
                }
                for (int i = 0; i < num_options; i++)
                {
                    if (i == option)
                        option_selection_line += "<color=red>" + MENU_OPTIONS[index][i + 2] + "</color>\t\t";
                    else
                        option_selection_line += MENU_OPTIONS[index][i + 2] + "\t\t";
                }
            }
            // Line with no listed options such as windowskin setting
            else
            {
                if (index == MESSAGE_FRAME_OPTION_INDEX)
                    option_selection_line += Enum.GetNames(typeof(MessageFrames))[(int)options.message_frame];
                else if (index == MENU_FRAME_OPTION_INDEX)
                    option_selection_line += Enum.GetNames(typeof(MenuFrames))[(int)options.menu_frame];
            }

            return new string[] { option_line, option_selection_line };
        }

        private string GetMessageText(int index)
        {
            string message = MENU_OPTIONS[index][1] + '.';
            return message;
        }

        private void MoveArrowsUp()
        {
            left_arrow.rectTransform.Translate(Vector3.up * LINE_SPACING);
            right_arrow.rectTransform.Translate(Vector3.up * LINE_SPACING);
        }

        private void MoveArrowsDown()
        {
            left_arrow.rectTransform.Translate(Vector3.down * LINE_SPACING);
            right_arrow.rectTransform.Translate(Vector3.down * LINE_SPACING);
        }

        private void MoveArrowsToTop()
        {
            left_arrow.rectTransform.Translate(Vector3.up * LINE_SPACING * (num_options - 1));
            right_arrow.rectTransform.Translate(Vector3.up * LINE_SPACING * (num_options - 1));
        }

        private void MoveArrowsToBottom()
        {
            left_arrow.rectTransform.Translate(Vector3.down * LINE_SPACING * (num_options - 1));
            right_arrow.rectTransform.Translate(Vector3.down * LINE_SPACING * (num_options - 1));
        }

        #endregion

    }
}