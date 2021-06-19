using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities.Implemented;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources.Implemented;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats.Implemented;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.Entities
{
    public class MonkClass : GameEntity_ServerSide
    {
        public static readonly string   CLASS_NAME                          =   "Monk"  ;
        public static readonly int      CLASS_ID                            =   5       ;

        public static readonly float    STAT_BASE_STRENGTH                  =   8       ;
        public static readonly float    STAT_BASE_AGILITY                   =   14      ;
        public static readonly float    STAT_BASE_SMARTYPANTS               =   10      ;
        public static readonly float    STAT_BASE_STINKINESS                =   10      ;

        public static readonly float    STAT_PROGRESSION_STRENGTH           =   0.05f   ;
        public static readonly float    STAT_PROGRESSION_AGILITY            =   0.20f   ;
        public static readonly float    STAT_PROGRESSION_SMARTYPANTS        =   0.10f   ;
        public static readonly float    STAT_PROGRESSION_STINKINESS         =   0.00f   ;

        public static readonly float    RESOURCE_BASE_HEALTH                =   10      ;
        public static readonly float    RESOURCE_BASE_STAMINA               =   10      ;
        public static readonly float    RESOURCE_BASE_MANA                  =   4       ;

        public static readonly float    RESOURCE_BASE_REGENERATION_HEALTH   =   0.1f    ;
        public static readonly float    RESOURCE_BASE_REGENERATION_STAMINA  =   0.15f   ;
        public static readonly float    RESOURCE_BASE_REGENERATION_MANA     =   0.05f   ;

        public static readonly float    RESOURCE_PROGRESSION_HEALTH         =   2       ;
        public static readonly float    RESOURCE_PROGRESSION_STAMINA        =   2       ;
        public static readonly float    RESOURCE_PROGRESSION_MANA           =   1       ;

        public static readonly float    RESISTANCE_BASE_PHYSICAL            =   1       ;
        public static readonly float    RESISTANCE_BASE_MAGICAL             =   1       ;
        public static readonly float    RESISTANCE_BASE_POISON              =   1       ;

        public MonkClass(string name, int level, GameEntity_ServerSide_Controller serverSideController)
            : base(
                  MD_VANILLA_RACE_NAMES.RACE_MONKEY,
                  name,
                  level,
                  CLASS_ID,
                  new List<GameEntity_ServerSide_Stat>()
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
                  new List<GameEntity_ServerSide_Resource>()
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
                  new List<GameEntity_ServerSide_Ability>()
                  {
                      new Ability_Punch(),
                      new Ability_PoopyFling(),
                      new Ability_KiKick()
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
                  serverSideController)
        { }
    }
}
