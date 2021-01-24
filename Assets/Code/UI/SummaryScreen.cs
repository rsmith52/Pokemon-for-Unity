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
        private static readonly string[] MOVE_INFO_TEXT = new string[]
        {
            "Category", "Power", "Accuracy"
        };

        private static readonly float ICON_ANIM_TIME = 0.25f;
        private static readonly float SPRITE_ANIM_TIME = 0.1f;
        private static readonly float ANIM_SPRITE_SIZE_MULT = 3f;

        private static readonly Color32 DARK_BG_COLOR = Constants.DARK_BG_TEXT_COLOR;
        private static readonly Color32 LIGHT_BG_COLOR = Constants.LIGHT_BG_TEXT_COLOR;

        private static readonly float EXP_BAR_MAX_WIDTH = 400f;

        private static readonly float HP_BAR_MAX_WIDTH = 300f;
        private static readonly float HP_BAR_YELLOW_PERCENT = 0.5f;
        private static readonly float HP_BAR_RED_PERCENT = 0.2f;

        private static readonly float MOVE_SEL_OFFSET = 200f * 0.4266667f;

        private static readonly int NUM_RIBBONS = 12;
        private static readonly int NUM_RIBBONS_IN_ROW = 4;
        private static readonly int NUM_RIBBONS_IN_COL = 3;

        private static readonly float RIBBON_SEL_OFFSET_X = 214f * 0.48f;
        private static readonly float RIBBON_SEL_OFFSET_Y = 212f * 0.4266667f;

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
        public Image[] info_two_types = new Image[2];
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
        public Image ribbon_sel;
        public Image ribbon_details_panel;
        public Text ribbon_name;
        public Text ribbon_desc;

        [Header("Egg Screen")]
        public Image egg_bg;

        [Header("Move Detail Screen")]
        public Image move_detail_bg;
        public Text move_detail_title;
        public Image[] move_detail_types = new Image[Constants.NUM_MOVES];
        public Text[] move_detail_names = new Text[Constants.NUM_MOVES];
        public Text[] move_detail_pp_labels = new Text[Constants.NUM_MOVES];
        public Text[] move_detail_pp = new Text[Constants.NUM_MOVES];
        public Image move_detail_poke_icon;
        public Image move_detail_one_type;
        public Image[] move_detail_two_types = new Image[2];
        public Text move_detail_info_labels;
        public Image move_detail_category;
        public Text move_detail_info;
        public Text move_detail_desc;
        public Image move_detail_sel;
        public Image move_detail_selection;

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
        public Sprite[] ribbons = new Sprite[Ribbons.NUM_RIBBONS];
        public Sprite[] move_categories = new Sprite[Enum.GetValues(typeof(DamageCategories)).Length];
        public Sprite[] move_sel = new Sprite[2];

        private PlayerTrainer player;
        private Party party;
        private MoveSlot temp_move_slot;
        private int[] poke_ribbons;

        private SummaryPages current_page;
        private Pokemon.Pokemon current_pokemon;
        private Sprite[] pokemon_frames;
        public int pokemon_selection;
        public bool in_pc;
        public bool learning_move;

        private float icon_anim_time;
        private int icon_anim_frame;
        private bool pokemon_changed;
        private float sprite_anim_time;
        private int sprite_anim_frame;

        private bool page_changed;
        private bool awaiting_input;
        private int selected_move;
        private bool swapping_moves;
        private int marked_move;
        private bool ribbon_details;
        private int selected_ribbon;

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
            pokemon_changed = true;
            awaiting_input = true;
        }

        private void Update()
        {
            // Get player input
            if (awaiting_input)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (current_page != SummaryPages.MoveDetail && !ribbon_details)
                    {
                        HidePage(current_page);
                        if ((int)current_page == SUMMARY_PAGES.Length - 1)
                            current_page = (SummaryPages)0;
                        else
                            current_page += 1;
                        page_changed = true;
                    }
                    else if (ribbon_details)
                    {
                        if (selected_ribbon % NUM_RIBBONS_IN_ROW == NUM_RIBBONS_IN_ROW - 1)
                        {
                            selected_ribbon -= (NUM_RIBBONS_IN_ROW - 1);
                            for (int i = 0; i < NUM_RIBBONS_IN_ROW - 1; i++)
                                RibbonSelLeft();
                        }
                        else
                        {
                            selected_ribbon += 1;
                            RibbonSelRight();
                        }
                        UpdateRibbonsScreen(current_pokemon);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (current_page != SummaryPages.MoveDetail && !ribbon_details)
                    {
                        HidePage(current_page);
                        if ((int)current_page == 0)
                            current_page = (SummaryPages)(SUMMARY_PAGES.Length - 1);
                        else
                            current_page -= 1;
                        page_changed = true;
                    }
                    else if (ribbon_details)
                    {
                        if (selected_ribbon % NUM_RIBBONS_IN_ROW == 0)
                        {
                            selected_ribbon += (NUM_RIBBONS_IN_ROW - 1);
                            for (int i = 0; i < NUM_RIBBONS_IN_ROW - 1; i++)
                                RibbonSelRight();
                        }
                        else
                        {
                            selected_ribbon -= 1;
                            RibbonSelLeft();
                        }
                        UpdateRibbonsScreen(current_pokemon);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (current_page == SummaryPages.MoveDetail)
                    {
                        int sel_index = 0;
                        if (swapping_moves) sel_index = 1;
                        if (selected_move == current_pokemon.GetNumMoves() - 1)
                        {
                            MoveSelToTop(sel_index);
                            selected_move = 0;
                        }
                        else
                        {
                            MoveSelDown(sel_index);
                            selected_move += 1;
                        }
                        UpdateMoveDetailsScreen(current_pokemon);
                    }
                    else if (ribbon_details)
                    {
                        if (selected_ribbon >= NUM_RIBBONS - NUM_RIBBONS_IN_ROW)
                        {
                            selected_ribbon -= (NUM_RIBBONS_IN_COL - 1) * NUM_RIBBONS_IN_ROW;
                            for (int i = 1; i < NUM_RIBBONS_IN_COL; i++)
                                RibbonSelUp();
                        }
                        else
                        {
                            selected_ribbon += NUM_RIBBONS_IN_ROW;
                            RibbonSelDown();
                        }
                        UpdateRibbonsScreen(current_pokemon);
                    }
                    else
                    {
                        HidePage(current_page);
                        if (pokemon_selection == party.size - 1)
                            pokemon_selection = 0;
                        else
                            pokemon_selection += 1;
                        page_changed = true;
                        pokemon_changed = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (current_page == SummaryPages.MoveDetail)
                    {
                        int sel_index = 0;
                        if (swapping_moves) sel_index = 1;
                        if (selected_move == 0)
                        {
                            MoveSelToBottom(sel_index);
                            selected_move = current_pokemon.GetNumMoves() - 1;
                        }
                        else
                        {
                            MoveSelUp(sel_index);
                            selected_move -= 1;
                        }
                        UpdateMoveDetailsScreen(current_pokemon);
                    }
                    else if (ribbon_details)
                    {
                        if (selected_ribbon < NUM_RIBBONS_IN_ROW)
                        {
                            selected_ribbon += (NUM_RIBBONS_IN_COL - 1) * NUM_RIBBONS_IN_ROW;
                            for (int i = 1; i < NUM_RIBBONS_IN_COL; i++)
                                RibbonSelDown();
                        }
                        else
                        {
                            selected_ribbon -= NUM_RIBBONS_IN_ROW;
                            RibbonSelUp();
                        }
                        UpdateRibbonsScreen(current_pokemon);
                    }
                    else
                    {
                        HidePage(current_page);
                        if (pokemon_selection == 0)
                            pokemon_selection = party.size - 1;
                        else
                            pokemon_selection -= 1;
                        page_changed = true;
                        pokemon_changed = true;
                    }
                        
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (current_page == SummaryPages.Moves)
                    {
                        HideGeneralComponents();
                        HidePage(current_page);
                        current_page = SummaryPages.MoveDetail;
                        selected_move = 0;
                        page_changed = true;
                    }
                    else if (current_page == SummaryPages.MoveDetail && !swapping_moves)
                    {
                        swapping_moves = true;
                        MarkCurrentMove();
                    }
                    else if (current_page == SummaryPages.MoveDetail)
                    {
                        swapping_moves = false;
                        SwapMoves(marked_move, selected_move);
                        UnmarkMarkedMove();
                        BuildCurrentPage();
                    }
                    else if (current_page == SummaryPages.Ribbons)
                    {
                        ribbon_details = true;
                        selected_ribbon = 0;
                        UpdateRibbonsScreen(current_pokemon);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    if (current_page == SummaryPages.MoveDetail && !swapping_moves)
                    {
                        HidePage(current_page);
                        MoveSelToTop(0);
                        current_page = SummaryPages.Moves;
                        page_changed = true;
                    }
                    else if (current_page == SummaryPages.MoveDetail)
                    {
                        swapping_moves = false;
                        UnmarkMarkedMove();
                    }
                    else if (current_page == SummaryPages.Ribbons && ribbon_details)
                    {
                        ribbon_details = false;
                        for (int i = 0; i < selected_ribbon % NUM_RIBBONS_IN_ROW; i++)
                            RibbonSelLeft();
                        for (int i = 0; i < selected_ribbon / NUM_RIBBONS_IN_ROW; i++)
                            RibbonSelUp();
                        BuildCurrentPage();
                    }
                    else
                        CloseMenu();
                }
            }

            if (current_page == SummaryPages.MoveDetail)
            {
                // Animate pokemon_icons
                icon_anim_time += Time.deltaTime;
                // Keep icons animated
                if (icon_anim_time > ICON_ANIM_TIME)
                {
                    icon_anim_time = 0;
                    icon_anim_frame = (icon_anim_frame + 1) % 2;
                    move_detail_poke_icon.sprite = Specie.GetPokemonIcon((uint)current_pokemon.species, icon_anim_frame, current_pokemon.form_id);
                }
            }

            // Build/Show current page
            if (page_changed)
                BuildCurrentPage();

            // Update pokemon sprite if using animated pokemon sprites
            if (pokemon_changed && Settings.ANIMATED_SPRITES)
            {
                if (!in_pc)
                    current_pokemon = party.pokemon[pokemon_selection];

                Specie species = current_pokemon.GetSpecieData();
                pokemon_frames = Specie.GetPokemonAnimFrames(species.national_dex, current_pokemon.gender, current_pokemon.is_shiny, false, current_pokemon.form_id);
                sprite_anim_frame = 0;
                pokemon_sprite.sprite = pokemon_frames[sprite_anim_frame];
                pokemon_sprite.SetNativeSize();
                pokemon_sprite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ANIM_SPRITE_SIZE_MULT * pokemon_sprite.rectTransform.sizeDelta.x);
                pokemon_sprite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ANIM_SPRITE_SIZE_MULT * pokemon_sprite.rectTransform.sizeDelta.y);
                pokemon_changed = false;
            }
            // Animate pokemon sprite if using animated pokemon sprites
            if (Settings.ANIMATED_SPRITES)
            {
                sprite_anim_time += Time.deltaTime;
                if (sprite_anim_time > SPRITE_ANIM_TIME)
                {
                    sprite_anim_time = 0;
                    sprite_anim_frame = (sprite_anim_frame + 1) % pokemon_frames.Length;
                    pokemon_sprite.sprite = pokemon_frames[sprite_anim_frame];
                }
            }
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
                case SummaryPages.Egg:
                    break;
                case SummaryPages.MoveDetail:
                    BuildMoveDetailScreen(current_pokemon);
                    break;
                case SummaryPages.LearnMove:
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
                case SummaryPages.Egg:
                    break;
                case SummaryPages.MoveDetail:
                    HideMoveDetailScreen();
                    break;
                default:
                    break;
            }
        }

        private void HideAllPages()
        {
            HideGeneralComponents();
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
            if (!Settings.ANIMATED_SPRITES)
                pokemon_sprite.sprite = Specie.GetPokemonSprite(species.national_dex, pokemon.gender, pokemon.is_shiny, false, pokemon.form_id);
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
                info_two_types[0].enabled = false;
                info_two_types[1].enabled = false;
            }
            else
            {
                info_one_type.enabled = false;
                info_two_types[0].enabled = true;
                info_two_types[0].sprite = types[(int)pokemon.types[0]];
                info_two_types[1].enabled = true;
                info_two_types[1].sprite = types[(int)pokemon.types[1]];
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
            info_two_types[0].enabled = false;
            info_two_types[1].enabled = false;
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
            {
                if ((int)Nature.GetBoostedStat(pokemon.nature) - 1 == i)
                    skills_labels.text += "<color=red>" + SKILLS_TEXT[i] + "</color>\n";
                else if ((int)Nature.GetHinderedStat(pokemon.nature) - 1 == i)
                    skills_labels.text += "<color=blue>" + SKILLS_TEXT[i] + "</color>\n";
                else
                    skills_labels.text += SKILLS_TEXT[i] + '\n';
            }
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
            poke_ribbons = pokemon.GetRibbonSet(NUM_RIBBONS, 0);

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

            ribbon_details = false;
            ribbon_sel.enabled = false;
            ribbon_details_panel.enabled = false;
            ribbon_name.enabled = false;
            ribbon_desc.enabled = false;
        }

        private void UpdateRibbonsScreen(Pokemon.Pokemon pokemon)
        {
            item_icon.enabled = false;
            item.enabled = false;
            item_name.enabled = false;
            for (int i = 0; i < Constants.NUM_MARKINGS; i++)
                markings[i].enabled = false;

            ribbon_sel.enabled = true;
            ribbon_details_panel.enabled = true;
            ribbon_name.enabled = true;
            ribbon_name.text = "";
            ribbon_desc.enabled = true;
            ribbon_desc.text = "";
            if (poke_ribbons[selected_ribbon] >= 0)
            {
                ribbon_name.text = Ribbons.RIBBON_NAMES[poke_ribbons[selected_ribbon]];
                ribbon_desc.text = Ribbons.RIBBON_DESCRIPTIONS[poke_ribbons[selected_ribbon]];
            }
        }

        private void HideRibbonsScreen()
        {
            ribbons_bg.enabled = false;
            ribbons_title.enabled = false;
            for (int i = 0; i < NUM_RIBBONS; i++)
                ribbons_sprites[i].enabled = false;
            ribbons_count.enabled = false;
            ribbons_count_num.enabled = false;
            ribbon_sel.enabled = false;
            ribbon_details_panel.enabled = false;
            ribbon_name.enabled = false;
            ribbon_desc.enabled = false;
        }

        private void RibbonSelRight()
        {
            ribbon_sel.rectTransform.Translate(Vector3.right * RIBBON_SEL_OFFSET_X);
        }
        private void RibbonSelLeft()
        {
            ribbon_sel.rectTransform.Translate(Vector3.left * RIBBON_SEL_OFFSET_X);
        }
        private void RibbonSelDown()
        {
            ribbon_sel.rectTransform.Translate(Vector3.down * RIBBON_SEL_OFFSET_Y);
        }
        private void RibbonSelUp()
        {
            ribbon_sel.rectTransform.Translate(Vector3.up * RIBBON_SEL_OFFSET_Y);
        }

        private void BuildMoveDetailScreen(Pokemon.Pokemon pokemon)
        {
            // Background and Title
            move_detail_bg.enabled = true;
            move_detail_title.enabled = true;
            move_detail_title.text = MOVE_DETAIL_PAGE;

            // Move Detail Components
            for (int i = 0; i < Constants.NUM_MOVES; i++)
            {
                if (pokemon.moves[i].move == Moves.None)
                {
                    move_detail_types[i].enabled = false;
                    move_detail_names[i].enabled = false;
                    move_detail_pp_labels[i].enabled = false;
                    move_detail_pp[i].enabled = false;
                }
                else
                {
                    MoveSlot move_slot = pokemon.moves[i];
                    move_detail_types[i].enabled = true;
                    move_detail_types[i].sprite = types[(int)move_slot.type];
                    move_detail_names[i].enabled = true;
                    move_detail_names[i].text = move_slot.name;
                    move_detail_pp_labels[i].enabled = true;
                    move_detail_pp_labels[i].text = PP_TEXT;
                    move_detail_pp[i].enabled = true;
                    move_detail_pp[i].text = move_slot.remaining_PP.ToString() + PP_SEPERATOR_TEXT + move_slot.total_pp.ToString();
                }
            }
            move_detail_poke_icon.enabled = true;
            move_detail_poke_icon.sprite = Specie.GetPokemonIcon((uint)pokemon.species, 0, (uint)pokemon.form_id);
            if (pokemon.types[1] == Pokemon.Types.None)
            {
                move_detail_one_type.enabled = true;
                move_detail_one_type.sprite = types[(int)pokemon.types[0]];
                move_detail_two_types[0].enabled = false;
                move_detail_two_types[1].enabled = false;
            }
            else
            {
                move_detail_one_type.enabled = false;
                move_detail_two_types[0].enabled = true;
                move_detail_two_types[0].sprite = types[(int)pokemon.types[0]];
                move_detail_two_types[1].enabled = true;
                move_detail_two_types[1].sprite = types[(int)pokemon.types[1]];
            }
            move_detail_info_labels.enabled = true;
            move_detail_info_labels.text = "";
            for (int i = 0; i < MOVE_INFO_TEXT.Length; i++)
                move_detail_info_labels.text += MOVE_INFO_TEXT[i] + '\n';
            Move move = Move.moves[pokemon.moves[selected_move].move];
            move_detail_category.enabled = true;
            move_detail_category.sprite = move_categories[(int)move.damage_category];
            move_detail_info.enabled = true;
            move_detail_info.text = move.GetBasePowerText() + '\n' + move.GetAccuracyText();
            move_detail_desc.enabled = true;
            move_detail_desc.text = move.description;
            move_detail_sel.enabled = true;
            move_detail_sel.sprite = move_sel[0];
            move_detail_selection.enabled = false;
        }

        private void UpdateMoveDetailsScreen(Pokemon.Pokemon pokemon)
        {
            Move move = Move.moves[pokemon.moves[selected_move].move];
            move_detail_category.enabled = true;
            move_detail_category.sprite = move_categories[(int)move.damage_category];
            move_detail_info.enabled = true;
            move_detail_info.text = move.GetBasePowerText() + '\n' + move.GetAccuracyText();
            move_detail_desc.enabled = true;
            move_detail_desc.text = move.description;
        }

        private void HideMoveDetailScreen()
        {
            move_detail_bg.enabled = false;
            move_detail_title.enabled = false;
            for (int i = 0; i < Constants.NUM_MOVES; i++)
            {
                move_detail_types[i].enabled = false;
                move_detail_names[i].enabled = false;
                move_detail_pp_labels[i].enabled = false;
                move_detail_pp[i].enabled = false;
            }
            move_detail_poke_icon.enabled = false;
            move_detail_one_type.enabled = false;
            move_detail_two_types[0].enabled = false;
            move_detail_two_types[1].enabled = false;
            move_detail_info_labels.enabled = false;
            move_detail_category.enabled = false;
            move_detail_info.enabled = false;
            move_detail_desc.enabled = false;
            move_detail_sel.enabled = false;
            move_detail_selection.enabled = false;
        }

        private void MoveSelDown(int sel_index)
        {
            if (sel_index == 0)
                move_detail_sel.rectTransform.Translate(Vector3.down * MOVE_SEL_OFFSET);
            else if (sel_index == 1)
                move_detail_selection.rectTransform.Translate(Vector3.down * MOVE_SEL_OFFSET);
        }
        private void MoveSelUp(int sel_index)
        {
            if (sel_index == 0)
                move_detail_sel.rectTransform.Translate(Vector3.up * MOVE_SEL_OFFSET);
            else if (sel_index == 1)
                move_detail_selection.rectTransform.Translate(Vector3.up * MOVE_SEL_OFFSET);
        }
        private void MoveSelToBottom(int sel_index)
        {
            if (sel_index == 0)
                move_detail_sel.rectTransform.Translate(Vector3.down * MOVE_SEL_OFFSET * (current_pokemon.GetNumMoves() - 1 - selected_move));
            else if (sel_index == 1)
                move_detail_selection.rectTransform.Translate(Vector3.down * MOVE_SEL_OFFSET * (current_pokemon.GetNumMoves() - 1 - selected_move));
        }
        private void MoveSelToTop(int sel_index)
        {
            if (sel_index == 0)
                move_detail_sel.rectTransform.Translate(Vector3.up * MOVE_SEL_OFFSET * selected_move);
            else if (sel_index == 1)
                move_detail_selection.rectTransform.Translate(Vector3.up * MOVE_SEL_OFFSET * selected_move);
        }
        private void MarkCurrentMove()
        {
            marked_move = selected_move;
            move_detail_sel.sprite = move_sel[1];
            move_detail_selection.enabled = true;
            move_detail_selection.sprite = move_sel[0];
            for (int i = 0; i < selected_move; i++)
                MoveSelDown(1);
        }
        private void UnmarkMarkedMove()
        {
            move_detail_sel.sprite = move_sel[0];
            move_detail_selection.enabled = false;
            MoveSelToTop(1);
            if (marked_move > selected_move)
                for (int i = 0; i < marked_move - selected_move; i++)
                    MoveSelUp(0);
            else
                for (int i = 0; i < selected_move - marked_move; i++)
                    MoveSelDown(0);
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

        private void SwapMoves(int move_1_index, int move_2_index)
        {
            temp_move_slot = current_pokemon.moves[move_1_index];
            current_pokemon.moves[move_1_index] = current_pokemon.moves[move_2_index];
            current_pokemon.moves[move_2_index] = temp_move_slot;
        }

        private void CloseMenu()
        {
            GameObject.Destroy(this.gameObject);
        }

        #endregion

    }
}

