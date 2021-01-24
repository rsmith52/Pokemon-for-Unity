using UnityEngine;

namespace Pokemon
{
    #region Enums

    public enum Genders
    {
        Male,
        Female,
        Genderless
    }

    public enum GenderRates
    {
        AlwaysMale,
        FemaleOneEighth,
        Female25Percent,
        Female50Percent,
        Female75Percent,
        FemaleSevenEighths,
        AlwaysFemale,
        Genderless
    }

    public enum EggGroups
    {
        Monster,
        Water1,
        Bug,
        Flying,
        Field,
        Fairy,
        Grass,
        Humanlike,
        Water3,
        Mineral,
        Amorphous,
        Water2,
        Ditto,
        Dragon,
        Undiscovered
    }

    #endregion

    public class Breeding
    {
        #region Static Methods

        public static Genders GetGenderFromID(uint pokemon_id, GenderRates gender_rate)
        {
            uint gender_id = pokemon_id & 0x000000FF;
            switch (gender_rate)
            {
                case GenderRates.AlwaysMale:
                    return Genders.Male;
                case GenderRates.FemaleOneEighth:
                    if (gender_id <= 31)
                        return Genders.Female;
                    else
                        return Genders.Male;
                case GenderRates.Female25Percent:
                    if (gender_id <= 63)
                        return Genders.Female;
                    else
                        return Genders.Male;
                case GenderRates.Female50Percent:
                    if (gender_id <= 127)
                        return Genders.Female;
                    else
                        return Genders.Male;
                case GenderRates.Female75Percent:
                    if (gender_id <= 191)
                        return Genders.Female;
                    else
                        return Genders.Male;
                case GenderRates.FemaleSevenEighths:
                    if (gender_id <= 225)
                        return Genders.Female;
                    else
                        return Genders.Male;
                case GenderRates.Genderless:
                    return Genders.Genderless;
                default:
                    return Genders.Genderless;
            }
        }

        #endregion

    }
}