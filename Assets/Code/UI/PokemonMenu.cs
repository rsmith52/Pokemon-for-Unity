using Items;
using Battle;
using Trainers;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using System.Collections;
using Eventing;
using System.Collections.Generic;
using System;
using Pokemon;

namespace UI
{
    public class PokemonMenu : MonoBehaviour
    {
        #region Enums

        private enum PartySelectChoices
        {
            Summary,
            Swap,
            Item,
            Cancel
        }

        #endregion


        #region Constants

        // Animations
        private static readonly float ICON_ANIM_TIME = 0.25f;
        private static readonly float SWAP_ANIMATION_DISTANCE = 500f;
        private static readonly float SWAP_ANIMATION_SPEED = 1000f;

        // Text
        private static readonly string WELCOME_MESSAGE = "Choose a Pokémon.";
        private static readonly string SELECT_MESSAGE = "Do what with this Pokémon?";
        private static readonly string SWAP_MESSAGE = "Move to where?";
        private static readonly string[] SELECT_OPTIONS = new string[]
        {
            "Summary",
            "Swap",
            "Item",
            "Cancel"
        };
        private static readonly string LEVEL_TEXT = "Lv.";
        private static readonly string HP_SEPERATOR_TEXT = "/ ";
        private static readonly string CANCEL_TEXT = "CANCEL";

        // HP Bars
        private static readonly float HP_BAR_MAX_WIDTH = 144f;
        private static readonly float HP_BAR_YELLOW_PERCENT = 0.5f;
        private static readonly float HP_BAR_RED_PERCENT = 0.2f;
        
        // Message and Menu Sizing
        private static readonly float MESSAGE_PANEL_FULL_WIDTH = 597f;
        private static readonly float MESSAGE_PANEL_SHORT_WIDTH = 550f;
        private static readonly float CHOICE_MENU_OFFSET = 124f;
        private static readonly Color32 CANCEL_TEXT_COLOR = Constants.DARK_BG_TEXT_COLOR;

        #endregion


        #region Fields

        private UIManager ui_manager;

        [Header("Menu Components")]
        public Image[] party_panels = new Image[Constants.PARTY_SIZE];
        public Image[] pokeballs = new Image[Constants.PARTY_SIZE];
        public Image[] icons = new Image[Constants.PARTY_SIZE];
        public Text[] names = new Text[Constants.PARTY_SIZE];
        public Text[] levels = new Text[Constants.PARTY_SIZE];
        public Text[] genders = new Text[Constants.PARTY_SIZE];
        public Image[] hp_backs = new Image[Constants.PARTY_SIZE];
        public Image[] hp_bars = new Image[Constants.PARTY_SIZE];
        public Text[] hp_texts = new Text[Constants.PARTY_SIZE];
        public Image[] items = new Image[Constants.PARTY_SIZE];
        public Image[] statuses = new Image[Constants.PARTY_SIZE];
        public Image message_panel;
        public Text message_text_field;
        public Image button;
        public Text button_text;

        private PlayerTrainer player;
        private Party party;
        private Pokemon.Pokemon temp_pokemon;

        private float anim_time;
        private int anim_frame;

        private int current_selection;
        private bool awaiting_input;
        public MonoBehaviour open_screen;

        private bool swapping;
        private int swap_selection;
        private int swap_selection2;
        private int swap_animation_stage;
        private int[] panels_to_move;
        private float distance_moved;

        #endregion


        #region References

        [Header("Party Panels")]
        public Sprite panel_empty;

        public Sprite panel_round;
        public Sprite panel_round_sel;
        public Sprite panel_round_faint;
        public Sprite panel_round_faint_sel;
        public Sprite panel_round_swap;
        public Sprite panel_round_swap_sel;
        public Sprite panel_round_swap_sel2;

        public Sprite panel_rect;
        public Sprite panel_rect_sel;
        public Sprite panel_rect_faint;
        public Sprite panel_rect_faint_sel;
        public Sprite panel_rect_swap;
        public Sprite panel_rect_swap_sel;
        public Sprite panel_rect_swap_sel2;

        [Header("Pokeball Decoration")]
        public Sprite pokeball;
        public Sprite pokeball_sel;

        [Header("HP Bars")]
        public Sprite hp_back;
        public Sprite hp_back_faint;
        public Sprite hp_back_swap;

        public Sprite hp_bar_green;
        public Sprite hp_bar_yellow;
        public Sprite hp_bar_red;

        [Header("Items")]
        public Sprite held_item;
        public Sprite held_mail;

        [Header("Statuses")]
        public Sprite status_burn;
        public Sprite status_freeze;
        public Sprite status_paralysis;
        public Sprite status_poison;
        public Sprite status_sleep;
        public Sprite status_faint;

        [Header("Buttons")]
        public Sprite cancel_button;
        public Sprite cancel_button_sel;

        #endregion


        #region MonoBehavior

        private void Start()
        {
            // Get references
            ui_manager = FindObjectOfType<UIManager>();

            // Get current party info
            player = ui_manager.GetPlayerTrainer();
            party = player.party;

            // Set windowskin and text color
            message_panel.sprite = ui_manager.GetCurrentMenuSkin();
            message_text_field.color = ui_manager.GetBestTextColor(message_panel);
            button_text.color = CANCEL_TEXT_COLOR;

            // Show pokemon info and build rest of scene
            BuildPokemonPanels();
            message_text_field.text = WELCOME_MESSAGE;
            button_text.text = CANCEL_TEXT;

            // Ready for interaction
            anim_time = 0;
            anim_frame = 0;
            temp_pokemon = null;
            current_selection = 0;
            swapping = false;
            awaiting_input = true;
        }

        private void Update()
        {
            // Get player input
            if (awaiting_input)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (party.size == 1 && current_selection == 0)
                        current_selection = -1;
                    else if (current_selection == party.size - 1)
                        current_selection = 0;
                    else
                        current_selection += 1;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (party.size == 1 && current_selection == 0)
                        current_selection = -1;
                    else if (current_selection <= 0)
                        current_selection = party.size - 1;
                    else
                        current_selection -= 1;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (current_selection == -1)
                        current_selection = 0;
                    else if (party.size - 1 - current_selection < 2)
                        current_selection = -1;
                    else if (current_selection + 2 > (party.size - 1))
                        current_selection += 1;
                    else
                        current_selection += 2;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (current_selection == -1)
                        current_selection = party.size - 1;
                    else if (current_selection - 2 < 0)
                        current_selection = -1;
                    else
                        current_selection -= 2;
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (swapping && current_selection == -1)
                        EndSwap();
                    else if (current_selection == -1)
                        CloseMenu();
                    else if (swapping && current_selection == swap_selection)
                        EndSwap();
                    else if (swapping)
                        StartCoroutine(PerformSwap(current_selection));
                    else
                        StartCoroutine(ShowPokemonChoices(party.pokemon[current_selection]));
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    if (swapping)
                        EndSwap();
                    else
                        CloseMenu();
                }
            }

            // Animate pokemon_icons
            anim_time += Time.deltaTime;
            // Keep icons animated
            if (anim_time > ICON_ANIM_TIME)
            {
                anim_time = 0;
                anim_frame = (anim_frame + 1) % 2;
                for (int i = 0; i < Constants.PARTY_SIZE; i++)
                    if (party.slot_filled[i])
                        icons[i].sprite = Specie.GetPokemonIcon((uint)party.pokemon[i].species, anim_frame, (uint)party.pokemon[i].form_id);
            }

            // Advance swapping animation
            if (swap_animation_stage > 0)
            {
                for (int i = 0; i < panels_to_move.Length; i++)
                {
                    if (panels_to_move[i] % 2 == 0 && swap_animation_stage == 1 ||
                        panels_to_move[i] % 2 == 1 && swap_animation_stage == 2)
                        MovePanelLeft(panels_to_move[i]);
                    else
                        MovePanelRight(panels_to_move[i]);
                }
                if (swap_animation_stage == 1)
                    distance_moved += SWAP_ANIMATION_SPEED * Time.deltaTime;
                else
                    distance_moved -= SWAP_ANIMATION_SPEED * Time.deltaTime;
            }

            // Update panels
            UpdatePokemonPanels();
            UpdateCancelButton();
        }

        #endregion


        #region Menu Building and Updating

        private void BuildPokemonPanels()
        {
            for (int i = 0; i < Constants.PARTY_SIZE; i++)
            {
                // Empty slot
                if (!party.slot_filled[i])
                {
                    party_panels[i].sprite = panel_empty;
                    pokeballs[i].enabled = false;
                    icons[i].enabled = false;
                    names[i].enabled = false;
                    levels[i].enabled = false;
                    genders[i].enabled = false;
                    hp_backs[i].enabled = false;
                    hp_bars[i].enabled = false;
                    hp_texts[i].enabled = false;
                    items[i].enabled = false;
                    statuses[i].enabled = false;
                }
                // Fainted slot
                else if (party.pokemon[i].current_HP == 0)
                {
                    if (current_selection == i)
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round_faint_sel;
                        else
                            party_panels[i].sprite = panel_rect_faint_sel;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball_sel;
                    }
                    else
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round_faint;
                        else
                            party_panels[i].sprite = panel_rect_faint;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball;
                    }
                    icons[i].enabled = true;
                    icons[i].sprite = Specie.GetPokemonIcon((uint)party.pokemon[i].species, 0, (uint)party.pokemon[i].form_id);
                    names[i].enabled = true;
                    names[i].text = party.pokemon[i].GetName();
                    levels[i].enabled = true;
                    levels[i].text = LEVEL_TEXT + party.pokemon[i].level.ToString();
                    genders[i].enabled = true;
                    genders[i].text = party.pokemon[i].GetGenderText();
                    hp_backs[i].enabled = true;
                    hp_backs[i].sprite = hp_back_faint;
                    hp_bars[i].enabled = false;
                    hp_texts[i].enabled = false;
                    Items.Items item = party.pokemon[i].held_item;
                    items[i].enabled = false;
                    if (item != Items.Items.None)
                    {
                        items[i].enabled = true;
                        items[i].sprite = GetItemSprite(item);
                    }
                    statuses[i].enabled = true;
                    statuses[i].sprite = status_faint;
                }
                // Normal slot
                else
                {
                    if (current_selection == i)
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round_sel;
                        else
                            party_panels[i].sprite = panel_rect_sel;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball_sel;
                    }
                    else
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round;
                        else
                            party_panels[i].sprite = panel_rect;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball;
                    }
                    icons[i].enabled = true;
                    icons[i].sprite = Specie.GetPokemonIcon((uint)party.pokemon[i].species, 0, (uint)party.pokemon[i].form_id);
                    names[i].enabled = true;
                    names[i].text = party.pokemon[i].GetName();
                    levels[i].enabled = true;
                    levels[i].text = LEVEL_TEXT + party.pokemon[i].level.ToString();
                    genders[i].enabled = true;
                    genders[i].text = party.pokemon[i].GetGenderText();
                    hp_backs[i].enabled = true;
                    hp_backs[i].sprite = hp_back;
                    hp_bars[i].enabled = true;
                    float hp_percent = party.pokemon[i].GetCurretHPPercent();
                    hp_bars[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hp_percent * HP_BAR_MAX_WIDTH);
                    hp_bars[i].sprite = GetHPBarSprite(hp_percent);
                    hp_texts[i].enabled = true;
                    hp_texts[i].text = party.pokemon[i].current_HP.ToString() + HP_SEPERATOR_TEXT + party.pokemon[i].stats.HP.ToString();
                    Items.Items item = party.pokemon[i].held_item;
                    items[i].enabled = false;
                    if (item != Items.Items.None)
                    {
                        items[i].enabled = true;
                        items[i].sprite = GetItemSprite(item);
                    }
                    Statuses status = party.pokemon[i].status;
                    statuses[i].enabled = false;
                    if (status != Statuses.None)
                    {
                        statuses[i].enabled = true;
                        statuses[i].sprite = GetStatusSprite(status);
                    }
                }
            }
        }

        private void UpdatePokemonPanels()
        {
            for (int i = 0; i < Constants.PARTY_SIZE; i++)
            {
                // Empty slot
                if (!party.slot_filled[i])
                    break;
                // Swap selection slot
                else if (swapping && swap_selection == i)
                {
                    if (current_selection == i)
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round_swap_sel;
                        else
                            party_panels[i].sprite = panel_rect_swap_sel;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball_sel;
                    }
                    else
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round_swap;
                        else
                            party_panels[i].sprite = panel_rect_swap;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball;
                    }
                }
                // 2nd Swap selection slot
                else if (swapping && swap_selection2 == i)
                {
                    // Note: current selection will always be this panel
                    if (i == 0)
                        party_panels[i].sprite = panel_round_swap_sel2;
                    else
                        party_panels[i].sprite = panel_rect_swap_sel2;
                    pokeballs[i].enabled = true;
                    pokeballs[i].sprite = pokeball_sel;
                }
                // Fainted slot
                else if (party.pokemon[i].current_HP == 0)
                {
                    if (current_selection == i)
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round_faint_sel;
                        else
                            party_panels[i].sprite = panel_rect_faint_sel;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball_sel;
                    }
                    else
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round_faint;
                        else
                            party_panels[i].sprite = panel_rect_faint;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball;
                    }
                }
                // Normal slot
                else
                {
                    if (current_selection == i)
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round_sel;
                        else
                            party_panels[i].sprite = panel_rect_sel;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball_sel;
                    }
                    else
                    {
                        if (i == 0)
                            party_panels[i].sprite = panel_round;
                        else
                            party_panels[i].sprite = panel_rect;
                        pokeballs[i].enabled = true;
                        pokeballs[i].sprite = pokeball;
                    }
                }
            }
        }

        private void UpdateCancelButton()
        {
            if (current_selection < 0)
                button.sprite = cancel_button_sel;
            else
                button.sprite = cancel_button;
        }

        private Sprite GetHPBarSprite(float hp_percent)
        {
            if (hp_percent <= HP_BAR_RED_PERCENT)
                return hp_bar_red;
            else if (hp_percent <= HP_BAR_YELLOW_PERCENT)
                return hp_bar_yellow;
            else return hp_bar_green;
        }

        private Sprite GetItemSprite(Items.Items item)
        {
            if (Item.items[item].special_item_type == SpecialItemTypes.Mail || Item.items[item].special_item_type == SpecialItemTypes.MailWithImages)
                return held_mail;
            else return held_item;
        }

        private Sprite GetStatusSprite(Statuses status)
        {
            switch (status)
            {
                case Statuses.Burn:
                    return status_burn;
                case Statuses.Freeze:
                    return status_freeze;
                case Statuses.Paralysis:
                    return status_paralysis;
                case Statuses.Poison:
                    return status_poison;
                case Statuses.Sleep:
                    return status_sleep;
                default:
                    return null;
            }
        }

        #endregion


        #region Pokemon Menu Methods

        private void CloseMenu()
        {
            GameObject.Destroy(this.gameObject);
        }
        

        private IEnumerator ShowPokemonChoices(Pokemon.Pokemon pokemon)
        {
            // Lock control and modify view
            awaiting_input = false;
            message_panel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MESSAGE_PANEL_SHORT_WIDTH);
            message_text_field.text = SELECT_MESSAGE;

            // Determine choices to show
            List<string> valid_choices = new List<string>();
            valid_choices.Add(SELECT_OPTIONS[0]);
            if (party.size > 1)
                valid_choices.Add(SELECT_OPTIONS[1]);
            if (pokemon.egg_steps == 0)
                valid_choices.Add(SELECT_OPTIONS[2]);
            valid_choices.Add(SELECT_OPTIONS[3]);
            ChoicePackage choice_package = new ChoicePackage(valid_choices.ToArray(), 0, valid_choices.Count - 1);

            // Show choices
            Choice choice = ui_manager.ShowChoices(choice_package);
            Image choice_panel = choice.GetComponent<Image>();
            choice_panel.rectTransform.Translate(Vector3.down * CHOICE_MENU_OFFSET);

            // Hide choices
            yield return new WaitUntil(() => choice.finished);
            PartySelectChoices selection = (PartySelectChoices)Enum.Parse(typeof(PartySelectChoices), valid_choices[choice.chosen_selection], true);
            GameObject.Destroy(choice.gameObject);

            // Act on selection
            switch (selection)
            {
                case PartySelectChoices.Summary:
                    Debug.Log("Showing Summary");
                    open_screen = ui_manager.ShowSummaryScreen();
                    SummaryScreen summary = open_screen.GetComponent<SummaryScreen>();
                    summary.pokemon_selection = current_selection;
                    summary.in_pc = false;
                    summary.learning_move = false;
                    yield return new WaitUntil(() => open_screen == null);
                    StartCoroutine(ShowPokemonChoices(pokemon));
                    break;
                case PartySelectChoices.Swap:
                    InitiateSwap(current_selection);
                    break;
                case PartySelectChoices.Item:
                    Debug.Log("Open bag to give item");
                    EndPokemonChoices(); // TODO: replace this with giving item
                    break;
                default:
                    EndPokemonChoices();
                    break;
            }
        }

        private void EndPokemonChoices()
        {
            // Set back to original view and enable control
            message_panel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MESSAGE_PANEL_FULL_WIDTH);
            message_text_field.text = WELCOME_MESSAGE;
            awaiting_input = true;
        }

        #endregion


        #region Pokemon Swapping Methods

        private void InitiateSwap(int selection)
        {
            // Set view to swap
            message_panel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MESSAGE_PANEL_FULL_WIDTH);
            message_text_field.text = SWAP_MESSAGE;
            swapping = true;
            swap_selection = selection;
            swap_selection2 = -1;
            awaiting_input = true;
        }

        private IEnumerator PerformSwap(int selection)
        {
            swap_selection2 = selection;
            panels_to_move = new int[] { swap_selection, swap_selection2 };
            swap_animation_stage = 1;
            awaiting_input = false;

            // Wait for panels to move off screen
            yield return new WaitUntil(() => distance_moved >= SWAP_ANIMATION_DISTANCE);

            // Perform swap
            temp_pokemon = party.pokemon[swap_selection];
            party.pokemon[swap_selection] = party.pokemon[swap_selection2];
            party.pokemon[swap_selection2] = temp_pokemon;
            BuildPokemonPanels();
            swap_animation_stage = 2;

            // Wait for panels to be back in place
            yield return new WaitUntil(() => distance_moved <= 0);
            swap_animation_stage = 0;

            EndSwap();
        }

        private void MovePanelLeft(int selection)
        {
            party_panels[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            pokeballs[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            icons[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            names[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            levels[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            genders[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            hp_backs[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            hp_bars[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            hp_texts[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            items[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
            statuses[selection].rectTransform.Translate(Vector3.left * SWAP_ANIMATION_SPEED * Time.deltaTime);
        }

        private void MovePanelRight(int selection)
        {
            party_panels[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            pokeballs[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            icons[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            names[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            levels[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            genders[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            hp_backs[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            hp_bars[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            hp_texts[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            items[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
            statuses[selection].rectTransform.Translate(Vector3.right * SWAP_ANIMATION_SPEED * Time.deltaTime);
        }

        private void EndSwap()
        {
            message_text_field.text = WELCOME_MESSAGE;
            swapping = false;
            awaiting_input = true;
        }

        #endregion

    }
}