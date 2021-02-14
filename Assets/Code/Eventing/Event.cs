using System;
using System.Collections;
using System.Collections.Generic;
using Pokemon;
using Trainers;
using UI;
using UnityEngine;

namespace Eventing
{
    #region Enums

    public enum Triggers
    {
        ActionButton,
        PlayerTouch,
        EventTouch,
        Autorun,
        ParallelProcess,
        IsPlayer
    }

    public enum Conditions
    {
        SwitchOn,
        SwitchOff,
        VarableLessThan,
        VariableEqualTo,
        VariableGreaterThan,
        HaveItem
    }

    public enum Effects
    {
        ShowMessage,
        ShowChoices,
        ShowMessageBubble,
        SetMoveRoute,
        SetPlayerMoveRoute,
        AddPokemon,
    }

    public enum Logics
    {
        Branch,
        Loop,
        BreakLoop,
        ExitEventProcessing,
        Label,
        JumpToLabel
    }

    #endregion


    #region Structs

    [Serializable]
    public struct EventEntry
    {
        public Effects effect;

        public string message;
        public MoveableCharacter character;
        public ChoicePackage choice_package;
        public MoveRoutePackage move_route_package;
        public PokemonPackage pokemon_package;
    }

    [Serializable]
    public struct ChoicePackage
    {
        public string[] choices;
        public int starting_choice;
        public int cancel_choice;

        public ChoicePackage(string[] choices, int starting_choice, int cancel_choice)
        {
            this.choices = choices;
            this.starting_choice = starting_choice;
            this.cancel_choice = cancel_choice;
        }
    }

    [Serializable]
    public struct MoveRoutePackage
    {
        public MoveCommands[] move_route;
        public bool wait_for_moves_completion;
    }

    [Serializable]
    public struct PokemonPackage
    {
        public Species species;
        public uint level;
        public uint form_id;
    }

    #endregion


    [Serializable]
    public class Event : MonoBehaviour
    {
        #region Fields

        private EventManager event_manager;
        private UIManager ui_manager;

        [Header("Event Settings")]
        public Conditions[] conditions;
        public Triggers trigger;
        public bool passable;

        public List<Switch> local_switches;
        public List<Variable> local_variables;

        private bool event_playing;
        private bool effect_playing;

        #endregion


        #region Effect Entries

        [Header("Effect List")]
        public EventEntry[] event_entries;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            event_manager = FindObjectOfType<EventManager>();
            ui_manager = FindObjectOfType<UIManager>();

            event_playing = false;
            effect_playing = false;
        }

        private void Update()
        {
            if (!event_playing)
            {
                if (trigger == Triggers.Autorun)
                    StartCoroutine(PlayEvent());
            }
        }

        public IEnumerator PlayEvent()
        {
            // Start Event Processing
            event_playing = true;
            if (trigger != Triggers.ParallelProcess)
            {
                event_manager.DisablePlayerControl();
            }
            // Turn towards player on activation
            MoveableCharacter character = GetComponent<MoveableCharacter>();
            if (trigger == Triggers.ActionButton && character != null)
            {
                character.TurnTowardsPlayer();
            }

            // Run through all effects
            for (int i = 0; i < event_entries.Length; i++)
            {
                Effects effect = event_entries[i].effect;
                EventEntry event_entry = event_entries[i];
                effect_playing = true;

                switch (effect)
                {
                    case Effects.ShowMessage:
                        StartCoroutine(ShowMessage(event_entry.message)); break;
                    case Effects.ShowChoices:
                        StartCoroutine(ShowChoices(event_entry.message, event_entry.choice_package)); break;
                    case Effects.ShowMessageBubble:
                        StartCoroutine(ShowMessage(event_entry.message)); break;
                    case Effects.SetMoveRoute:
                        StartCoroutine(SetMoveRoute(event_entry.character, event_entry.move_route_package)); break;
                    case Effects.SetPlayerMoveRoute:
                        StartCoroutine(SetPlayerMoveRoute(event_entry.move_route_package)); break;
                    case Effects.AddPokemon:
                        StartCoroutine(AddPokemon(event_entry.pokemon_package)); break;
                    default:
                        Debug.Log("Unknown effect played: " + effect); break;
                }

                // Wait for effect to finish playing
                yield return new WaitUntil(() => !effect_playing);
            }

            // End Event Processing
            if (trigger != Triggers.ParallelProcess)
            {
                event_manager.EnablePlayerControl();
            }
            event_playing = false;
        }

        public void DestroyThisEvent()
        {
            event_manager.events.Remove(this);
            GameObject.Destroy(this.gameObject);
        }

        #endregion


        #region Effect Definitions

        public IEnumerator ShowMessage(string text)
        {
            Message message = ui_manager.ShowMessage(text);

            yield return new WaitUntil(() => message.finished);

            GameObject.Destroy(message.gameObject);
            effect_playing = false;
        }

        public IEnumerator ShowChoices(string text, ChoicePackage choice_package)
        {
            Message message = ui_manager.ShowMessage(text);

            yield return new WaitUntil(() => message.on_final_line && !message.writing);
            
            Choice choice = ui_manager.ShowChoices(choice_package);

            yield return new WaitUntil(() => choice.finished);

            GameObject.Destroy(message.gameObject);
            GameObject.Destroy(choice.gameObject);
            effect_playing = false;
        }

        // TODO: ShowMessageBubble that takes in a moveable character as well, makes bubble speech

        public IEnumerator SetMoveRoute(MoveableCharacter character, MoveRoutePackage package)
        {
            if (character == null)
                character = GetComponent<MoveableCharacter>();
            yield return new WaitUntil(() => !character.GetInMoveRoute());
            StartCoroutine(character.StartMoveRoute(package.move_route));

            if (package.wait_for_moves_completion)
                yield return new WaitUntil(() => !character.GetInMoveRoute());

            effect_playing = false;
        }

        public IEnumerator SetPlayerMoveRoute(MoveRoutePackage package)
        {
            MoveableCharacter character = event_manager.GetPlayerMover();
            yield return new WaitUntil(() => !character.GetInMoveRoute());
            StartCoroutine(character.StartMoveRoute(package.move_route));

            if (package.wait_for_moves_completion)
                yield return new WaitUntil(() => !character.GetInMoveRoute());

            effect_playing = false;
        }

        public IEnumerator AddPokemon(PokemonPackage package)
        {
            Trainer player = event_manager.GetPlayerTrainer();
            Pokemon.Pokemon new_pokemon = new Pokemon.Pokemon(package.species, package.level, package.form_id);
            player.AddPokemonToParty(new_pokemon);

            yield return new WaitForSeconds(0.5f);

            effect_playing = false;
        }

        #endregion

    }
}