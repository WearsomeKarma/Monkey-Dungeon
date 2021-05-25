using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.Abilities
{
    public class Ability_Ranged : GameEntity_Ability
    {
        protected string Ranged_Particle_Type { get; set; }
            
        public Ability_Ranged(
            string name, 
            string resourceName, 
            string statName,
            string particleType,
            DamageType relevantDamageType = DamageType.Abstract, 
            bool requiresTarget = false) 
            : base(name, resourceName, statName, relevantDamageType, requiresTarget)
        {
            Ranged_Particle_Type = particleType;
        }

        protected override void Handle_AbilityUsage(Combat_Action combatAction)
        {
            Combat_GameState combat = Entity.Game.Get_GameState<Combat_GameState>();

            combat.Act_Ranged_Attack(
                combatAction.Owner_OfCombatAction.Scene_GameObject_ID,
                combatAction.Target_ID,
                Ranged_Particle_Type
                );
        }
    }
}
