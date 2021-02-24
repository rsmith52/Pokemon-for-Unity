using UI;
using UnityEngine;
using Utilities;

namespace Eventing
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields

        private MoveableCharacter player_mover;
        private UIManager ui_manager;

        private Vector3 current_pos;
        private Vector3 target_pos;
        private Directions direction;

        private float key_held_time;

        #endregion


        #region Mono Behavior

        private void Awake()
        {
            // Set Player game object to not destroy on load
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            player_mover = GetComponent<MoveableCharacter>();
            ui_manager = FindObjectOfType<UIManager>();
        }

        private void OnDisable()
        {
            StopRunning();
        }

        private void OnEnable()
        {
            if (Input.GetKey(KeyCode.X))
                StartRunning();
        }

        private void Update()
        {
            // Get Positioning
            current_pos = player_mover.GetCurrentPos();
            target_pos = player_mover.GetTargetPos();
            direction = player_mover.direction;

            // Check Key Held Time - allows tapping to turn
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                key_held_time = Time.time;

            // Handle Pause Menu Input
            if (Input.GetKeyDown(KeyCode.Space))
                ui_manager.ShowPauseMenu();

            // Handle Action Input
            if (Input.GetKeyDown(KeyCode.Z))
                player_mover.ActivateEvent();

            // Handle Running Input
            if (Input.GetKeyDown(KeyCode.X))
                StartRunning();
            else if (Input.GetKeyUp(KeyCode.X))
                StopRunning();

            // Handle Movement Input
            if (Input.GetKey(KeyCode.UpArrow) && current_pos == target_pos)
            {
                if (!player_mover.fix_direction && direction != Directions.Up)
                {
                    player_mover.TurnUp();
                }
                else if (Time.time - key_held_time > Constants.TAP_VS_HOLD_TIME)
                {
                    player_mover.MoveUp();
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && current_pos == target_pos)
            {
                if (!player_mover.fix_direction && direction != Directions.Left)
                {
                    player_mover.TurnLeft();
                }
                else if (Time.time - key_held_time > Constants.TAP_VS_HOLD_TIME)
                {
                    player_mover.MoveLeft();
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow) && current_pos == target_pos)
            {
                if (!player_mover.fix_direction && direction != Directions.Right)
                {
                    player_mover.TurnRight();
                }
                else if (Time.time - key_held_time > Constants.TAP_VS_HOLD_TIME)
                {
                    player_mover.MoveRight();
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) && current_pos == target_pos)
            {
                if (!player_mover.fix_direction && direction != Directions.Down)
                {
                    player_mover.TurnDown();
                }
                else if (Time.time - key_held_time > Constants.TAP_VS_HOLD_TIME)
                {
                    player_mover.MoveDown();
                }
            }

            // Handle Debug Inputs
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (player_mover.move_through_walls)
                    player_mover.MoveThroughWallsOn();
                else
                    player_mover.MoveThroughWallsOff();
            }
            if (Input.GetKeyDown(KeyCode.W))
                player_mover.MoveLayerUp();
            else if (Input.GetKeyDown(KeyCode.S))
                player_mover.MoveLayerDown();

        }

        private void StartRunning()
        {
            player_mover.ChangeSpeed(MovementSpeeds.VeryFast);
            player_mover.animator.SetBool(Constants.RUN_ANIMATION, true);
        }

        private void StopRunning()
        {
            player_mover.ChangeSpeed(MovementSpeeds.Moderate);
            player_mover.animator.SetBool(Constants.RUN_ANIMATION, false);
        }

        #endregion

    }
}