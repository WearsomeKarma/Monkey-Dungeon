using System.Collections.Generic;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Redirect_Chance : Combat_Action_Resolution_Stage
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
        }

        private void Check_For_Target_Alteration(Combat_Target target, Combat_Redirection_Chance[] chances)
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
            Combat_Redirection_Chance[] chances = new Combat_Redirection_Chance[targets.Length];

            Combat_Redirection_Chance abilityChance;
            Combat_Redirection_Chance[] statusEffectChances;
            
            for (int i = 0; i < targets.Length; i++)
            {
                target = Entity_Field.Get_Entity(targets[i]);
                targetPositionType = (GameEntity_Position_Type) target.GameEntity_Position;
                chances[i] = MD_VANILLA_COMBAT.Base_Redirection_Chance(assaultType, ownerPositionType, targetPositionType);

                abilityChance = usedAbility.Calculate_Redirect_Chance__Ability
                (
                    action,
                    ownerPositionType,
                    targetPositionType,
                    chances[i]
                );

                chances[i] = Combat_Redirection_Chance.Combine(chances[i], abilityChance);
                
                statusEffectChances = target.React_To__Redirect_Chance__GameEntity
                (
                    action,
                    assaultType,
                    ownerPositionType,
                    targetPositionType,
                    chances[i]
                );

                foreach (Combat_Redirection_Chance chance in statusEffectChances)
                {
                    chances[i] = Combat_Redirection_Chance.Combine(chances[i], chance);
                }
            }

            return chances;
        }
    }
}
