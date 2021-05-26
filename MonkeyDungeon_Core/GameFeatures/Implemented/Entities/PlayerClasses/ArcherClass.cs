using MonkeyDungeon_Core.GameFeatures;
using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.Abilities;
using MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.Entities.PlayerClasses
{
    public class ArcherClass : GameEntity
    {
        public static readonly string   CLASS_NAME                          =   "Archer";
        public static readonly int      CLASS_ID                            =   2       ;

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
        
        public ArcherClass(string name, int level, GameEntity_Controller controller)
            : base(
                  MD_VANILLA_RACES.PLAYER_RACE,
                  name,
                  level,
                  CLASS_ID,
                  new List<GameEntity_Stat>()
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
                  new List<GameEntity_Resource>()
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
                  new List<GameEntity_Ability>()
                  {
                      new Ability_Punch(),
                      new Ability_PoopyFling(),
                      new Ability_BanannaRang()
                  },
                  new List<GameEntity_Resistance>()
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
                  },
                  controller)
        { }
    }
}
