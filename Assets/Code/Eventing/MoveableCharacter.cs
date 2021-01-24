using System.Collections;
using Mapping;
using UnityEngine;
using Utilities;

namespace Eventing
{
    #region Enums

    public enum MovementSpeeds
    {
        VerySlow,
        Slow,
        Moderate,
        Fast,
        VeryFast
    }

    public enum Directions
    {
        Up,
        Left,
        Right,
        Down
    }

    public enum MoveCommands
    {
        TurnUp,
        TurnLeft,
        TurnRight,
        TurnDown,
        Turn90DegreesCW,
        Turn90DegreesCCW,
        Turn180Degrees,
        TurnAtRandom,
        TurnTowardsPlayer,
        MoveUp,
        MoveLeft,
        MoveRight,
        MoveDown,
        StepForward,
        StepBackward,
        MoveAtRandom,
        Jump,
        JumpForward,
        JumpBackward,
        MoveLayerUp,
        MoveLayerDown,
        SetInvisibleFlag,
        SetThroughFlag,
        SetFixDirectionFlag,
        SetWalkingFlag,
        SetSteppingFlag,
        Wait,
        ChangeSpeed
    }

    #endregion

    public class MoveableCharacter : MonoBehaviour
    {
        #region Fields

        [Header("Positition")]
        public MovementSpeeds movement_speed;
        private float speed;
        private Vector3 pos;
        public Directions direction;
        public Animator animator;
        private SpriteRenderer[] sprites;
        private SpriteMask bush_mask;

        [Header("Flags")]
        public bool invisible = false;
        public bool move_through_walls = false;
        public bool fix_direction = false;
        public bool walking_animation = true;
        public bool stepping_animation = false;

        [Header("Awareness")]
        private bool moved;
        private bool other_moved;
        private bool in_move_route;
        private bool tile_activated;
        private bool in_bush;

        private MapManager map_manager;
        private AdvancedTileBase on_tile;
        private AdvancedTileBase up_tile;
        private AdvancedTileBase left_tile;
        private AdvancedTileBase right_tile;
        private AdvancedTileBase down_tile;

        private EventManager event_manager;
        private Event[] neighbor_events = new Event[0];
        private Event on_event;
        private Event up_event;
        private Event left_event;
        private Event right_event;
        private Event down_event;

        #endregion


        #region Mono Behavior

        private void Start()
        {
            // Basic Setup
            pos = transform.position;
            speed = Constants.SPEEDS[(int)movement_speed];
            animator = GetComponentInChildren<Animator>();
            sprites = GetComponentsInChildren<SpriteRenderer>();
            bush_mask = GetComponentInChildren<SpriteMask>();
            moved = true;
            other_moved = false;
            in_move_route = false;
            tile_activated = false;

            // Map Awareness Setup
            map_manager = FindObjectOfType<MapManager>();
            event_manager = FindObjectOfType<EventManager>();

            // Update Animator
            animator.SetInteger(Constants.DIRECTION_ANIMATION, (int)direction);
            if (stepping_animation)
                animator.SetBool(Constants.STEP_ANIMATION, true);
        }

        public void OnSpaceEntered()
        {
            other_moved = true;
        }

        private void Update()
        {
            // Update Animator
            animator.SetInteger(Constants.DIRECTION_ANIMATION, (int)direction);
            if (walking_animation && !stepping_animation)
                animator.SetBool(Constants.WALK_ANIMATION, moved);
            else if (stepping_animation)
                animator.SetBool(Constants.WALK_ANIMATION, true);
            if (invisible)
                foreach (SpriteRenderer sprite in sprites)
                    sprite.enabled = false;
            else
                foreach (SpriteRenderer sprite in sprites)
                    sprite.enabled = true;
            if (in_bush)
                bush_mask.enabled = true;
            else
                bush_mask.enabled = false;

            // Apply Movement
            if (moved)
            {
                // Activate the tile being moved onto
                if (!tile_activated)
                {
                    Vector3 move_dir = pos - transform.position;
                    tile_activated = ActivateTile(move_dir);
                }

                // Move in that direction
                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
            }

            // Tilemap and Event awareness check
            if ((other_moved || moved) && transform.position == pos)
            {
                // Get neighboring tiles
                AdvancedTileBase[] neighbor_tiles = map_manager.GetNeighborTiles(this);
                on_tile = neighbor_tiles[0];
                up_tile = neighbor_tiles[1];
                left_tile = neighbor_tiles[2];
                right_tile = neighbor_tiles[3];
                down_tile = neighbor_tiles[4];

                // Notify old neighbor events
                foreach (Event e in neighbor_events)
                {
                    if (e != null)
                    {
                        MoveableCharacter character = e.GetComponent<MoveableCharacter>();
                        if (character != null)
                            character.other_moved = true;
                    }
                }

                // Get neighboring events
                neighbor_events = event_manager.GetNeighborEvents(this);
                on_event = neighbor_events[0];
                up_event = neighbor_events[1];
                left_event = neighbor_events[2];
                right_event = neighbor_events[3];
                down_event = neighbor_events[4];

                // Notify new neighbor events
                foreach (Event e in neighbor_events) {
                    if (e != null)
                    {
                        MoveableCharacter character = e.GetComponent<MoveableCharacter>();
                        if (character != null)
                            character.other_moved = true;
                    }
                }

                moved = false;
                other_moved = false;
                if (!tile_activated)
                    tile_activated = ActivateTile(on_tile);
            }
        }

        #endregion


        #region Move Routing

        public bool GetInMoveRoute()
        {
            return in_move_route;
        }

        public IEnumerator StartMoveRoute(MoveCommands[] moves)
        {
            in_move_route = true;

            foreach (MoveCommands move in moves)
            {
                switch (move)
                {
                    case MoveCommands.TurnUp:
                        TurnUp();
                        yield return new WaitForSeconds(1 / speed);
                        break;
                    case MoveCommands.TurnLeft:
                        TurnLeft();
                        yield return new WaitForSeconds(1 / speed);
                        break;
                    case MoveCommands.TurnRight:
                        TurnRight();
                        yield return new WaitForSeconds(1 / speed);
                        break;
                    case MoveCommands.TurnDown:
                        TurnDown();
                        yield return new WaitForSeconds(1 / speed);
                        break;
                    case MoveCommands.MoveUp:
                        MoveUp(); break;
                    case MoveCommands.MoveLeft:
                        MoveLeft(); break;
                    case MoveCommands.MoveRight:
                        MoveRight(); break;
                    case MoveCommands.MoveDown:
                        MoveDown(); break;
                    default: break;
                }

                yield return new WaitUntil(() => !moved);
            }

            in_move_route = false;
        }

        #endregion


        #region Player Controller Support

        public Vector3 GetCurrentPos()
        {
            return transform.position;
        }

        public Vector3 GetTargetPos()
        {
            return pos;
        }

        #endregion


        #region Map Interfacing

        private bool ActivateTile(AdvancedTileBase tile)
        {
            // No tile, must be moving through walls
            if (tile == null)
                return true;

            // Bush Flag
            if (tile.is_bush)
            {
                in_bush = true;
                StartCoroutine(map_manager.GrassRustle(pos));
            }
            else
                in_bush = false;

            return true;
        }
        private bool ActivateTile(Vector3 move_dir)
        {
            if (Mathf.Abs(move_dir.x) > 0)
            {
                // Move Right
                if (move_dir.x > 0)
                {
                    // Move Off Left Stairs
                    if (on_tile.terrain_tag == TerrainTags.StairLeft)
                    {
                        CancelMovement();
                        MoveLayerDown();
                        MoveDownRight();
                        return true;
                    }
                    // Move Into Bush
                    else if (right_tile.is_bush && move_dir.x < 0.25f)
                        return ActivateTile(right_tile);
                    // Move Out of Bush
                    else if (on_tile.is_bush && !right_tile.is_bush && move_dir.x < 0.25f)
                        return ActivateTile(right_tile);
                }
                // Move Left
                else if (move_dir.x < 0)
                {
                    // Move Onto Left Stairs
                    if (left_tile.terrain_tag == TerrainTags.StairLeft)
                    {
                        CancelMovement();
                        MoveLayerUp();
                        MoveUpLeft();
                        return true;
                    }
                    // Move Into Bush
                    else if (left_tile.is_bush && move_dir.x > -0.25f)
                        return ActivateTile(left_tile);
                    // Move Out of Bush
                    else if (on_tile.is_bush && !left_tile.is_bush && move_dir.x > -0.25f)
                        return ActivateTile(left_tile);
                }
            }
            else if (Mathf.Abs(move_dir.y) > 0)
            {
                // Move Up
                if (move_dir.y > 0)
                {
                    // Move Into Bush
                    if (up_tile.is_bush && move_dir.y == 0)
                        return ActivateTile(up_tile);
                    // Move Out of Bush
                    else if (on_tile.is_bush && !up_tile.is_bush && move_dir.y < 0.5f)
                        return ActivateTile(up_tile);
                    // Move Off Up Stairs
                    else if (on_tile.terrain_tag == TerrainTags.StairUp)
                    {
                        MoveLayerUp();
                        return ActivateTile(up_tile);
                    }
                }
                // Move Down
                else if (move_dir.y < 0)
                {
                    // Move Into Bush
                    if (down_tile.is_bush && move_dir.y > -0.5f)
                        return ActivateTile(down_tile);
                    // Move Out of Bush
                    else if (on_tile.is_bush && !down_tile.is_bush && move_dir.y > -0.5f)
                        return ActivateTile(down_tile);
                    // Move Onto Up Stairs
                    else if (down_tile.terrain_tag == TerrainTags.StairUp)
                    {
                        MoveLayerDown();
                        return ActivateTile(down_tile);
                    }
                }
            }

            return false;
        }

        public void ActivateEvent()
        {
            if (on_event != null && on_event.trigger == Triggers.ActionButton)
                StartCoroutine(on_event.PlayEvent());
            else if (direction == Directions.Up && up_event != null && up_event.trigger == Triggers.ActionButton)
                StartCoroutine(up_event.PlayEvent());
            else if (direction == Directions.Left && left_event != null && left_event.trigger == Triggers.ActionButton)
                StartCoroutine(left_event.PlayEvent());
            else if (direction == Directions.Right && right_event != null && right_event.trigger == Triggers.ActionButton)
                StartCoroutine(right_event.PlayEvent());
            else if (direction == Directions.Down && down_event != null && down_event.trigger == Triggers.ActionButton)
                StartCoroutine(down_event.PlayEvent());
        }

        #endregion


        #region Direction Facing

        public void TurnUp()
        {
            if (!fix_direction)
                direction = Directions.Up;
        }
        public void TurnLeft()
        {
            if (!fix_direction)
                direction = Directions.Left;
        }
        public void TurnRight()
        {
            if (!fix_direction)
                direction = Directions.Right;
        }
        public void TurnDown()
        {
            if (!fix_direction)
                direction = Directions.Down;
        }

        public void Turn90DegreesCCW()
        {
            switch (direction)
            {
                case Directions.Up:
                    TurnLeft(); break;
                case Directions.Left:
                    TurnDown(); break;
                case Directions.Right:
                    TurnUp(); break;
                case Directions.Down:
                    TurnRight(); break;
                default:
                    break;
            }
        }
        public void Turn90DegreesCW()
        {
            switch (direction)
            {
                case Directions.Up:
                    TurnRight(); break;
                case Directions.Left:
                    TurnUp(); break;
                case Directions.Right:
                    TurnDown(); break;
                case Directions.Down:
                    TurnLeft(); break;
                default:
                    break;
            }
        }
        public void Turn180Degrees()
        {
            switch (direction)
            {
                case Directions.Up:
                    TurnDown(); break;
                case Directions.Left:
                    TurnRight(); break;
                case Directions.Right:
                    TurnLeft(); break;
                case Directions.Down:
                    TurnUp(); break;
                default:
                    break;
            }
        }

        public void TurnAtRandom()
        {
            System.Random random = Utilities.Random.GetRandom();
            Directions new_direction = (Directions)random.Next(4);
            while (new_direction == direction)
            {
                new_direction = (Directions)UnityEngine.Random.Range(0, 3);
            }
            if (!fix_direction)
                direction = new_direction;
        }

        public void TurnTowardsPlayer()
        {
            MoveableCharacter player = event_manager.GetPlayerMover();
            Vector3 player_dir = player.transform.position - transform.position;
            if (Mathf.Abs(player_dir.x) > Mathf.Abs(player_dir.y))
            {
                if (player_dir.x > 0)
                    TurnRight();
                else
                    TurnLeft();
            }
            else
            {
                if (player_dir.y > 0)
                    TurnUp();
                else
                    TurnDown();
            }
        }

        #endregion


        #region Movement

        public void CancelMovement()
        {
            pos = transform.position;
        }

        public void MoveUp()
        {
            TurnUp();
            if (move_through_walls || (on_tile != null && up_tile != null &&
                (up_event == null || up_event.passable) && on_tile.up_passage &&
                up_tile.allow_passage && up_tile.down_passage))
            {
                pos += Vector3.up;
                moved = true;
                tile_activated = false;
            }
        }
        public void MoveLeft()
        {
            TurnLeft();
            if (move_through_walls || (on_tile != null && left_tile != null &&
                (left_event == null || left_event.passable) && on_tile.left_passage &&
                left_tile.allow_passage && left_tile.right_passage))
            {
                pos += Vector3.left;
                moved = true;
                tile_activated = false;
            }
        }
        public void MoveRight()
        {
            TurnRight();
            if (move_through_walls || on_tile.terrain_tag == TerrainTags.StairLeft ||
                (on_tile != null && right_tile != null &&
                (right_event == null || right_event.passable) && on_tile.right_passage &&
                right_tile.allow_passage && right_tile.left_passage))
            {
                pos += Vector3.right;
                moved = true;
                tile_activated = false;
            }
        }
        public void MoveDown()
        {
            TurnDown();
            if (move_through_walls || (on_tile != null && down_tile != null &&
                (down_event == null || down_event.passable) && on_tile.down_passage &&
                down_tile.allow_passage && down_tile.up_passage))
            {
                pos += Vector3.down;
                moved = true;
                tile_activated = false;
            }
        }

        public void MoveUpLeft()
        {
            TurnLeft();
            pos += Vector3.up + Vector3.left;
            moved = true;
            tile_activated = false;
        }
        public void MoveDownRight()
        {
            TurnRight();
            pos += Vector3.down + Vector3.right;
            moved = true;
            tile_activated = false;
        }

        public void StepForward()
        {
            switch (direction)
            {
                case Directions.Up:
                    MoveUp();
                    break;
                case Directions.Left:
                    MoveLeft();
                    break;
                case Directions.Right:
                    MoveRight();
                    break;
                case Directions.Down:
                    MoveDown();
                    break;
                default:
                    break;
            }
            moved = true;
        }
        public void StepBackward()
        {
            switch (direction)
            {
                case Directions.Up:
                    FixDirectionOn();
                    MoveDown();
                    FixDirectionOff();
                    break;
                case Directions.Left:
                    FixDirectionOn();
                    MoveRight();
                    FixDirectionOff();
                    break;
                case Directions.Right:
                    FixDirectionOn();
                    MoveLeft();
                    FixDirectionOff();
                    break;
                case Directions.Down:
                    FixDirectionOn();
                    MoveUp();
                    FixDirectionOff();
                    break;
                default:
                    break;
            }
            moved = true;
        }

        public void MoveAtRandom()
        {
            System.Random random = Utilities.Random.GetRandom();
            Directions new_direction = (Directions)random.Next(4);
            switch (new_direction)
            {
                case Directions.Up:
                    MoveUp();
                    break;
                case Directions.Left:
                    MoveLeft();
                    break;
                case Directions.Right:
                    MoveRight();
                    break;
                case Directions.Down:
                    MoveDown();
                    break;
                default:
                    break;
            }
            moved = true;
        }

        public void JumpInPlace() { }
        public void JumpForward(int num_tiles) { }
        public void JumpBackward(int num_tiles) { }

        #endregion


        #region Layer Movement

        public void MoveLayerUp()
        {
            pos += (0.5f * Vector3.back);
            moved = true;
            tile_activated = false;
        }
        public void MoveLayerDown()
        {
            pos += (0.5f * Vector3.forward);
            moved = true;
            tile_activated = false;
        }

        #endregion


        #region Change Flags

        public void InvisibleOn()
        {
            invisible = true;
        }
        public void InvisibleOff()
        {
            invisible = false;
        }

        public void MoveThroughWallsOn()
        {
            move_through_walls = true;
        }
        public void MoveThroughWallsOff()
        {
            move_through_walls = false;
        }

        public void FixDirectionOn()
        {
            fix_direction = true;
        }
        public void FixDirectionOff()
        {
            fix_direction = false;
        }

        public void WalkingAnimationOn()
        {
            walking_animation = true;
        }
        public void WalkingAnimationOff()
        {
            walking_animation = true;
        }

        public void SteppingAnimationOn()
        {
            stepping_animation = true;
        }
        public void SteppingAnimationOff()
        {
            stepping_animation = false;
        }

        #endregion


        #region Other

        public IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
        public void ChangeSpeed(MovementSpeeds new_speed)
        {
            movement_speed = new_speed;
            speed = Constants.SPEEDS[(int)movement_speed];
        }

        #endregion
    }
}