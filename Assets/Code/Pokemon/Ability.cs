﻿using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    #region Enums

    public enum Abilities
    {
        None,
        // Generation 1-5
        Adaptability,
        Aftermath,
        AirLock,
        Analytic,
        AngerPoint,
        Anticipation,
        ArenaTrap,
        BadDreams,
        BattleArmor,
        BigPecks,
        Blaze,
        Chlorophyll,
        ClearBody,
        CloudNine,
        ColorChange,
        CompoundEyes,
        Contrary,
        CursedBody,
        CuteCharm,
        Damp,
        Defeatist,
        Defiant,
        Download,
        Drizzle,
        Drought,
        DrySkin,
        EarlyBird,
        EffectSpore,
        Filter,
        FlameBody,
        FlareBoost,
        FlashFire,
        FlowerGift,
        Forecast,
        Forewarn,
        FriendGuard,
        Frisk,
        Gluttony,
        Guts,
        Harvest,
        Healer,
        Heatproof,
        HeavyMetal,
        HoneyGather,
        HugePower,
        Hustle,
        Hydration,
        HyperCutter,
        IceBody,
        Illuminate,
        Illusion,
        Immunity,
        Imposter,
        Infiltrator,
        InnerFocus,
        Insomnia,
        Intimidate,
        IronBarbs,
        IronFist,
        Justified,
        KeenEye,
        Klutz,
        LeafGuard,
        Levitate,
        LightMetal,
        LightningRod,
        Limber,
        LiquidOoze,
        MagicBounce,
        MagicGuard,
        MagmaArmor,
        MagnetPull,
        MarvelScale,
        Minus,
        MoldBreaker,
        Moody,
        MotorDrive,
        Moxie,
        Multiscale,
        Multitype,
        Mummy,
        NaturalCure,
        NoGuard,
        Normalize,
        Oblivious,
        Overcoat,
        Overgrow,
        OwnTempo,
        Pickpocket,
        Pickup,
        Plus,
        PoisonHeal,
        PoisonPoint,
        PoisonTouch,
        Prankster,
        Pressure,
        PurePower,
        QuickFeet,
        RainDish,
        Rattled,
        Reckless,
        Regenerator,
        Rivalry,
        RockHead,
        RoughSkin,
        RunAway,
        SandForce,
        SandRush,
        SandStream,
        SandVeil,
        SapSipper,
        Scrappy,
        SereneGrace,
        ShadowTag,
        ShedSkin,
        SheerForce,
        ShellArmor,
        ShieldDust,
        Simple,
        SkillLink,
        SlowStart,
        Sniper,
        SnowCloak,
        SnowWarning,
        SolarPower,
        SolidRock,
        Soundproof,
        SpeedBoost,
        Stall,
        Static,
        Steadfast,
        Stench,
        StickyHold,
        StormDrain,
        Sturdy,
        SuctionCups,
        SuperLuck,
        Swarm,
        SwiftSwim,
        Synchronize,
        TangledFeet,
        Technician,
        Telepathy,
        Teravolt,
        ThickFat,
        TintedLens,
        Torrent,
        ToxicBoost,
        Trace,
        Truant,
        Turboblaze,
        Unaware,
        Unburden,
        Unnerve,
        VictoryStar,
        VitalSpirit,
        VoltAbsorb,
        WaterAbsorb,
        WaterVeil,
        WeakArmor,
        WhiteSmoke,
        WonderGuard,
        WonderSkin,
        ZenMode,
        // Generation 6
        Aerilate,
        AromaVeil,
        AuraBreak,
        BulletProof,
        CheekPouch,
        Competitive,
        DarkAura,
        DeltaStream,
        DesolateLand,
        FairyAura,
        FlowerVeil,
        FurCoat,
        GaleWings,
        Gooey,
        GrassPelt,
        Magician,
        MegaLauncher,
        ParentalBond,
        Pixilate,
        PrimordialSea,
        Protean,
        Refrigerate,
        StanceChange,
        StrongJaw,
        SweetVeil,
        Symbiosis,
        ToughClaws,
        // Generation 7
        Battery,
        BattleBond,
        BeastBoost,
        Berserk,
        Comatose,
        Corrosion,
        Dancer,
        Dazzling,
        Disguise,
        ElectricSurge,
        EmergencyExit,
        Fluffy,
        FullMetalBody,
        Galvanize,
        GrassySurge,
        InnardsOut,
        LiquidVoice,
        LongReach,
        Merciless,
        Neuroforce,
        MistySurge,
        PowerConstruct,
        PowerOfAlchemy,
        PrismArmor,
        PsychicSurge,
        QueenlyMajesty,
        Receiver,
        RKSSystem,
        Schooling,
        ShadowShield,
        ShieldsDown,
        SlushRush,
        SoulHeart,
        Stakeout,
        Stamina,
        Steelworker,
        SurgeSurfer,
        TanglingHair,
        Triage,
        WaterBubble,
        WaterCompaction,
        WimpOut
    }

    #endregion

    [CreateAssetMenu(fileName = "Ability", menuName = "Pokemon/Ability")]
    public class Ability: ScriptableObject
    {
        #region Fields

        public static Dictionary<Abilities, Ability> abilities;

        #endregion


        #region Scriptable Object

        public string description;

        #endregion


        #region Static Methods

        public static Abilities GetAbilityFromID(uint pokemon_id, Abilities[] abilities)
        {
            if (abilities.Length == 1)
                return abilities[0];

            uint ability_id = pokemon_id % 2;
            return abilities[ability_id];
        }

        public static bool TriggersOnBattleStart(Abilities ability)
        {
            // TODO: Add all abilities that trigger on battle start
            List<Abilities> battle_start_abilities = new List<Abilities>()
            {
                Abilities.Intimidate
            };
            if (battle_start_abilities.Contains(ability)) {
                return true;
            }
            else return false;
        }

        #endregion

    }

}