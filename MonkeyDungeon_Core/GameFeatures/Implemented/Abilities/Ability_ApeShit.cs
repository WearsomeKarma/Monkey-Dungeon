using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.Abilities
{
    public class Ability_ApeShit : GameEntity_Ability
    {
        public static readonly string NAME_APE_SHIT = "Ape Shit";

        public Ability_ApeShit() 
            : base(NAME_APE_SHIT, ENTITY_RESOURCES.STAMINA, ENTITY_STATS.STRENGTH)
        {
        }

        protected override double Get_AbilityResourceCost()
        {
            return 0;
            float cost = 8 - ((Entity.Level > 12) ? 4 : Entity.Level / 3);
            return cost;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_ApeShit();
        }
    }
}
