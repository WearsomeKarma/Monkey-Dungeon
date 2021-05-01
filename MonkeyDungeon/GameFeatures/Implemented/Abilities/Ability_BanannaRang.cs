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
    public class Ability_BanannaRang : Ability
    {
        public static readonly string NAME_BANANNA_RANG = "Bananna Rang";

        public Ability_BanannaRang() 
            : base(NAME_BANANNA_RANG, ENTITY_RESOURCES.STAMINA, ENTITY_STATS.AGILITY, DamageType.Physical, true)
        {
        }

        protected override double Get_AbilityResourceCost()
        {
            float cost = 6 - ((Entity.Level > 12) ? 3 : Entity.Level / 4);
            return cost;
        }
    }
}
