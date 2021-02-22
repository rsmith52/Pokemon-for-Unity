using UnityEngine;
using Pokemon;
using System;
using Utilities;

namespace Trainers
{
    #region Enums

    public enum TrainerTypes
    {
        PokemonTrainer
    }

    #endregion


    #region Structs

    [Serializable]
    public struct Party
    {
        public Pokemon.Pokemon[] pokemon;
        [HideInInspector]
        public bool[] slot_filled;
        [HideInInspector]
        public bool is_full;
        public bool is_empty;
        public int size;
    }
    
    #endregion

    public class Trainer : MonoBehaviour
    {
        #region Fields

        public TrainerTypes trainer_type;
        new public string name;
        public Genders gender;

        public ushort trainer_id;
        public uint secret_id;

        public Party party;

        #endregion


        #region Mono Behavior

        protected virtual void Start()
        {
            InitializeParty();
        }

        #endregion


        #region Trainer Methods

        public void InitializeParty()
        {
            party = new Party();
            party.pokemon = new Pokemon.Pokemon[Constants.PARTY_SIZE];
            party.slot_filled = new bool[Constants.PARTY_SIZE];

            for (int i = 0; i < Constants.PARTY_SIZE; i++)
            {
                party.pokemon[i] = null;
                party.slot_filled[i] = false;
            }

            party.is_full = false;
            party.is_empty = true;
            party.size = 0;
        }

        public bool AddPokemonToParty(Pokemon.Pokemon pokemon)
        {
            if (party.is_full)
                return false;

            for (int i = 0; i < Constants.PARTY_SIZE; i++)
            {
                if (party.slot_filled[i] == false)
                {
                    party.pokemon[i] = pokemon;
                    party.slot_filled[i] = true;
                    party.is_empty = false;
                    party.size += 1;
                    if (i == Constants.PARTY_SIZE - 1)
                        party.is_full = true;

                    break;
                }
            }
            return true;
        }

        public Pokemon.Pokemon GetFirstEligiblePokemon()
        {
            for (int i = 0; i < party.size; i++)
            {
                if (party.pokemon[i].current_HP > 0)
                    return party.pokemon[i];
            }

            return null;
        }

        #endregion


        #region Static Methods

        public static ushort GenerateTrainerID()
        {
            System.Random random = new System.Random();

            ushort id = (ushort)random.Next(65535 + 1); // upper bound is exclusive

            return id;
        }

        public static uint GenerateSecretID()
        {
            System.Random random = new System.Random();

            uint id = (uint)random.Next(9999 + 1); // upper bound is exclusive

            return id;
        }

        #endregion

    }
}