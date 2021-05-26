using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.Abilities
{
    public class Ability_BanannaRang : Ability_Ranged
    {
        Random rand = new Random();

        public Ability_BanannaRang() 
            : base(MD_VANILLA_ABILITYNAMES.ABILITY_BANNANA_RANG, MD_VANILLA_RESOURCES.RESOURCE_STAMINA, MD_VANILLA_STATS.STAT_AGILITY, MD_VANILLA_PARTICLES.BANNANA_RANG, DamageType.Physical, true)
        {
        }

        protected override void Handle_AbilityUsage(Combat_Action combatAction)
        {
            base.Handle_AbilityUsage(combatAction);
            GameEntity target = Entity.Game.Get_Entity(combatAction.Target_ID);

            target.Damage_This(
                new Combat_Damage(
                    DamageType.Physical,
                    Get_RelevantOutput() * 0.15
                    )
                );
        }

        protected override double Get_AbilityResourceCost()
        {
            float cost = 6 - ((Entity.Level > 12) ? 3 : Entity.Level / 4);
            return cost;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_BanannaRang();
        }
    }
}
