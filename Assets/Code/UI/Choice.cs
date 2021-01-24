using UnityEngine;
using UnityEngine.UI;
using Eventing;

namespace UI
{
    public class Choice : MonoBehaviour
    {
        #region Constants

        private static readonly float PANEL_WIDTH_BASE = 90;
        private static readonly float PANEL_WIDTH_MULTIPLIER = 0.95f;
        private static readonly float PANEL_HEIGHT_BASE = 40;
        private static readonly float PANEL_HEIGHT_MULTIPLIER = 43.5f;

        public static readonly ChoicePackage YES_NO_CHOICE = new ChoicePackage(new string[] { "Yes", "No" }, 0, 1);

        #endregion


        #region Fields

        private UIManager ui_manager;

        [Header("Choice Components")]
        public Image panel;
        public Text text_field;
        public Image sel_arrow;
        public Image sel_arrow_white;

        private ChoicePackage choice_package;
        private string[] choices;
        private int current_selection;

        public int chosen_selection;
        public bool finished;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            // Get References
            ui_manager = FindObjectOfType<UIManager>();

            // Set Windowskin and Text Color
            panel.sprite = ui_manager.GetCurrentMenuSkin();
            text_field.color = ui_manager.GetBestTextColor(panel);
            if (ui_manager.IsDarkBG(panel))
                sel_arrow.enabled = false;
            else
                sel_arrow_white.enabled = false;

            // Initialize View Settings
            choices = text_field.text.Split('\n');
            text_field.text = null;
            BuildChoicePanel();

            // Ready for Interaction
            choice_package = ui_manager.choice_package;
            current_selection = choice_package.starting_choice;
            SetPointerStartPos();
            finished = false;
        }

        private void Update()
        {
            // Get player input
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (current_selection == choices.Length - 1)
                {
                    current_selection = 0;
                    MovePointerToTop();
                }
                    
                else
                {
                    current_selection += 1;
                    MovePointerDown();
                }
                    
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (current_selection == 0)
                {
                    current_selection = choices.Length - 1;
                    MovePointerToBottom();
                }
                    
                else
                {
                    current_selection -= 1;
                    MovePointerUp();
                }
                    
            }
            else if (Input.GetKeyDown(KeyCode.Z))
                MakeSelection(current_selection);
            else if (Input.GetKeyDown(KeyCode.X) && choice_package.cancel_choice >= 0)
                CancelOut();
        }

        #endregion


        #region Choice Methods

        private void MakeSelection(int selection)
        {
            chosen_selection = selection;
            finished = true;
        }

        private void CancelOut()
        {
            chosen_selection = choice_package.cancel_choice;
            finished = true;
        }

        private void BuildChoicePanel()
        {
            // Set Panel Height
            int num_choices = choices.Length;

            float ideal_panel_height = PANEL_HEIGHT_BASE + PANEL_HEIGHT_MULTIPLIER * num_choices;
            panel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ideal_panel_height);

            // Set Panel Width
            float longest_choice_width = 0;
            for (int i = 0; i < choices.Length; i++)
            {
                text_field.text = choices[i];

                float choice_width = text_field.preferredWidth;
                if (choice_width > longest_choice_width)
                    longest_choice_width = choice_width;

                text_field.text = null;
            }

            float ideal_panel_width = PANEL_WIDTH_BASE + PANEL_WIDTH_MULTIPLIER * longest_choice_width;
            float size_difference = ideal_panel_width - panel.rectTransform.rect.width;
            panel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ideal_panel_width);
            panel.rectTransform.Translate(Vector3.left * size_difference / 2);

            // Show options
            text_field.text = string.Join("\n", choices);
        }

        private void SetPointerStartPos()
        {
            for (int i = 0; i < current_selection; i++)
                MovePointerDown();
        }

        private void MovePointerUp()
        {
            sel_arrow.rectTransform.Translate(Vector3.up * PANEL_HEIGHT_MULTIPLIER);
            sel_arrow_white.rectTransform.Translate(Vector3.up * PANEL_HEIGHT_MULTIPLIER);
        }

        private void MovePointerDown()
        {
            sel_arrow.rectTransform.Translate(Vector3.down * PANEL_HEIGHT_MULTIPLIER);
            sel_arrow_white.rectTransform.Translate(Vector3.down * PANEL_HEIGHT_MULTIPLIER);
        }

        private void MovePointerToTop()
        {
            sel_arrow.rectTransform.Translate(Vector3.up * PANEL_HEIGHT_MULTIPLIER * (choices.Length - 1));
            sel_arrow_white.rectTransform.Translate(Vector3.up * PANEL_HEIGHT_MULTIPLIER * (choices.Length - 1));
        }

        private void MovePointerToBottom()
        {
            sel_arrow.rectTransform.Translate(Vector3.down * PANEL_HEIGHT_MULTIPLIER * (choices.Length - 1));
            sel_arrow_white.rectTransform.Translate(Vector3.down * PANEL_HEIGHT_MULTIPLIER * (choices.Length - 1));
        }

        #endregion

    }
}