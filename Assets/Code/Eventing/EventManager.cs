using System.Collections.Generic;
using UnityEngine;
using Trainers;
using UnityEngine.SceneManagement;
using Utilities;

namespace Eventing
{
    public class EventManager : MonoBehaviour
    {
        #region Fields

        public List<Event> events;

        private PlayerController player;
        public GameObject player_prefab;

        #endregion


        #region Mono Behavior

        private void Awake()
        {
            // Set Game Manager game object to not destroy on load
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Setup event manager automatically if in the map making scene
            Scene current_scene = SceneManager.GetActiveScene();
            string scene_name = current_scene.name;
            if (scene_name == Constants.MAP_MAKING_SCENE)
                Setup();
            // Perform initial setup if this is the original overworld load
            else if (SceneLoader.initial_overworld_load)
            {
                SpawnPlayer();
                Setup();
            }
        }

        private void Update()
        {
            // TODO: Update events on conditions... maybe every couple frames
        }

        #endregion


        #region Event Manager Methods

        private void Setup()
        {
            // Find and setup player
            player = FindObjectOfType<PlayerController>();
            Event player_event = player.GetComponent<Event>();

            // Get events in scene
            Event[] all_events = FindObjectsOfType<Event>();
            foreach (Event e in all_events)
                events.Add(e);
        }

        private void SpawnPlayer()
        {
            GameObject player = Instantiate(player_prefab);
        }

        #endregion


        #region Player Referencing

        public PlayerController GetPlayerControler()
        {
            return player;
        }

        public MoveableCharacter GetPlayerMover()
        {
            return player.GetComponent<MoveableCharacter>();
        }

        public PlayerTrainer GetPlayerTrainer()
        {
            return player.GetComponent<PlayerTrainer>();
        }

        public void DisablePlayerControl()
        {
            player.enabled = false;
        }

        public void EnablePlayerControl()
        {
            player.enabled = true;
        }

        #endregion


        #region Event Controls

        public void PlayEvent(Event e)
        {
            e.PlayEvent();
        }

        public void PlayEffect(Event e, Effects effect, string param_string)
        {
            // TODO: have event e perform effect
        }

        public void DisableEvent(Event e)
        {
            // TODO
        }

        public void DisableAllEvents()
        {
            // TODO call DisableEvent() on all events
        }

        #endregion


        #region Event Awareness

        // Returns the events directly around a moveable character
        public Event[] GetNeighborEvents(MoveableCharacter character)
        {
            Event on_event = null;
            Event up_event = null;
            Event left_event = null;
            Event right_event = null;
            Event down_event = null;

            Vector3 pos = character.transform.position;

            for (int i = 0; i < events.Count; i++)
            {
                Event this_event = events[i];
                if (on_event == null && this_event.transform.position == pos)
                    on_event = this_event;
                if (up_event == null && this_event.transform.position == pos + Vector3.up)
                    up_event = this_event;
                if (left_event == null && this_event.transform.position == pos + Vector3.left)
                    left_event = this_event;
                if (right_event == null && this_event.transform.position == pos + Vector3.right)
                    right_event = this_event;
                if (down_event == null && this_event.transform.position == pos + Vector3.down)
                    down_event = this_event;
            }

            return new Event[] { on_event, up_event, left_event, right_event, down_event };
        }

        #endregion

    }
}