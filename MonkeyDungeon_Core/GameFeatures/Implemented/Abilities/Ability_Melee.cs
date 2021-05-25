using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.Abilities
{
    public class Ability_Melee : GameEntity_Ability
    {
        public Ability_Melee(
            string name, 
            string resourceName, 
            string statName,
            DamageType relevantDamageType = DamageType.Abstract,
            bool requiresTarget = false) 
            : base(name, resourceName, statName, relevantDamageType, requiresTarget)
        {
        }

        protected override void Handle_AbilityUsage(Combat_Action combatAction)
        {
            Combat_GameState combat = Entity.Game.Get_GameState<Combat_GameState>();

            combat.Act_Melee_Attack(
                combatAction.Owner_OfCombatAction.Scene_GameObject_ID,
                combatAction.Target_ID
                );
        }
    }
}
