using System;
using Pokemon;

namespace Trainers
{
    [Serializable]
    public class PlayerTrainer : Trainer
    {
        #region Fields

        public uint money;
        public bool has_pokedex;

        #endregion


        #region Mono Behavior

        protected override void Start()
        {
            // SUPER TEMPORARY, NOT THE PLACE FOR THIS
            this.trainer_id = GenerateTrainerID();
            this.secret_id = GenerateSecretID();
            this.gender = Genders.Male;

            base.Start();
        }

        #endregion


        #region Static Methods
        #endregion
    }
}