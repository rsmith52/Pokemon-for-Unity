using System;
using Utilities;

namespace Pokemon
{
    #region Enums

    public enum Stats
    {
        None,
        HP,
        Attack,
        Defense,
        SpecialAttack,
        SpecialDefense,
        Speed
    }

    public enum ContestStats
    {
        Beauty,
        Cool,
        Cute,
        Smart,
        Tough,
        Sheen
    }

    #endregion


    #region Structs

    [Serializable]
    public struct StatArray
    {
        public uint HP;
        public uint ATK;
        public uint DEF;
        public uint SP_ATK;
        public uint SP_DEF;
        public uint SPD;
    }

    [Serializable]
    public struct ContestStatArray
    {
        public uint beauty;
        public uint cool;
        public uint cute;
        public uint smart;
        public uint tough;
        public uint sheen;
    }

    public struct StatPair
    {
        public Stats stat;
        public uint value;

        public StatPair(Stats stat, uint value)
        {
            this.stat = stat;
            this.value = value;
        }
    }

    #endregion

    public class Stat
    {
        #region Constants

        public static string[][] CHARACTERISTICS = new string[][]
        {
            new string[] { "Loves to eat", "Takes plenty of siestas", "Nods off a lot", "Scatters things often", "Likes to relax" },
            new string[] { "Proud of its power", "Likes to thrash about", "A little quick tempered", "Likes to fight", "Quick tempered" },
            new string[] { "Sturdy body", "Capable of taking hits", "Highly persistent", "Good endurance", "Good perserverance" },
            new string[] { "Highly curious", "Mischievous", "Thoroughly cunning", "Often lost in thought", "Very finicky" },
            new string[] { "Strong willed", "Somewhat vain", "Strongly defiant", "Hates to lose", "Somewhat stubborn" },
            new string[] { "Likes to run", "Alert to sounds", "Impetuous and silly", "Somewhat of a clown", "Quick to flee" }
        };

        #endregion


        #region Static Methods

        public static StatArray GenerateRandomIVs()
        {
            StatArray ivs = new StatArray();
            ivs.HP = GenerateRandomIV();
            ivs.ATK = GenerateRandomIV();
            ivs.DEF = GenerateRandomIV();
            ivs.SP_ATK = GenerateRandomIV();
            ivs.SP_DEF = GenerateRandomIV();
            ivs.SPD = GenerateRandomIV();

            return ivs;
        }

        public static uint GenerateRandomIV()
        {
            System.Random random = Utilities.Random.GetRandom();

            uint iv = (uint)random.Next(31 + 1); // upper bound is exclusive
            return iv;
        }

        public static StatArray CalculateStats(StatArray base_stats, StatArray ivs, StatArray evs,
            uint level, Natures nature)
        {
            StatArray stats = new StatArray();
            stats.HP = CalculateHPStat(base_stats.HP, ivs.HP, evs.HP, level);
            stats.ATK = CalculateStat(Stats.Attack, base_stats.ATK, ivs.ATK, evs.ATK, level, nature);
            stats.DEF = CalculateStat(Stats.Defense, base_stats.DEF, ivs.DEF, evs.DEF, level, nature);
            stats.SP_ATK = CalculateStat(Stats.SpecialAttack, base_stats.SP_ATK, ivs.SP_ATK, evs.SP_ATK, level, nature);
            stats.SP_DEF = CalculateStat(Stats.SpecialDefense, base_stats.SP_DEF, ivs.SP_DEF, evs.SP_DEF, level, nature);
            stats.SPD = CalculateStat(Stats.Speed, base_stats.SPD, ivs.SPD, evs.SPD, level, nature);

            return stats;
        }

        public static uint CalculateHPStat(uint base_hp, uint hp_iv, uint hp_ev, uint level)
        {
            uint hp = (uint)((2 * base_hp + hp_iv + Math.Floor((double)hp_ev / 4)) * level / 100) + level + 10;
            return hp;
        }

        public static uint CalculateStat(Stats stat, uint base_stat, uint stat_iv, uint stat_ev,
            uint level, Natures nature)
        {
            double nature_mod = 1;
            if (Nature.GetBoostedStat(nature) == stat)
                nature_mod = 1.1;
            else if (Nature.GetHinderedStat(nature) == stat)
                nature_mod = 0.9;

            uint value = (uint)(Math.Floor((double)((2 * base_stat + stat_iv + Math.Floor((double)stat_ev / 4)) * level) / 100) + 5);
            value = (uint)(value * nature_mod);
            return value;
        }

        #endregion

    }
}