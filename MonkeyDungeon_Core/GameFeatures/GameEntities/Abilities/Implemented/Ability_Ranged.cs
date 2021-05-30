using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Core.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_Ranged : GameEntity_Combat_Ability
    {
        protected string Ranged_Particle_Type { get; set; }
            
        public Ability_Ranged(
            string name, 
            string resourceName, 
            string statName,
            string particleType,
            Combat_Damage_Type relevantDamageType = Combat_Damage_Type.Abstract, 
            bool requiresTarget = false) 
            : base(name, resourceName, statName)
        {
            Ranged_Particle_Type = particleType;
        }

        protected override void Handle_Begin_Ability_Resolution(Combat_Action combatAction)
        {
            Combat_GameState combat = Entity.Game.Get_GameState<Combat_GameState>();
            
            combat.Act_Ranged_Attack(
                combatAction.Action_Owner.Scene_GameObject_ID,
                combatAction.Target.Get_Targets()[0],
                Ranged_Particle_Type
                );
        }
    }
}
