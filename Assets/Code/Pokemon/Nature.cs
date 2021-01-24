namespace Pokemon
{
    #region Enums

    public enum Natures
    {
        Hardy,
        Lonely,
        Brave,
        Adamant,
        Naughty,
        Bold,
        Docile,
        Relaxed,
        Impish,
        Lax,
        Timid,
        Hasty,
        Serious,
        Jolly,
        Naive,
        Modest,
        Mild,
        Quiet,
        Bashful,
        Rash,
        Calm,
        Gentle,
        Sassy,
        Careful,
        Quirky
    }

    public enum Flavors
    {
        Spicy,
        Sour,
        Sweet,
        Dry,
        Bitter
    }

    #endregion

    public class Nature
    {
        #region Fields

        public Stats increased_stat;
        public Stats decreased_stat;
        public Flavors favorite_flavor;
        public Flavors disliked_flavor;

        #endregion


        #region Static Methods

        public static Natures GetNatureFromID(uint pokemon_id)
        {
            uint nature_id = pokemon_id % 25;
            return (Natures)nature_id;
        }

        public static Stats GetBoostedStat(Natures nature)
        {
            if (nature == Natures.Lonely || nature == Natures.Brave ||
                nature == Natures.Adamant || nature == Natures.Naughty)
                return Stats.Attack;
            else if (nature == Natures.Bold || nature == Natures.Relaxed ||
                nature == Natures.Impish || nature == Natures.Lax)
                return Stats.Defense;
            else if (nature == Natures.Timid || nature == Natures.Hasty ||
                nature == Natures.Jolly || nature == Natures.Naive)
                return Stats.Speed;
            else if (nature == Natures.Modest || nature == Natures.Mild ||
                nature == Natures.Quiet || nature == Natures.Rash)
                return Stats.SpecialAttack;
            else if (nature == Natures.Calm || nature == Natures.Gentle ||
                nature == Natures.Sassy || nature == Natures.Careful)
                return Stats.SpecialDefense;
            else return Stats.None;
        }

        public static Stats GetHinderedStat(Natures nature)
        {
            if (nature == Natures.Bold || nature == Natures.Timid ||
                nature == Natures.Modest || nature == Natures.Calm)
                return Stats.Attack;
            else if (nature == Natures.Lonely || nature == Natures.Hasty ||
                nature == Natures.Mild || nature == Natures.Gentle)
                return Stats.Defense;
            else if (nature == Natures.Adamant || nature == Natures.Impish ||
                nature == Natures.Jolly || nature == Natures.Careful)
                return Stats.SpecialAttack;
            else if (nature == Natures.Naughty || nature == Natures.Lax ||
                nature == Natures.Naive || nature == Natures.Rash)
                return Stats.SpecialDefense;
            else if (nature == Natures.Brave || nature == Natures.Relaxed ||
                nature == Natures.Quiet || nature == Natures.Sassy)
                return Stats.Speed;
            else return Stats.None;
        }

        #endregion

    }
}