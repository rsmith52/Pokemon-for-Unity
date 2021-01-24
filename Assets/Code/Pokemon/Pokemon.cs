using UnityEngine;
using Battle;
using Trainers;
using Utilities;
using Mapping;
using System;

namespace Pokemon
{
    #region Enums

    public enum ObtainModes
    {
        None,
        Met,
        EggReceived,
        Traded,
        FatefulEncounter
    }

    #endregion

    [Serializable]
    public class Pokemon
    {
        #region Fields

        public Species species;
        public string nickname;
        public uint level;
        public uint exp;
        public Types[] types;
        public Abilities ability;
        public Nullable<int> force_ability;
        public Genders gender;
        public Nullable<Genders> force_gender;
        public Natures nature;
        public Nullable<Natures> force_nature;
        public bool is_shiny;
        public Nullable<bool> force_shiny;
        public uint form_id;
        public uint current_HP;
        public StatArray stats;
        public uint happiness;
        public Statuses status;
        public uint status_parameter;
        public uint egg_steps;
        public MoveSlot[] moves;
        public Items.Items ball_used;
        public Items.Items held_item;
        public Nullable<Items.Mail> mail;
        public StatArray evs;
        public StatArray ivs;
        public int pokerus_status;
        public Pokemon fused_pokemon;
        public ContestStatArray contest_stats;
        public bool[] markings;
        public bool[] ribbons;
        public uint personal_id;
        public uint public_id;
        public uint trainer_id;
        public string original_trainer;
        public Genders original_trainer_gender;
        public ObtainModes obtain_mode;
        public DateTime obtain_date;
        public Map obtain_map;
        public uint obtain_level;
        public Map hatched_map;
        public string obtain_text;
        public Languages language;

        #endregion


        #region Constructors

        public Pokemon(Species species, uint level, uint form_id = 0, string nickname = "",
            Nullable<int>ability_id = null, Nullable<Genders> gender = null,
            Nullable<Natures> nature = null, Nullable<bool> is_shiny = null,
            Items.Items ball_used = Items.Items.PokeBall, Trainer ot = null,
            ObtainModes obtain_mode = ObtainModes.Met)
        {
            // Trainer
            if (ot == null)
                ot = GameObject.FindObjectOfType<PlayerTrainer>();
            this.trainer_id = ot.trainer_id;
            this.original_trainer = ot.name;
            this.original_trainer_gender = ot.gender;

            // Form
            Form alt_form = Form.GetFormData(species, form_id);
            if (alt_form != null)
                this.form_id = alt_form.form_id;
            else
                this.form_id = 0;

            // Personal ID Creation
            this.personal_id = GeneratePokemonID();
            this.public_id = this.personal_id & 0x0000FFFF;

            // Constant among species
            Specie specie = Specie.species[species];
            this.species = species;
            if (alt_form != null && alt_form.types.Length > 0)
                this.types = alt_form.types;
            else
                this.types = specie.types;

            // Level and experience
            this.level = level;
            this.exp = Experience.GetLevelTotalExp(level, specie.growth_rate);

            // Nickname
            this.nickname = nickname;

            // Ability (determined by personal id)
            if (ability_id != null)
            {
                this.force_ability = ability_id;
                if (ability_id == 0)
                {
                    if (alt_form != null && alt_form.abilities.Length > 0)
                        this.ability = alt_form.abilities[0];
                    else
                        this.ability = specie.abilities[0];
                }
                else if (ability_id == 1)
                {
                    if (alt_form != null && alt_form.abilities.Length > 1)
                        this.ability = alt_form.abilities[1];
                    else if (specie.abilities.Length > 1)
                        this.ability = specie.abilities[1];
                    else
                        this.ability = Ability.GetAbilityFromID(this.personal_id, specie.abilities);
                }
                else if (ability_id == 2)
                {
                    if (alt_form != null && alt_form.hidden_ability.Length > 0)
                        this.ability = alt_form.hidden_ability[0];
                    else if (specie.hidden_ability.Length > 0)
                        this.ability = specie.hidden_ability[0];
                    else
                        this.ability = Ability.GetAbilityFromID(this.personal_id, specie.abilities);
                }
                else
                    this.ability = Ability.GetAbilityFromID(this.personal_id, specie.abilities);
            }
            else
            {
                if (alt_form != null && alt_form.abilities.Length > 0)
                    this.ability = Ability.GetAbilityFromID(this.personal_id, alt_form.abilities);
                else
                    this.ability = Ability.GetAbilityFromID(this.personal_id, specie.abilities);
            }

            // Gender (determined by personal id)
            if (gender != null)
            {
                this.force_gender = (Genders)gender;
                if (gender == Genders.Male && specie.gender_rate != GenderRates.Genderless &&
                    specie.gender_rate != GenderRates.AlwaysFemale)
                    this.gender = (Genders)gender;
                else if (gender == Genders.Female && specie.gender_rate != GenderRates.Genderless &&
                    specie.gender_rate != GenderRates.AlwaysMale)
                    this.gender = (Genders)gender;
                else
                    this.gender = Breeding.GetGenderFromID(this.personal_id, specie.gender_rate);
            }
            else
                this.gender = Breeding.GetGenderFromID(this.personal_id, specie.gender_rate);

            // Nature (determined by personal id)
            if (nature != null)
            {
                this.force_nature = nature;
                if ((int)nature >= 0 && (int)nature < 25)
                    this.nature = (Natures)nature;
                else
                    this.nature = Nature.GetNatureFromID(this.personal_id);
            }
            else
                this.nature = Nature.GetNatureFromID(this.personal_id);

            // Shininess (determined by personal id and trainer id)
            if (is_shiny != null)
            {
                this.force_shiny = is_shiny;
                this.is_shiny = (bool)is_shiny;
            }
            else
                this.is_shiny = GetShininessFromIDs(this.personal_id, ot.trainer_id, ot.secret_id);

            // IVs and EVs
            StatArray ivs = Stat.GenerateRandomIVs();
            this.ivs = ivs;
            StatArray evs = new StatArray();
            this.evs = evs;

            // Stats
            if (alt_form != null && alt_form.base_stats.HP > 0)
                this.stats = Stat.CalculateStats(alt_form.base_stats, ivs, evs, level, this.nature);
            else
                this.stats = Stat.CalculateStats(specie.base_stats, ivs, evs, level, this.nature);

            // Current HP
            this.current_HP = this.stats.HP;

            // Friendship
            if (alt_form != null && alt_form.happiness != 0 && alt_form.happiness != specie.happiness)
                // TODO: If alt form has set happiness of 0 this doesn't work...
                // don't think this ever comes up though
                this.happiness = alt_form.happiness;
            else
                this.happiness = specie.happiness;

            // Status Condition
            this.status = Statuses.None;
            this.status_parameter = 0;

            // Egg Steps
            this.egg_steps = 0;

            // Set Moves
            if (alt_form != null && alt_form.moves.Length > 0)
                this.moves = Move.GeneratePokemonMoveSlots(alt_form.moves, level);
                
            else
                this.moves = Move.GeneratePokemonMoveSlots(specie.moves, level);
                
            // Ball Used and Held Item
            this.ball_used = ball_used;
            this.held_item = Items.Items.None;
            this.mail = null;

            // Pokerus Status
            this.pokerus_status = 0;

            // Fused Pokemon
            this.fused_pokemon = null;

            // Contest Stats
            this.contest_stats = new ContestStatArray();

            // Markings
            this.markings = new bool[Constants.NUM_MARKINGS];

            // Ribbons
            this.ribbons = new bool[Constants.NUM_RIBBONS];

            // Obtain info
            this.obtain_mode = obtain_mode;
            this.obtain_date = DateTime.Now;
            this.obtain_map = null; // TODO: Implement this
            this.obtain_level = level;
            this.hatched_map = null;
            this.obtain_text = null;

            // Language
            this.language = Languages.English; // TODO: Implement different options
        }

        #endregion


        #region Pokemon Methods

        public Specie GetSpecieData()
        {
            return Specie.species[this.species];
        }

        public string GetName()
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                // Special case for Type: Null
                if (species == Species.TypeNull)
                    return "Type: Null";
                else return GetSpecieData().name;
            }
            else return nickname;
        }

        public string GetGenderText()
        {
            switch (gender)
            {
                case Genders.Male:
                    return Constants.MALE_TEXT;
                case Genders.Female:
                    return Constants.FEMALE_TEXT;
                default:
                    return "";
            }
        }

        public string GetTrainerNameText()
        {
            switch (original_trainer_gender)
            {
                case Genders.Male:
                    return "<color=blue>" + original_trainer + "</color>";
                case Genders.Female:
                    return "<color=red>" + original_trainer + "</color>";
                default:
                    return original_trainer;
            }
        }

        public string GetPublicPersonalIDText()
        {
            uint public_id = personal_id & 0x0000FFFF;
            return public_id.ToString().PadLeft(5, '0');
        }

        public string GetDateText()
        {
            string month = Constants.MONTHS[obtain_date.Month];
            string day = obtain_date.Day.ToString();
            string year = obtain_date.Year.ToString();

            return month + ' ' + day + ", " + year;
        }

        public string GetMapText()
        {
            // TODO: This
            return "Secret Test Map";
        }

        public StatPair GetHighestStat()
        {
            uint highest_iv_value = ivs.HP;
            Stats highest_iv_stat = Stats.HP;

            if (ivs.ATK > highest_iv_value)
            {
                highest_iv_value = ivs.ATK;
                highest_iv_stat = Stats.Attack;
            }
            if (ivs.DEF > highest_iv_value)
            {
                highest_iv_value = ivs.DEF;
                highest_iv_stat = Stats.Defense;
            }
            if (ivs.SP_ATK > highest_iv_value)
            {
                highest_iv_value = ivs.SP_ATK;
                highest_iv_stat = Stats.SpecialAttack;
            }
            if (ivs.SP_DEF > highest_iv_value)
            {
                highest_iv_value = ivs.SP_DEF;
                highest_iv_stat = Stats.SpecialDefense;
            }
            if (ivs.SPD > highest_iv_value)
            {
                highest_iv_value = ivs.SPD;
                highest_iv_stat = Stats.Speed;
            }

            return new StatPair(highest_iv_stat, highest_iv_value);
        }

        public string GetCharacteristic()
        {
            StatPair highest_stat = GetHighestStat();
            int stat_index = (int)highest_stat.stat - 1;
            uint characteristic_id = highest_stat.value % 5;

            return Stat.CHARACTERISTICS[stat_index][characteristic_id];
        }

        public int[] GetRibbonSet(int set_size, int set_index)
        {
            int[] ribbon_set = null;
            int ribbons_found = 0;
            int last_ribbon_index = 0;

            for (int i = 0; i <= set_index; i++)
            {
                ribbon_set = new int[set_size];
                for (int j = 0; j < set_size; j++)
                {
                    ribbon_set[j] = -1;
                } 
                for (int j = last_ribbon_index; j < Constants.NUM_RIBBONS; j++)
                {
                    if (ribbons[j])
                    {
                        ribbon_set[ribbons_found++] = j;
                        last_ribbon_index++;
                    }
                    if (ribbons_found >= set_size)
                        break;
                }
            }

            return ribbon_set;
        }

        public int GetNumRibbons()
        {
            int ribbon_count = 0;

            for (int i = 0; i < Constants.NUM_RIBBONS; i++)
            {
                if (ribbons[i])
                    ribbon_count++;
            }

            return ribbon_count;
        }

        public float GetCurretHPPercent()
        {
            return (float)current_HP / (float)stats.HP;
        }

        #endregion


        #region Static Methods

        public static uint GeneratePokemonID()
        {
            System.Random random = new System.Random();

            uint thirty_bits = (uint)random.Next(1 << 30);
            uint two_bits = (uint)random.Next(1 << 2);
            uint full_range = (thirty_bits << 2) | two_bits;

            return full_range;
        }

        public static bool GetShininessFromIDs(uint pokemon_id, ushort trainer_id, uint secret_id)
        {
            uint p1 = pokemon_id / 65536;
            uint p2 = pokemon_id % 65536;

            uint shiny_chance = trainer_id ^ secret_id ^ p1 ^ p2;

            if (shiny_chance < Settings.SHINY_CHANCE)
                return true;
            else return false;
        }

        #endregion

    }
}