using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.Abilities
{
    public class Ability_ApeShit : Ability
    {
        public static readonly string NAME_APE_SHIT = "Ape Shit";

        public Ability_ApeShit() 
            : base(NAME_APE_SHIT, ENTITY_RESOURCES.STAMINA, ENTITY_STATS.STRENGTH)
        {
        }

        protected override float Get_AbilityResourceCost()
        {
            return 0;
            float cost = 8 - ((Entity.Level > 12) ? 4 : Entity.Level / 3);
            return cost;
        }
    }
}
