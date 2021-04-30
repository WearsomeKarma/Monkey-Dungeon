using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.Implemented.Abilities;
using MonkeyDungeon.GameFeatures.Implemented.ActingEntities;
using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;

namespace MonkeyDungeon.Components.Implemented.Enemies.Goblins
{
    public class EC_Goblin : EntityComponent
    {
        public static readonly string DEFAULT_NAME = "Goblin";
        public static readonly string DEFAULT_RACE_NAME = "Goblin";
        public static readonly string DEFAULT_TYPE_NAME = "Enemy_Goblin";
        public static readonly int DEFAULT_UNIQUE_ID = 0;
        
        public static readonly float    STAT_BASE_STRENGTH                  =   8       ;
        public static readonly float    STAT_BASE_AGILITY                   =   10      ;
        public static readonly float    STAT_BASE_SMARTYPANTS               =   8       ;
        public static readonly float    STAT_BASE_STINKINESS                =   14      ;

        public static readonly float    STAT_PROGRESSION_STRENGTH           =   0.05f   ;
        public static readonly float    STAT_PROGRESSION_AGILITY            =   0.10f   ;
        public static readonly float    STAT_PROGRESSION_SMARTYPANTS        =   0.05f   ;
        public static readonly float    STAT_PROGRESSION_STINKINESS         =   0.00f   ;

        public static readonly float    RESOURCE_BASE_HEALTH                =   4       ;
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
                  DEFAULT_RACE_NAME, 
                  DEFAULT_NAME,
                  DEFAULT_TYPE_NAME,
                  level,
                  DEFAULT_UNIQUE_ID,
                  new List<EntityStat>()
                  {
                      new Strength(
                          STAT_BASE_STRENGTH,
                          STAT_PROGRESSION_STRENGTH
                          ),
                      new Agility(
                          STAT_BASE_AGILITY,
                          STAT_PROGRESSION_AGILITY
                          ),
                      new Smartypants(
                          STAT_BASE_SMARTYPANTS,
                          STAT_PROGRESSION_SMARTYPANTS
                          ),
                      new Stinkiness(
                          STAT_BASE_STINKINESS,
                          STAT_PROGRESSION_STINKINESS
                          ),
                  },
                  new List<EntityResource>()
                  {
                      new Health(
                          RESOURCE_BASE_HEALTH,
                          RESOURCE_BASE_HEALTH,
                          0,
                          RESOURCE_BASE_REGENERATION_HEALTH,
                          RESOURCE_PROGRESSION_HEALTH
                          ),
                      new Stamina(
                          RESOURCE_BASE_STAMINA,
                          RESOURCE_BASE_STAMINA,
                          RESOURCE_BASE_REGENERATION_STAMINA,
                          RESOURCE_PROGRESSION_STAMINA
                          ),
                      new Mana(
                          RESOURCE_BASE_MANA,
                          RESOURCE_BASE_MANA,
                          RESOURCE_BASE_REGENERATION_MANA,
                          RESOURCE_PROGRESSION_MANA
                          )
                  },
                  new List<Ability>()
                  {
                      new Ability_Punch()
                  },
                  new List<Resistance>()
                  {
                      new Resistance(
                          DamageType.Physical,
                          RESISTANCE_BASE_PHYSICAL
                          ),
                      new Resistance(
                          DamageType.Magical,
                          RESISTANCE_BASE_MAGICAL
                          ),
                      new Resistance(
                          DamageType.Poison,
                          RESISTANCE_BASE_POISON
                          )
                  },
                  new ActingEntity_AI()
                  )
        {
        }
    }
}
