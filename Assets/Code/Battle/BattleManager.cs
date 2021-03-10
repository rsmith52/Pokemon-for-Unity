using System.Collections;
using Eventing;
using Pokemon;
using Trainers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Battle
{
    #region Enums

    public enum BattleTypes
    {
        None,
        Wild,
        Trainer,
        Safari
    }

    public enum BattleStates
    {
        None,
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
        private bool player_found;
        private Trainer enemy_trainer;
        private UIManager ui_manager;
        private EventManager event_manager;

        private Pokemon.Pokemon player_pokemon;
        private Pokemon.Pokemon enemy_pokemon;

        public bool in_battle;
        private BattleTypes battle_type;
        private BattleStates battle_state;
        private bool effect_playing;

        #endregion


        #region References
        #endregion


        #region Mono Behavior

        private void Start()
        {
            // Setup battle manager automatically if in the map making scene
            Scene current_scene = SceneManager.GetActiveScene();
            string scene_name = current_scene.name;
            if (scene_name == Constants.MAP_MAKING_SCENE)
                Setup();
            // Perform initial setup if this is the original overworld load
            else if (SceneLoader.initial_overworld_load)
            {
                Setup();
            }
        }

        private void Update()
        {
            // Await player spawn to get reference to trainer
            if (!player_found)
            {
                player_trainer = FindObjectOfType<PlayerTrainer>();
                if (player_trainer != null)
                {
                    player_found = true;
                }
            }
        }

        #endregion


        #region Battle Manager Methods

        private void Setup()
        {
            player_found = false;
            ui_manager = FindObjectOfType<UIManager>();
            event_manager = FindObjectOfType<EventManager>();

            in_battle = false;
            battle_type = BattleTypes.None;
            battle_state = BattleStates.None;
            effect_playing = false;
        }

        #endregion


        #region Battle Start Methods

        public IEnumerator WildBattle(Pokemon.Pokemon wild_pokemon)
        {
            // Pause overworld
            event_manager.DisablePlayerControl();
            event_manager.DisableAllEvents();

            // Setup battle parameters for wild encounter
            in_battle = true;
            battle_type = BattleTypes.Wild;
            battle_state = BattleStates.Start;
            enemy_trainer = null;
            player_pokemon = player_trainer.GetFirstEligiblePokemon();
            enemy_pokemon = wild_pokemon;

            // Call Wild Battle Intro Scene
            effect_playing = true;
            StartCoroutine(RunWildBattleIntro());
            yield return new WaitUntil(() => !effect_playing);
            StartCoroutine(BattleStart());
        }

        public IEnumerator TrainerBattle(Trainer trainer)
        {
            // Pause overworld
            event_manager.DisablePlayerControl();
            event_manager.DisableAllEvents();

            // Setup battle parameters for a trainer battle
            in_battle = true;
            battle_type = BattleTypes.Trainer;
            battle_state = BattleStates.Start;
            enemy_trainer = trainer;
            player_pokemon = player_trainer.GetFirstEligiblePokemon();
            enemy_pokemon = enemy_trainer.GetFirstEligiblePokemon();

            // Call Trainer Battle Intro Scene
            effect_playing = true;
            StartCoroutine(RunTrainerBattleIntro());
            yield return new WaitUntil(() => !effect_playing);
            StartCoroutine(BattleStart());
        }

        public IEnumerator BattleStart()
        {
            // TODO: Implement this

            // Check for start of battle abilities
            /*
             * Check for which pokemon is faster, then check each in speed order
             * for start of battle triggering abilities using Ability.TriggersOnBattleStart(ability),
             * doing the following
             */

            /*
             * Start of battle abilities
             */

            yield return new WaitForSeconds(1);
            battle_state = BattleStates.Command;
        }

        #endregion


        #region Battle Scene Methods

        private IEnumerator RunWildBattleIntro()
        {
            // TODO: Implement this

            /* 
             * Use SceneLoader to transition to the battle scene, not destroying the game_manager (at minimum)
             * 
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
             *      - Pokemon/pokeball flies in
             *      - Burst as battler appears
             * - Definitely more...
            */

            StartCoroutine(SceneLoader.LoadScene(Constants.BATTLE_SCENE));
            yield return new WaitUntil(() => !SceneLoader.loading_scene);

            yield return new WaitForSeconds(1);
            effect_playing = false;
        }

        private IEnumerator RunTrainerBattleIntro()
        {
            // TODO: Implement this

            /*
             * 
             */

            StartCoroutine(SceneLoader.LoadScene(Constants.BATTLE_SCENE));
            yield return new WaitUntil(() => !SceneLoader.loading_scene);

            yield return new WaitForSeconds(1);
            effect_playing = false;
        }

        #endregion

    }
}

