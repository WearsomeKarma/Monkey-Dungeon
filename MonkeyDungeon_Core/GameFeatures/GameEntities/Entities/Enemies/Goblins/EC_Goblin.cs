using System.Collections.Generic;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Controllers;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats.Implemented;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Entities
{
    public class EC_Goblin : GameEntity_ServerSide
    {
        public static readonly int DEFAULT_UNIQUE_ID = 0;
        
        public static readonly float    STAT_BASE_STRENGTH                  =   8       ;
        public static readonly float    STAT_BASE_AGILITY                   =   10      ;
        public static readonly float    STAT_BASE_SMARTYPANTS               =   8       ;
        public static readonly float    STAT_BASE_STINKINESS                =   14      ;

        public static readonly float    STAT_PROGRESSION_STRENGTH           =   0.05f   ;
        public static readonly float    STAT_PROGRESSION_AGILITY            =   0.10f   ;
        public static readonly float    STAT_PROGRESSION_SMARTYPANTS        =   0.05f   ;
        public static readonly float    STAT_PROGRESSION_STINKINESS         =   0.00f   ;

        public static readonly float    RESOURCE_BASE_HEALTH                =   14       ;
        public static readonly float    RESOURCE_BASE_STAMINA               =   14      ;
        public static readonly float    RESOURCE_BASE_MANA                  =   4       ;

        public static readonly float    RESOURCE_BASE_REGENERATION_HEALTH   =   0.05f   ;
        public static readonly float    RESOURCE_BASE_REGENERATION_STAMINA  =   0.10f   ;
        public static readonly float    RESOURCE_BASE_REGENERATION_MANA     =   0.05f   ;

        public static readonly float    RESOURCE_PROGRESSION_HEALTH         =   1       ;
        public static readonly float    RESOURCE_PROGRESSION_STAMINA        =   2       ;
        public static readonly float    RESOURCE_PROGRESSION_MANA           =   1       ;

        public static readonly float    RESISTANCE_BASE_PHYSICAL            =   1       ;
        public static readonly float    RESISTANCE_BASE_MAGICAL             =   1       ;
        public static readonly float    RESISTANCE_BASE_POISON              =   0.75f   ;

        public EC_Goblin(int level) 
            : base(
                  MD_VANILLA_RACE_NAMES.RACE_GOBLIN,
                  MD_VANILLA_RACE_NAMES.RACE_GOBLIN,
                  level,
                  DEFAULT_UNIQUE_ID,
                  new List<GameEntity_Stat>()
                  {
                      new Strength(
                          STAT_BASE_STRENGTH
                          ),
                      new Agility(
                          STAT_BASE_AGILITY
                          ),
                      new Smartypants(
                          STAT_BASE_SMARTYPANTS
                          ),
                      new Stinkiness(
                          STAT_BASE_STINKINESS
                          ),
                  },
                  new List<GameEntity_Resource>()
                  {
                      new Health(
                          0,
                          RESOURCE_BASE_HEALTH
                          ),
                      new Stamina(
                          RESOURCE_BASE_STAMINA
                          ),
                      new Mana(
                          RESOURCE_BASE_MANA
                          )
                  },
                  new List<GameEntity_Ability>()
                  {
                      new Ability_Punch()
                  },
                  /*new List<GameEntity_Resistance>()
                  {
                      new GameEntity_Resistance(
                          DamageType.Physical,
                          RESISTANCE_BASE_PHYSICAL
                          ),
                      new GameEntity_Resistance(
                          DamageType.Magical,
                          RESISTANCE_BASE_MAGICAL
                          ),
                      new GameEntity_Resistance(
                          DamageType.Poison,
                          RESISTANCE_BASE_POISON
                          )
                  },*/

                  new GameEntity_Controller_AI()
                  )
        {
        }
    }
}
