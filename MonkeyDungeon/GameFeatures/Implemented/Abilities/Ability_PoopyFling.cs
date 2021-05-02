using MonkeyDungeon.GameFeatures.CombatObjects;
using MonkeyDungeon.GameFeatures.EntityResourceManagement;
using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.Abilities
{
    public class Ability_PoopyFling : GameEntity_Ability
    {
        public static readonly string NAME_POOPY_FLING = "Poopy Fling";

        public Ability_PoopyFling() 
            : base(NAME_POOPY_FLING, ENTITY_RESOURCES.HEALTH, ENTITY_STATS.STINKINESS, DamageType.Poison, true)
        {
        }

        protected override double Get_AbilityResourceCost()
        {
            return Resource_Value * 0.05f;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_PoopyFling();
        }
    }
}
