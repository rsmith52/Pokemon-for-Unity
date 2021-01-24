using System;
using Trainers;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Items;
using Pokemon;

namespace UI
{
    public class SummaryScreen : MonoBehaviour
    {
        #region Enums

        public enum SummaryPages
        {
            Info,
            TrainerMemo,
            Skills,
            Moves,
            Ribbons,
            Egg,
            MoveDetail,
            LearnMove
        }

        #endregion


        #region Constants

        private static readonly string[] SUMMARY_PAGES = new string[]
        {
            "INFO",
            "TRAINER MEMO",
            "SKILLS",
            "MOVES",
            "RIBBONS"
        };
        private static readonly string EGG_PAGE = SUMMARY_PAGES[1];
        private static readonly string MOVE_DETAIL_PAGE = SUMMARY_PAGES[3];
        private static readonly string LEARN_MOVE_PAGE = SUMMARY_PAGES[3];
        private static readonly string ITEM_TEXT = "Item";
        private static readonly string[] INFO_LABELS = new string[]
        {
            "Dex No.", "Species", "Type", "OT", "ID No.", "Exp. Points", "", "To Next Lv."
        };
        private static readonly string NATURE_TEXT = "nature";
        private static readonly string OBTAIN_LEVEL_TEXT = "Met at Lv. ";
        private static readonly string[] SKILLS_TEXT = new string[]
        {
            "HP", "Attack", "Defense", "Sp. Atk", "Sp. Def", "Speed"
        };
        private static readonly string HP_SEPERATOR_TEXT = "/ ";
        private static readonly string ABILITY_TEXT = "Ability";
        private static readonly string PP_TEXT = "PP";
        private static readonly string PP_SEPERATOR_TEXT = "/";
        private static readonly string NUM_RIBBONS_TEXT = "No. of Ribbons:";

        private static readonly Color32 DARK_BG_COLOR = Constants.DARK_BG_TEXT_COLOR;
        private static readonly Color32 LIGHT_BG_COLOR = Constants.LIGHT_BG_TEXT_COLOR;

        private static readonly float EXP_BAR_MAX_WIDTH = 400f;

        private static readonly float HP_BAR_MAX_WIDTH = 300f;
        private static readonly float HP_BAR_YELLOW_PERCENT = 0.5f;
        private static readonly float HP_BAR_RED_PERCENT = 0.2f;

        private static readonly int NUM_RIBBONS = 12;

        #endregion


        #region Fields

        private UIManager ui_manager;

        [Header("General Components")]
        public Image ball_icon;
        public Text pokemon_name;
        public Text gender;
        public Text level;
        public Image pokemon_sprite;
        public Image[] markings = new Image[Constants.NUM_MARKINGS];
        public Image item_icon;
        public Text item;
        public Text item_name;

        [Header("Info Screen")]
        public Image info_bg;
        public Text info_title;
        public Text info_labels;
        public Text info_dex_number;
        public Text info_species;
        public Image info_one_type;
        public Image[] info_types = new Image[2];
        public Text info_trainer;
        public Text info_personal_id;
        public Text info_exp;
        public Text info_exp_to_level;
        public Image info_exp_bar;

        [Header("Trainer Memo Screen")]
        public Image trainer_bg;
        public Text trainer_title;
        public Text trainer_memo;

        [Header("Skills Screen")]
        public Image skills_bg;
        public Text skills_title;
        public Text skills_hp_label;
        public Text skills_hp;
        public Image skills_hp_bar;
        public Text skills_labels;
        public Text skills_values;
        public Text skills_ability;
        public Text skills_ability_name;
        public Text skills_ability_desc;

        [Header("Moves Screen")]
        public Image moves_bg;
        public Text moves_title;
        public Image[] moves_types = new Image[Constants.NUM_MOVES];
        public Text[] moves_names = new Text[Constants.NUM_MOVES];
        public Text[] moves_pp_labels = new Text[Constants.NUM_MOVES];
        public Text[] moves_pp = new Text[Constants.NUM_MOVES];

        [Header("Ribbons Screen")]
        public Image ribbons_bg;
        public Text ribbons_title;
        public Image[] ribbons_sprites = new Image[NUM_RIBBONS];
        public Text ribbons_count;
        public Text ribbons_count_num;

        [Header("Egg Screen")]
        public Image egg_bg;

        [Header("Move Detail Screen")]
        public Image move_detail_bg;

        [Header("Learn Move Screen")]
        public Image learn_move_bg;

        [Header("References")]
        public Sprite[] pokeballs = new Sprite[Enum.GetNames(typeof(Pokeballs)).Length];
        public Sprite[] types = new Sprite[Enum.GetNames(typeof(Pokemon.Types)).Length];
        public Sprite[] markings_unsel = new Sprite[Constants.NUM_MARKINGS];
        public Sprite[] markings_sel = new Sprite[Constants.NUM_MARKINGS];
        public Sprite hp_bar_green;
        public Sprite hp_bar_yellow;
        public Sprite hp_bar_red;
        public Sprite[] ribbons = new Sprite[Constants.NUM_RIBBONS];

        private PlayerTrainer player;
        private Party party;

        private SummaryPages current_page;
        private Pokemon.Pokemon current_pokemon;
        public int pokemon_selection;
        public bool in_pc;
        public bool learning_move;

        private bool page_changed;
        private bool awaiting_input;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            // Get references
            ui_manager = FindObjectOfType<UIManager>();
            player = ui_manager.GetPlayerTrainer();
            party = player.party;

            // Set starting page
            HideAllPages();
            current_page = SummaryPages.Info;

            // Ready for interaction
            page_changed = true;
            awaiting_input = true;
        }

        private void Update()
        {
            // Get player input
            if (awaiting_input)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    HidePage(current_page);
                    if ((int)current_page == SUMMARY_PAGES.Length - 1)
                        current_page = (SummaryPages)0;
                    else
                        current_page += 1;
                    page_changed = true;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    HidePage(current_page);
                    if ((int)current_page == 0)
                        current_page = (SummaryPages)(SUMMARY_PAGES.Length - 1);
                    else
                        current_page -= 1;
                    page_changed = true;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    HidePage(current_page);
                    if (pokemon_selection == party.size - 1)
                        pokemon_selection = 0;
                    else
                        pokemon_selection += 1;
                    page_changed = true;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    HidePage(current_page);
                    if (pokemon_selection == 0)
                        pokemon_selection = party.size - 1;
                    else
                        pokemon_selection -= 1;
                    page_changed = true;
                }
                else if (Input.GetKeyDown(KeyCode.X))
                    CloseMenu();
            }

            // Build/Show current page
            if (page_changed)
                BuildCurrentPage();
        }

        #endregion


        #region Screen Building and Updating

        private void BuildCurrentPage()
        {
            // Get current pokemon
            if (!in_pc)
                current_pokemon = party.pokemon[pokemon_selection];

            switch (current_page)
            {
                case SummaryPages.Info:
                    BuildInfoScreen(current_pokemon);
                    break;
                case SummaryPages.TrainerMemo:
                    BuildTrainerMemoScreen(current_pokemon);
                    break;
                case SummaryPages.Skills:
                    BuildSkillsScreen(current_pokemon);
                    break;
                case SummaryPages.Moves:
                    BuildMovesScreen(current_pokemon);
                    break;
                case SummaryPages.Ribbons:
                    BuildRibbonsScreen(current_pokemon);
                    break;
                default:
                    break;
            }

            page_changed = false;
        }

        private void HidePage(SummaryPages page)
        {
            switch (page)
            {
                case SummaryPages.Info:
                    HideInfoScreen();
                    break;
                case SummaryPages.TrainerMemo:
                    HideTrainerMemoScreen();
                    break;
                case SummaryPages.Skills:
                    HideSkillsScreen();
                    break;
                case SummaryPages.Moves:
                    HideMovesScreen();
                    break;
                case SummaryPages.Ribbons:
                    HideRibbonsScreen();
                    break;
                default:
                    break;
            }
        }

        private void HideAllPages()
        {
            for (int i = 0; i < Enum.GetValues(typeof(SummaryPages)).Length; i++)
                HidePage((SummaryPages)i);
        }

        private void BuildGeneralComponents(Pokemon.Pokemon pokemon)
        {
            ball_icon.enabled = true;
            ball_icon.sprite = pokeballs[(int)(Pokeballs)Enum.Parse(typeof(Pokeballs), pokemon.ball_used.ToString(), true)];
            pokemon_name.enabled = true;
            pokemon_name.text = pokemon.GetName();
            gender.enabled = true;
            gender.text = pokemon.GetGenderText();
            level.enabled = true;
            level.text = pokemon.level.ToString();
            pokemon_sprite.enabled = true;
            Specie species = pokemon.GetSpecieData();
            pokemon_sprite.sprite = Specie.GetPokemonSprite(species.national_dex, pokemon.gender, pokemon.is_shiny, false, 0, pokemon.form_id);
            for (int i = 0; i < Constants.NUM_MARKINGS; i++)
            {
                markings[i].enabled = true;
                if (pokemon.markings[i])
                    markings[i].sprite = markings_sel[i];
                else
                    markings[i].sprite = markings_unsel[i];
            }
            item.enabled = true;
            item.text = ITEM_TEXT;
            if (pokemon.held_item != Items.Items.None)
            {
                item_icon.enabled = true;
                item_icon.sprite = Item.GetItemIcon(Item.items[pokemon.held_item].item_id);
                item_name.enabled = true;
                item_name.text = Item.items[pokemon.held_item].name;
            }
            else
            {
                item_icon.enabled = false;
                item_name.enabled = false;
            }
        }

        private void HideGeneralComponents()
        {
            ball_icon.enabled = false;
            pokemon_name.enabled = false;
            gender.enabled = false;
            level.enabled = false;
            pokemon_sprite.enabled = false;
            for (int i = 0; i < Constants.NUM_MARKINGS; i++)
                markings[i].enabled = false;
            item.enabled = false;
            item_icon.enabled = false;
            item_name.enabled = false;
        }

        private void BuildInfoScreen(Pokemon.Pokemon pokemon)
        {
            // Background and Title
            info_bg.enabled = true;
            info_title.enabled = true;
            info_title.text = SUMMARY_PAGES[(int)SummaryPages.Info];

            // General Components
            BuildGeneralComponents(pokemon);

            // Info Components
            info_labels.enabled = true;
            info_labels.text = "";
            for (int i = 0; i < INFO_LABELS.Length; i++)
                info_labels.text += INFO_LABELS[i] + '\n';
            info_dex_number.enabled = true;
            info_dex_number.text = pokemon.GetSpecieData().national_dex.ToString().PadLeft(3, '0');
            info_species.enabled = true;
            info_species.text = pokemon.GetSpecieData().name;
            if (pokemon.types[1] == Pokemon.Types.None)
            {
                info_one_type.enabled = true;
                info_one_type.sprite = types[(int)pokemon.types[0]];
                info_types[0].enabled = false;
                info_types[1].enabled = false;
            }
            else
            {
                info_one_type.enabled = false;
                info_types[0].enabled = true;
                info_types[0].sprite = types[(int)pokemon.types[0]];
                info_types[1].enabled = true;
                info_types[1].sprite = types[(int)pokemon.types[1]];
            }
            info_trainer.enabled = true;
            info_trainer.text = pokemon.GetTrainerNameText();
            info_personal_id.enabled = true;
            info_personal_id.text = pokemon.GetPublicPersonalIDText();
            info_exp.enabled = true;
            info_exp.text = pokemon.exp.ToString();
            info_exp_to_level.enabled = true;
            info_exp_to_level.text = Experience.GetExpToNextLevel(pokemon.exp, pokemon.GetSpecieData().growth_rate).ToString();
            info_exp_bar.enabled = true;
            float exp_percent = Experience.GetExpToNextLevelPercent(pokemon.exp, pokemon.GetSpecieData().growth_rate);
            info_exp_bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, exp_percent * EXP_BAR_MAX_WIDTH);
        }

        private void HideInfoScreen()
        {
            info_bg.enabled = false;
            info_title.enabled = false;
            info_labels.enabled = false;
            info_dex_number.enabled = false;
            info_species.enabled = false;
            info_one_type.enabled = false;
            info_types[0].enabled = false;
            info_types[1].enabled = false;
            info_trainer.enabled = false;
            info_personal_id.enabled = false;
            info_exp.enabled = false;
            info_exp_to_level.enabled = false;
            info_exp_bar.enabled = false;
        }

        private void BuildTrainerMemoScreen(Pokemon.Pokemon pokemon)
        {
            // Background and Title
            trainer_bg.enabled = true;
            trainer_title.enabled = true;
            trainer_title.text = SUMMARY_PAGES[(int)SummaryPages.TrainerMemo];

            // General Components
            BuildGeneralComponents(pokemon);

            // Trainer Memo Components
            trainer_memo.enabled = true;
            trainer_memo.text = "";
            trainer_memo.text += "<color=red>" + pokemon.nature.ToString() + "</color> " + NATURE_TEXT + ".\n";
            trainer_memo.text += pokemon.GetDateText() + '\n';
            if (pokemon.obtain_text != null)
                trainer_memo.text += pokemon.obtain_text + '\n';
            else
                trainer_memo.text += pokemon.GetMapText() + '\n';
            trainer_memo.text += OBTAIN_LEVEL_TEXT + pokemon.obtain_level.ToString() + ".\n";
            trainer_memo.text += '\n';
            trainer_memo.text += pokemon.GetCharacteristic() + '.';
        }

        private void HideTrainerMemoScreen()
        {
            trainer_bg.enabled = false;
            trainer_title.enabled = false;
            trainer_memo.enabled = false;
        }

        private void BuildSkillsScreen(Pokemon.Pokemon pokemon)
        {
            // Background and Title
            skills_bg.enabled = true;
            skills_title.enabled = true;
            skills_title.text = SUMMARY_PAGES[(int)SummaryPages.Skills];

            // General Components
            BuildGeneralComponents(pokemon);

            // Skills Components
            skills_hp_label.enabled = true;
            skills_hp_label.text = SKILLS_TEXT[0];
            skills_hp.enabled = true;
            skills_hp.text = pokemon.current_HP.ToString() + HP_SEPERATOR_TEXT + pokemon.stats.HP.ToString();
            skills_hp_bar.enabled = true;
            float hp_percent = pokemon.GetCurretHPPercent();
            skills_hp_bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hp_percent * HP_BAR_MAX_WIDTH);
            skills_hp_bar.sprite = GetHPBarSprite(hp_percent);
            skills_labels.enabled = true;
            skills_labels.text = "";
            for (int i = 1; i < SKILLS_TEXT.Length; i++)
                skills_labels.text += SKILLS_TEXT[i] + '\n';
            skills_values.enabled = true;
            skills_values.text = "";
            skills_values.text += pokemon.stats.ATK.ToString() + '\n';
            skills_values.text += pokemon.stats.DEF.ToString() + '\n';
            skills_values.text += pokemon.stats.SP_ATK.ToString() + '\n';
            skills_values.text += pokemon.stats.SP_DEF.ToString() + '\n';
            skills_values.text += pokemon.stats.SPD.ToString();
            skills_ability.enabled = true;
            skills_ability.text = ABILITY_TEXT;
            skills_ability_name.enabled = true;
            skills_ability_name.text = Ability.abilities[pokemon.ability].name;
            skills_ability_desc.enabled = true;
            skills_ability_desc.text = Ability.abilities[pokemon.ability].description;
        }

        private void HideSkillsScreen()
        {
            skills_bg.enabled = false;
            skills_title.enabled = false;
            skills_hp_label.enabled = false;
            skills_hp.enabled = false;
            skills_hp_bar.enabled = false;
            skills_labels.enabled = false;
            skills_values.enabled = false;
            skills_ability.enabled = false;
            skills_ability_name.enabled = false;
            skills_ability_desc.enabled = false;
        }

        private void BuildMovesScreen(Pokemon.Pokemon pokemon)
        {
            // Background and Title
            moves_bg.enabled = true;
            moves_title.enabled = true;
            moves_title.text = SUMMARY_PAGES[(int)SummaryPages.Moves];

            // General Components
            BuildGeneralComponents(pokemon);

            // Moves Components
            for (int i = 0; i < Constants.NUM_MOVES; i++)
            {
                if (pokemon.moves[i].move == Moves.None)
                {
                    moves_types[i].enabled = false;
                    moves_names[i].enabled = false;
                    moves_pp_labels[i].enabled = false;
                    moves_pp[i].enabled = false;
                }
                else
                {
                    MoveSlot move = pokemon.moves[i];
                    moves_types[i].enabled = true;
                    moves_types[i].sprite = types[(int)move.type];
                    moves_names[i].enabled = true;
                    moves_names[i].text = move.name;
                    moves_pp_labels[i].enabled = true;
                    moves_pp_labels[i].text = PP_TEXT;
                    moves_pp[i].enabled = true;
                    moves_pp[i].text = pokemon.moves[i].remaining_PP.ToString() + PP_SEPERATOR_TEXT + pokemon.moves[i].total_pp.ToString();
                }
            }
    }

        private void HideMovesScreen()
        {
            moves_bg.enabled = false;
            moves_title.enabled = false;
            for (int i = 0; i < Constants.NUM_MOVES; i++)
            {
                moves_types[i].enabled = false;
                moves_names[i].enabled = false;
                moves_pp_labels[i].enabled = false;
                moves_pp[i].enabled = false;
            }
        }

        private void BuildRibbonsScreen(Pokemon.Pokemon pokemon)
        {
            // Background and Title
            ribbons_bg.enabled = true;
            ribbons_title.enabled = true;
            ribbons_title.text = SUMMARY_PAGES[(int)SummaryPages.Ribbons];

            // General Components
            BuildGeneralComponents(pokemon);

            // Ribbons Components
            int[] poke_ribbons = pokemon.GetRibbonSet(NUM_RIBBONS, 0);

            int num_ribbons = 0;
            for (int i = 0; i < NUM_RIBBONS; i++)
            {
                if (poke_ribbons[i] < 0)
                    ribbons_sprites[i].enabled = false;
                else
                {
                    ribbons_sprites[i].enabled = true;
                    ribbons_sprites[i].sprite = ribbons[poke_ribbons[i]];
                    num_ribbons++;
                }
            }
            ribbons_count.enabled = true;
            ribbons_count.text = NUM_RIBBONS_TEXT;
            ribbons_count_num.enabled = true;
            ribbons_count_num.text = pokemon.GetNumRibbons().ToString();
        }

        private void HideRibbonsScreen()
        {
            ribbons_bg.enabled = false;
            ribbons_title.enabled = false;
            for (int i = 0; i < NUM_RIBBONS; i++)
                ribbons_sprites[i].enabled = false;
            ribbons_count.enabled = false;
            ribbons_count_num.enabled = false;
        }

        private Sprite GetHPBarSprite(float hp_percent)
        {
            if (hp_percent <= HP_BAR_RED_PERCENT)
                return hp_bar_red;
            else if (hp_percent <= HP_BAR_YELLOW_PERCENT)
                return hp_bar_yellow;
            else return hp_bar_green;
        }

        #endregion


        #region Summary Screen Methods

        private void CloseMenu()
        {
            GameObject.Destroy(this.gameObject);
        }

        #endregion

    }
}

