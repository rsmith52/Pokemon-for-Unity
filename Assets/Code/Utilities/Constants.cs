using UnityEngine;

namespace Utilities
{
    public static class Constants
    {
        #region Scenes

        public static readonly string BATTLE_SCENE = "Battle";
        public static readonly string MAP_MAKING_SCENE = "Map Making";

        #endregion


        #region Movement

        // Movement Speeds and Turn Sensitivity
        public static readonly float[] SPEEDS = new float[]
        {
            1f,         // VerySlow
            2f,         // Slow
            3f,         // Moderate
            4.5f,       // Fast
            6f          // VeryFast
        };
        public static readonly float TAP_VS_HOLD_TIME = 0.075f;

        // Movement Animation Variables
        public static readonly string DIRECTION_ANIMATION = "Direction";
        public static readonly string WALK_ANIMATION = "Walk";
        public static readonly string STEP_ANIMATION = "Step";
        public static readonly string RUN_ANIMATION = "Run";

        #endregion


        #region Pokemon

        public static readonly int MAX_LEVEL = 100;
        public static readonly int PARTY_SIZE = 6;
        public static readonly int NUM_MARKINGS = 6;
        public static readonly int NUM_MOVES = 4;

        public static readonly string MALE = "♂";
        public static readonly string FEMALE = "♀";
        public static readonly string MALE_TEXT = "<color=blue>" + MALE + "</color>";
        public static readonly string FEMALE_TEXT = "<color=red>" + FEMALE + "</color>";

        public static readonly float ICON_ANIM_TIME = 0.25f;
        public static readonly float SPRITE_ANIM_TIME = 0.05f;

        #endregion


        #region UI

        // Text Colors
        public static readonly Color32 LIGHT_BG_TEXT_COLOR = new Color32(50, 50, 50, 255);
        public static readonly Color32 DARK_BG_TEXT_COLOR = new Color32(255, 255, 255, 255);

        // Text Speeds
        public static readonly float[] TEXT_SPEEDS = new float[]
        {
            15f,        // Slow
            25f,        // Moderate
            35f,        // Fast
        };
        public static readonly float TEXT_FAST_FORWARD_SPEED = 50f;

        #endregion


        #region Utilities

        public static readonly string[] MONTHS = new string[]
        {
            "Jan.", "Feb.", "Mar.", "Apr.", "May", "Jun.", "Jul.", "Aug.", "Sep.", "Oct.", "Nov.", "Dec."
        };

        #endregion

    }
}