using System;
using System.Collections.Generic;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Redirection : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity_ServerSide owner = Get_Entity(action.Action_Owner);

            GameEntity_Position[] targets = action.Target.Get_Reduced_Fields();

            Combat_Redirection_Chance[] chances = Get_Chances_For_Each_Target(
                action,
                targets,
                action.Selected_Ability,
                (GameEntity_Position_Type)owner.GameEntity_Position
            );
            
            Relay_UI_Event(owner, action);
        }

        private void Apply_Redirections()
        {
            
        }

        private Combat_Redirection_Chance[] Get_Chances_For_Each_Target(
            Combat_Action action,
            GameEntity_Position[] targets, 
            GameEntity_Ability usedAbility, 
            GameEntity_Position_Type ownerPositionType
            )
        {
            Combat_Assault_Type assaultType = usedAbility.Ability__Combat_Assault_Type;
            GameEntity_Position_Type targetPositionType;
            
            GameEntity_ServerSide target;
            List<Combat_Redirection_Chance> chances = new List<Combat_Redirection_Chance>();

            Combat_Redirection_Chance abilityChance;
            Combat_Redirection_Chance[] statusEffectChances;

            Combat_Redirection_Chance baseChance;
            
            for (int i = 0; i < targets.Length; i++)
            {
                target = Entity_Field.Get_Entity(targets[i]);
                targetPositionType = (GameEntity_Position_Type) target.GameEntity_Position;
                baseChance = MD_VANILLA_COMBAT.Base_Redirection_Chance(assaultType, ownerPositionType, targetPositionType);

                abilityChance = usedAbility.Calculate_Redirect_Chance__Ability
                (
                    action,
                    ownerPositionType,
                    targetPositionType,
                    baseChance
                );

                baseChance = Combat_Redirection_Chance.Combine(baseChance, abilityChance);
                
                statusEffectChances = target.React_To__Redirect_Chance__GameEntity
                (
                    action,
                    assaultType,
                    ownerPositionType,
                    targetPositionType,
                    baseChance
                );

                foreach (Combat_Redirection_Chance chance in statusEffectChances)
                {
                    baseChance = Combat_Redirection_Chance.Combine(baseChance, chance);
                }
                
                if (baseChance.REDIRECTION != GameEntity_Position_Swap_Type.Swap_Null)
                    chances.Add(baseChance);
            }

            return chances.ToArray();
        }

        private void Relay_UI_Event(GameEntity_ServerSide entityServerSide, Combat_Action action)
        {
            GameEntity_Ability ability = action.Selected_Ability;
            Combat_Assault_Type assaultType = ability.Ability__Combat_Assault_Type;

            GameEntity_Position[] positions = action.Target.Get_Reduced_Fields();
            GameEntity_ServerSide[] targets = Get_Entities(positions);
            
            //TOOD: fix
            switch (assaultType)
            {
                case Combat_Assault_Type.Melee:
                    Resolver.Combat.Act_Melee_Attack(entityServerSide.GameEntity_ID, targets[0].GameEntity_ID);
                    break;
                case Combat_Assault_Type.Ranged:
                    Resolver.Combat.Act_Ranged_Attack(entityServerSide.GameEntity_ID, targets[0].GameEntity_ID, ability.Ability__Particle_Name);
                    break;
            }
        }
    }
}
