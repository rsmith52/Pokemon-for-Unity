using System.Collections;
using Pokemon;
using Trainers;
using UnityEngine;

namespace Battle
{
    #region Enums

    public enum BattleStates
    {
        Start,
        Command,
        Move,
        Item,
        Pokemon,
        Run,
        Attacks,
        End
    }

    #endregion

    public class BattleManager : MonoBehaviour
    {
        #region Fields

        private PlayerTrainer player_trainer;
        private Trainer enemy_trainer;

        private Pokemon.Pokemon player_pokemon;
        private Pokemon.Pokemon enemy_pokemon;

        private BattleStates current_state;
        private bool animation_playing;

        [Header("Battle Components")]
        public GameObject player_base;
        public GameObject enemy_base;

        public GameObject player_battler;
        public GameObject enemy_battler;

        #endregion


        #region References
        #endregion


        #region Mono Behavior

        private void Start()
        {
            player_trainer = FindObjectOfType<PlayerTrainer>();
        }

        private void Update()
        {

        }

        #endregion


        #region Battle Manager Methods

        public IEnumerator WildBattle(Pokemon.Pokemon wild_pokemon)
        {
            enemy_trainer = null;
            player_pokemon = player_trainer.GetFirstEligiblePokemon();
            enemy_pokemon = wild_pokemon;

            current_state = BattleStates.Start;
            animation_playing = true;
            /* 
             * Start opening animations, yielding as necessary for specific times or vars
             * - Black screen slides open as...
             *      - Bases slide into position with...
             *          - Dimmed version of enemy battler
             *          - Player trainer back (or nothing)
             *      - Full Message bar is shown (empty)
             * - Once black gone and all in position...
             *      - Enemy info box slides in
             *      - Message appears (with arrow) awaiting input
             * - After input is received...
             *      - Message "Go Pokemon!" is displayed
             *      - Player animation of throwing
             *      - Pokemon flies in
             *      - Burst as battler appears
             *      
             * - Enemy
            */
            animation_playing = false;

            /*
             * Start of battle abilities
             */

            /*
             * Go to command state
             */
        }

        #endregion

    }
}

