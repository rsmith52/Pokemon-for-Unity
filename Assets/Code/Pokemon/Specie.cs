using System;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Utilities;
using System.IO;

namespace Pokemon
{
    #region Enums

    public enum Species
    {
        None,
        // Generation 1
        Bulbasaur,
        Ivysaur,
        Venusaur,
        Charmander,
        Charmeleon,
        Charizard,
        Squirtle,
        Wartortle,
        Blastoise,
        Caterpie,
        Metapod,
        Butterfree,
        Weedle,
        Kakuna,
        Beedrill,
        Pidgey,
        Pidgeotto,
        Pidgeot,
        Rattata,
        Raticate,
        Spearow,
        Fearow,
        Ekans,
        Arbok,
        Pikachu,
        Raichu,
        Sandshrew,
        Sandslash,
        NidoranF,
        Nidorina,
        Nidoqueen,
        NidoranM,
        Nidorino,
        Nidoking,
        Clefairy,
        Clefable,
        Vulpix,
        Ninetales,
        Jigglypuff,
        Wigglytuff,
        Zubat,
        Golbat,
        Oddish,
        Gloom,
        Vileplume,
        Paras,
        Parasect,
        Venonat,
        Venomoth,
        Diglett,
        Dugtrio,
        Meowth,
        Persian,
        Psyduck,
        Golduck,
        Mankey,
        Primeape,
        Growlithe,
        Arcanine,
        Poliwag,
        Poliwhirl,
        Poliwrath,
        Abra,
        Kadabra,
        Alakazam,
        Machop,
        Machoke,
        Machamp,
        Bellsprout,
        Weepinbell,
        Victreebel,
        Tentacool,
        Tentacruel,
        Geodude,
        Graveler,
        Golem,
        Ponyta,
        Rapidash,
        Slowpoke,
        Slowbro,
        Magnemite,
        Magneton,
        Farfetchd,
        Doduo,
        Dodrio,
        Seel,
        Dewgong,
        Grimer,
        Muk,
        Shellder,
        Cloyster,
        Gastly,
        Haunter,
        Gengar,
        Onix,
        Drowzee,
        Hypno,
        Krabby,
        Kingler,
        Voltorb,
        Electrode,
        Exeggcute,
        Exeggutor,
        Cubone,
        Marowak,
        Hitmonlee,
        Hitmonchan,
        Lickitung,
        Koffing,
        Weezing,
        Rhyhorn,
        Rhydon,
        Chansey,
        Tangela,
        Kangaskhan,
        Horsea,
        Seadra,
        Goldeen,
        Seaking,
        Staryu,
        Starmie,
        MrMime,
        Scyther,
        Jynx,
        Electabuzz,
        Magmar,
        Pinsir,
        Tauros,
        Magikarp,
        Gyarados,
        Lapras,
        Ditto,
        Eevee,
        Vaporeon,
        Jolteon,
        Flareon,
        Porygon,
        Omanyte,
        Omastar,
        Kabuto,
        Kabutops,
        Aerodactyl,
        Snorlax,
        Articuno,
        Zapdos,
        Moltres,
        Dratini,
        Dragonair,
        Dragonite,
        Mewtwo,
        Mew,
        // Generation 2
        Chikorita,
        Bayleef,
        Meganium,
        Cyndaquil,
        Quilava,
        Typhlosion,
        Totodile,
        Croconaw,
        Feraligatr,
        Sentret,
        Furret,
        Hoothoot,
        Noctowl,
        Ledyba,
        Ledian,
        Spinarak,
        Ariados,
        Crobat,
        Chinchou,
        Lanturn,
        Pichu,
        Cleffa,
        Igglybuff,
        Togepi,
        Togetic,
        Natu,
        Xatu,
        Mareep,
        Flaaffy,
        Ampharos,
        Bellossom,
        Marill,
        Azumarill,
        Sudowoodo,
        Politoed,
        Hoppip,
        Skiploom,
        Jumpluff,
        Aipom,
        Sunkern,
        Sunflora,
        Yanma,
        Wooper,
        Quagsire,
        Espeon,
        Umbreon,
        Murkrow,
        Slowking,
        Misdreavus,
        Unown,
        Wobbuffet,
        Girafarig,
        Pineco,
        Forretress,
        Dunsparce,
        Gligar,
        Steelix,
        Snubbull,
        Granbull,
        Qwilfish,
        Scizor,
        Shuckle,
        Heracross,
        Sneasel,
        Teddiursa,
        Ursaring,
        Slugma,
        Magcargo,
        Swinub,
        Piloswine,
        Corsola,
        Remoraid,
        Octillery,
        Delibird,
        Mantine,
        Skarmory,
        Houndour,
        Houndoom,
        Kingdra,
        Phanpy,
        Donphan,
        Porygon2,
        Stantler,
        Smeargle,
        Tyrogue,
        Hitmontop,
        Smoochum,
        Elekid,
        Magby,
        Miltank,
        Blissey,
        Raikou,
        Entei,
        Suicune,
        Larvitar,
        Pupitar,
        Tyranitar,
        Lugia,
        HoOh,
        Celebi,
        // Generation 3
        Treecko,
        Grovyle,
        Sceptile,
        Torchic,
        Combusken,
        Blaziken,
        Mudkip,
        Marshtomp,
        Swampert,
        Poochyena,
        Mightyena,
        Zigzagoon,
        Linoone,
        Wurmple,
        Silcoon,
        Beautifly,
        Cascoon,
        Dustox,
        Lotad,
        Lombre,
        Ludicolo,
        Seedot,
        Nuzleaf,
        Shiftry,
        Taillow,
        Swellow,
        Wingull,
        Pelipper,
        Ralts,
        Kirlia,
        Gardevoir,
        Surskit,
        Masquerain,
        Shroomish,
        Breloom,
        Slakoth,
        Vigoroth,
        Slaking,
        Nincada,
        Ninjask,
        Shedinja,
        Whismur,
        Loudred,
        Exploud,
        Makuhita,
        Hariyama,
        Azurill,
        Nosepass,
        Skitty,
        Delcatty,
        Sableye,
        Mawile,
        Aron,
        Lairon,
        Aggron,
        Meditite,
        Medicham,
        Electrike,
        Manectric,
        Plusle,
        Minun,
        Volbeat,
        Illumise,
        Roselia,
        Gulpin,
        Swalot,
        Carvanha,
        Sharpedo,
        Wailmer,
        Wailord,
        Numel,
        Camerupt,
        Torkoal,
        Spoink,
        Grumpig,
        Spinda,
        Trapinch,
        Vibrava,
        Flygon,
        Cacnea,
        Cacturne,
        Swablu,
        Altaria,
        Zangoose,
        Seviper,
        Lunatone,
        Solrock,
        Barboach,
        Whiscash,
        Corphish,
        Crawdaunt,
        Baltoy,
        Claydol,
        Lileep,
        Cradily,
        Anorith,
        Armaldo,
        Feebas,
        Milotic,
        Castform,
        Kecleon,
        Shuppet,
        Banette,
        Duskull,
        Dusclops,
        Tropius,
        Chimecho,
        Absol,
        Wynaut,
        Snorunt,
        Glalie,
        Spheal,
        Sealeo,
        Walrein,
        Clamperl,
        Huntail,
        Gorebyss,
        Relicanth,
        Luvdisc,
        Bagon,
        Shelgon,
        Salamence,
        Beldum,
        Metang,
        Metagross,
        Regirock,
        Regice,
        Registeel,
        Latias,
        Latios,
        Kyogre,
        Groudon,
        Rayquaza,
        Jirachi,
        Deoxys,
        // Generation 4
        Turtwig,
        Grotle,
        Torterra,
        Chimchar,
        Monferno,
        Infernape,
        Piplup,
        Prinplup,
        Empoleon,
        Starly,
        Staravia,
        Staraptor,
        Bidoof,
        Bibarel,
        Kricketot,
        Kricketune,
        Shinx,
        Luxio,
        Luxray,
        Budew,
        Roserade,
        Cranidos,
        Rampardos,
        Shieldon,
        Bastiodon,
        Burmy,
        Wormadam,
        Mothim,
        Combee,
        Vespiquen,
        Pachirisu,
        Buizel,
        Floatzel,
        Cherubi,
        Cherrim,
        Shellos,
        Gastrodon,
        Ambipom,
        Drifloon,
        Drifblim,
        Buneary,
        Lopunny,
        Mismagius,
        Honchkrow,
        Glameow,
        Purugly,
        Chingling,
        Stunky,
        Skuntank,
        Bronzor,
        Bronzong,
        Bonsly,
        MimeJr,
        Happiny,
        Chatot,
        Spiritomb,
        Gible,
        Gabite,
        Garchomp,
        Munchlax,
        Riolu,
        Lucario,
        Hippopotas,
        Hippowdon,
        Skorupi,
        Drapion,
        Croagunk,
        Toxicroak,
        Carnivine,
        Finneon,
        Lumineon,
        Mantyke,
        Snover,
        Abomasnow,
        Weavile,
        Magnezone,
        Lickilicky,
        Rhyperior,
        Tangrowth,
        Electivire,
        Magmortar,
        Togekiss,
        Yanmega,
        Leafeon,
        Glaceon,
        Gliscor,
        Mamoswine,
        PorygonZ,
        Gallade,
        Probopass,
        Dusknoir,
        Froslass,
        Rotom,
        Uxie,
        Mesprit,
        Azelf,
        Dialga,
        Palkia,
        Heatran,
        Regigigas,
        Giratina,
        Cresselia,
        Phione,
        Manaphy,
        Darkrai,
        Shaymin,
        Arceus,
        // Generation 5
        Victini,
        Snivy,
        Servine,
        Serperior,
        Tepig,
        Pignite,
        Emboar,
        Oshawott,
        Dewott,
        Samurott,
        Patrat,
        Watchog,
        Lillipup,
        Herdier,
        Stoutland,
        Purrloin,
        Liepard,
        Pansage,
        Simisage,
        Pansear,
        Simisear,
        Panpour,
        Simipour,
        Munna,
        Musharna,
        Pidove,
        Tranquill,
        Unfezant,
        Blitzle,
        Zebstrika,
        Roggenrola,
        Boldore,
        Gigalith,
        Woobat,
        Swoobat,
        Drilbur,
        Excadrill,
        Audino,
        Timburr,
        Gurdurr,
        Conkeldurr,
        Tympole,
        Palpitoad,
        Seismitoad,
        Throh,
        Sawk,
        Sewaddle,
        Swadloon,
        Leavanny,
        Venipede,
        Whirlipede,
        Scolipede,
        Cottonee,
        Whimsicott,
        Petilil,
        Lilligant,
        Basculin,
        Sandile,
        Krokorok,
        Krookodile,
        Darumaka,
        Darmanitan,
        Maractus,
        Dwebble,
        Crustle,
        Scraggy,
        Scrafty,
        Sigilyph,
        Yamask,
        Cofagrigus,
        Tirtouga,
        Carracosta,
        Archen,
        Archeops,
        Trubbish,
        Garbodor,
        Zorua,
        Zoroark,
        Minccino,
        Cinccino,
        Gothita,
        Gothorita,
        Gothitelle,
        Solosis,
        Duosion,
        Reuniclus,
        Ducklett,
        Swanna,
        Vanillite,
        Vanillish,
        Vanilluxe,
        Deerling,
        Sawsbuck,
        Emolga,
        Karrablast,
        Escavalier,
        Foongus,
        Amoonguss,
        Frillish,
        Jellicent,
        Alomomola,
        Joltik,
        Galvantula,
        Ferroseed,
        Ferrothorn,
        Klink,
        Klang,
        Klinklang,
        Tynamo,
        Eelektrik,
        Eelektross,
        Elgyem,
        Beheeyem,
        Litwick,
        Lampent,
        Chandelure,
        Axew,
        Fraxure,
        Haxorus,
        Cubchoo,
        Beartic,
        Cryogonal,
        Shelmet,
        Accelgor,
        Stunfisk,
        Mienfoo,
        Mienshao,
        Druddigon,
        Golett,
        Golurk,
        Pawniard,
        Bisharp,
        Bouffalant,
        Rufflet,
        Braviary,
        Vullaby,
        Mandibuzz,
        Heatmor,
        Durant,
        Deino,
        Zweilous,
        Hydreigon,
        Larvesta,
        Volcarona,
        Cobalion,
        Terrakion,
        Virizion,
        Tornadus,
        Thundurus,
        Reshiram,
        Zekrom,
        Landorus,
        Kyurem,
        Keldeo,
        Meloetta,
        Genesect,
        // Generation 6
        Chespin,
        Quilladin,
        Chesnaught,
        Fennekin,
        Braixen,
        Delphox,
        Froakie,
        Frogadier,
        Greninja,
        Bunnelby,
        Diggersby,
        Fletchling,
        Fletchinder,
        Talonflame,
        Scatterbug,
        Spewpa,
        Vivillon,
        Litleo,
        Pyroar,
        Flabebe,
        Floette,
        Florges,
        Skiddo,
        Gogoat,
        Pancham,
        Pangoro,
        Furfrou,
        Espurr,
        Meowstic,
        Honedge,
        Doublade,
        Aegislash,
        Spritzee,
        Aromatisse,
        Swirlix,
        Slurpuff,
        Inkay,
        Malamar,
        Binacle,
        Barbaracle,
        Skrelp,
        Dragalge,
        Clauncher,
        Clawitzer,
        Helioptile,
        Heliolisk,
        Tyrunt,
        Tyrantrum,
        Amaura,
        Aurorus,
        Sylveon,
        Hawlucha,
        Dedenne,
        Carbink,
        Goomy,
        Sliggoo,
        Goodra,
        Klefki,
        Phantump,
        Trevenant,
        Pumpkaboo,
        Gourgeist,
        Bergmite,
        Avalugg,
        Noibat,
        Noivern,
        Xerneas,
        Yveltal,
        Zygarde,
        Diancie,
        Hoopa,
        Volcanion,
        // Generation 7
        Rowlet,
        Dartrix,
        Decidueye,
        Litten,
        Torracat,
        Incineroar,
        Popplio,
        Brionne,
        Primarina,
        Pikipek,
        Trumbeak,
        Toucannon,
        Yungoos,
        Gumshoos,
        Grubbin,
        Charjabug,
        Vikavolt,
        Crabrawler,
        Crabominable,
        Oricorio,
        Cutiefly,
        Ribombee,
        Rockruff,
        Lycanroc,
        Wishiwashi,
        Mareanie,
        Toxapex,
        Mudbray,
        Mudsdale,
        Dewpider,
        Araquanid,
        Fomantis,
        Lurantis,
        Morelull,
        Shiinotic,
        Salandit,
        Salazzle,
        Stufful,
        Bewear,
        Bounsweet,
        Steenee,
        Tsareena,
        Comfey,
        Oranguru,
        Passimian,
        Wimpod,
        Golisopod,
        Sandygast,
        Palossand,
        Pyukumuku,
        TypeNull,
        Silvally,
        Minior,
        Komala,
        Turtonator,
        Togedemaru,
        Mimikyu,
        Bruxish,
        Drampa,
        Dhelmise,
        JangmoO,
        HakamoO,
        KommoO,
        TapuKoko,
        TapuLele,
        TapuBulu,
        TapuFini,
        Cosmog,
        Cosmoem,
        Solgaleo,
        Lunala,
        Nihilego,
        Buzzwole,
        Pheromosa,
        Xurkitree,
        Celesteela,
        Kartana,
        Guzzlord,
        Necrozma,
        Magearna,
        Marshadow,
        Poipole,
        Naganadel,
        Stakataka,
        Blacephalon,
        Zeraora,
        Meltan,
        Melmetal
    }

    public enum EvolutionMethods
    {
        None,
        Level,
        LevelMale,
        LevelFemale,
        LevelDay,
        LevelNight,
        LevelMorning,
        LevelAfternoon,
        LevelEvening,
        LevelNoWeather,
        LevelSun,
        LevelRain,
        LevelSnow,
        LevelSandstorm,
        LevelCycling,
        LevelSurfing,
        LevelDiving,
        LevelDarkness,
        LevelDarkInParty,
        AttackGreater,
        AtkDefEqual,
        DefenseGreater,
        Silcoon,
        Cascoon,
        Ninjask,
        Shedinja,
        Happiness,
        HappinessMale,
        HappinessFemale,
        HappinessDay,
        HappinessNight,
        HappinessMove,
        HappinessMoveType,
        HappinessHoldItem,
        MaxHappiness,
        Beauty,
        HoldItem,
        HoldItemMale,
        HoldItemFemale,
        DayHoldItem,
        NightHoldItem,
        HoldItemHappiness,
        HasMove,
        HasMoveType,
        HasInParty,
        Location,
        Region,
        Item,
        ItemMale,
        ItemFemale,
        ItemDay,
        ItemNight,
        ItemHappiness,
        Trade,
        TradeMale,
        TradeFemale,
        TradeDay,
        TradeNight,
        TradeItem,
        TradeSpecies
    }

    #endregion


    #region Structs

    [Serializable]
    public struct LevelUpMove
    {
        public uint level;
        public Moves move;
    }

    [Serializable]
    public struct Evolution
    {
        public Species evolution;
        public EvolutionMethods evolution_method;
        public string parameter;
    }

    #endregion

    [CreateAssetMenu(fileName = "Species", menuName = "Pokemon/Species")]
    public class Specie : ScriptableObject
    {
        #region Fields

        public static Dictionary<Species, Specie> species;

        #endregion


        #region Scriptable Object

        [Header("Basic Info")]
        public uint national_dex;
        public Types[] types;
        public StatArray base_stats;
        public GenderRates gender_rate;
        public GrowthRates growth_rate;
        public uint base_exp;
        public StatArray effort_values;
        public uint rareness;
        public uint happiness;
        public Abilities[] abilities;
        public Abilities[] hidden_ability;
        public LevelUpMove[] moves;
        public Evolution[] evolutions;

        [Header("Breeding Info")]
        public Moves[] egg_moves;
        public EggGroups[] egg_groups;
        public uint steps_to_hatch;

        [Header("Pokedex Entry")]
        public float height;
        public float weight;
        public Colors color;
        public Shapes shape;
        public Habitats habitat;
        public uint[] regional_dexes;
        public string kind;
        public string pokedex_entry;

        [Header("Optional Info")]
        public Items.Items wild_item_common;
        public Items.Items wild_item_uncommon;
        public Items.Items wild_item_rare;
        public string form_name;
        public Items.Items incense;

        [Header("Battler Sprite Positioning")]
        public int battler_player_x;
        public int battler_player_y;
        public int battler_enemy_x;
        public int battler_enemy_y;
        public int battler_altitude;
        public int battler_shadow_x;
        public int battler_shadow_size;

        #endregion


        #region Static Methods

        public static Sprite GetPokemonIcon(uint dex_number, int frame = 0, uint form_id = 0)
        {
            string poke_icon_path = Settings.POKEMON_ICON_PATH;

            string formatted_number = dex_number.ToString().PadLeft(3, '0');
            if (form_id != 0)
                formatted_number += '_' + form_id.ToString();

            string file_name = Settings.POKEMON_ICON_PREFIX + formatted_number;
            string local_path = Path.GetFileName(poke_icon_path);
            string file_path = Path.Combine(local_path, file_name);

            try
            {
                return Resources.LoadAll<Sprite>(file_path)[frame];
            }
            catch
            {
                file_path = Path.Combine(local_path, Settings.POKEMON_MISSING_ICON);
                return Resources.LoadAll<Sprite>(file_path)[frame];
            }
        }

        public static Sprite GetPokemonSprite(uint dex_number, Genders gender = Genders.Male, bool is_shiny = false,
            bool is_back = false, uint form_id = 0)
        {
            string poke_sprite_path = Settings.POKEMON_SPRITE_PATH;
            string local_path = Path.GetFileName(poke_sprite_path);

            string file_name = dex_number.ToString().PadLeft(3, '0');

            // check for female form
            if (gender == Genders.Female)
            {
                string female_file_name = file_name + 'f';
                string female_file_path = Path.Combine(local_path, female_file_name);
                Sprite[] frames = Resources.LoadAll<Sprite>(female_file_path);
                if (frames.Length > 0)
                    file_name = female_file_name;
            }

            if (is_shiny)
                file_name += 's';
            if (is_back)
                file_name += 'b';
            if (form_id != 0)
                file_name += '_' + form_id.ToString();

            string file_path = Path.Combine(local_path, file_name);

            try
            {
                return Resources.LoadAll<Sprite>(file_path)[0];
            }
            catch
            {
                file_path = Path.Combine(local_path, Settings.POKEMON_MISSING_SPRITE);
                if (is_shiny)
                    file_path += 's';
                if (is_back)
                    file_path += 'b';
                return Resources.LoadAll<Sprite>(file_path)[0];
            }
        }

        public static Sprite[] GetPokemonAnimFrames(uint dex_number, Genders gender = Genders.Male, bool is_shiny = false,
            bool is_back = false, uint form_id = 0)
        {
            string poke_sprite_path = Settings.POKEMON_ANIM_SPRITE_PATH;
            string local_path = Path.GetFileName(poke_sprite_path);

            string file_name = dex_number.ToString().PadLeft(3, '0');

            // check for female form
            if (gender == Genders.Female)
            {
                string female_file_name = file_name + 'f';
                string female_file_path = Path.Combine(local_path, female_file_name);
                Sprite[] frames = Resources.LoadAll<Sprite>(female_file_path);
                if (frames.Length > 0)
                    file_name = female_file_name;
            }

            if (is_shiny)
                file_name += 's';
            if (is_back)
                file_name += 'b';
            if (form_id != 0)
                file_name += '_' + form_id.ToString();

            string file_path = Path.Combine(local_path, file_name);

            try
            {
                Sprite[] frames = Resources.LoadAll<Sprite>(file_path);
                Sprite test_access = frames[0];
                return frames;
            }
            catch
            {
                file_path = Path.Combine(local_path, Settings.POKEMON_MISSING_ANIM_SPRITE);
                if (is_shiny)
                    file_path += 's';
                if (is_back)
                    file_path += 'b';
                return Resources.LoadAll<Sprite>(file_path);
            }
        }

        #endregion

    }
}