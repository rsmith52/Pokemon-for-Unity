using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class Message : MonoBehaviour
    {
        #region Fields

        private UIManager ui_manager;

        [Header("Message Components")]
        public Image panel;
        public Text text_field;
        public Image pause_arrow;

        private float text_field_width;
        private string message;
        private string[] message_lines;
        private int num_lines;
        private int num_lines_written;

        private float speed;
        private bool awaiting_action;

        public bool writing;
        public bool on_final_line;
        public bool finished;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            // Get References
            ui_manager = FindObjectOfType<UIManager>();

            // Set Windowskin and Text Color
            panel.sprite = ui_manager.GetCurrentMessageSkin();
            text_field.color = ui_manager.GetBestTextColor(panel);

            // Initialize View Settings
            text_field_width = text_field.rectTransform.rect.width;
            message = text_field.text;
            pause_arrow.enabled = false;
            text_field.text = null;

            // Divide Message
            message_lines = BreakUpMessage();
            num_lines = message_lines.Length;
            num_lines_written = 0;

            // Ready for Interaction
            speed = Constants.TEXT_SPEEDS[(int)ui_manager.text_speed];
            awaiting_action = false;
            writing = false;
            on_final_line = false;
            finished = false;
        }

        private void Update()
        {
            // Ready to show next lines
            if (!awaiting_action && num_lines_written < num_lines)
            {
                WriteNextLines();
                pause_arrow.enabled = false;
                writing = true;
                awaiting_action = true;
            }
            // All lines shown
            else if (!awaiting_action)
            {
                finished = true;
            }
            // Awaiting player input
            else if (!writing && Input.GetKeyDown(KeyCode.Z))
            {
                awaiting_action = false;
            }

            // Allow speeding up of text printing
            if (writing && Input.GetKey(KeyCode.X))
                speed = Constants.TEXT_FAST_FORWARD_SPEED;
            else
                speed = Constants.TEXT_SPEEDS[(int)ui_manager.GetTextSpeed()];
        }

        #endregion


        #region Message Methods

        private void WriteNextLines()
        {
            int lines_left = num_lines - num_lines_written;
            string next_line = message_lines[num_lines_written];
            string next_next_line = null;
            if (lines_left < 2)
            {
                num_lines_written += 1;
                // WriteLines(next_line);
                StartCoroutine(WriteLinesByCharacter(next_line));
            }
            else
            {
                next_next_line = message_lines[num_lines_written + 1];
                num_lines_written += 2;
                // WriteLines(next_line + "\n" + next_next_line);
                StartCoroutine(WriteLinesByCharacter(next_line + "\n" + next_next_line));
            }

            lines_left = num_lines - num_lines_written;
            if (lines_left == 0)
                on_final_line = true;
        }

        IEnumerator WriteLinesByCharacter(string lines)
        {
            Char[] chars = lines.ToCharArray();

            string written_message = "";
            foreach (Char c in chars)
            {
                written_message += c;
                text_field.text = written_message;
                yield return new WaitForSeconds(1 / speed);
            }

            if (num_lines_written < num_lines)
                pause_arrow.enabled = true;
            writing = false;
        }

        private string[] BreakUpMessage()
        {
            List<string> lines = new List<string>();

            // Split message by words
            string[] words = message.Split(' ');
            int num_words = words.Length;

            // Get size of a space in current context
            text_field.text = " ";
            float space_width = text_field.preferredWidth;
            text_field.text = null;

            // Build lines word by word
            string current_line = "";
            float current_line_width = 0f;
            //Stack<string> current_markup = new Stack<string>();
            for (int i = 0; i < num_words; i++)
            {
                string word = words[i];

                // TODO: Check for line breaks, tabs, pauses, etc.

                // Check for replacement codes {CODE} such as player name: {PN}
                word = ui_manager.ReplaceTextCode(word);

                // Get length of this word
                text_field.text = word;
                float word_width = text_field.preferredWidth + space_width;
                text_field.text = null;

                // Add word to current line if it fits
                if (current_line_width + word_width <= text_field_width)
                {
                    current_line += word + " ";
                    current_line_width += word_width;
                }
                // Otherwise add the finished line and start a new one
                else
                {
                    lines.Add(current_line);
                    current_line = word + " ";
                    current_line_width = word_width;
                }
            }

            // Add final line and return all lines
            lines.Add(current_line);
            return lines.ToArray();
        }

        #endregion

    }
}