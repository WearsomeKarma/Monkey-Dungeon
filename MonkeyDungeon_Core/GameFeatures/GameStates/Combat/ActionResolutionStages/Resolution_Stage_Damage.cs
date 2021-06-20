using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using System;
using System.ComponentModel.Design;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Damage : Combat_Action_Resolution_Stage
    {
        protected override Combat_Action_Conclusion_Type Handle__Resolve_Action__Resolution_Stage(GameEntity_ServerSide_Action action)
        {
            Determine__Invoker_Base_Damage(action);
            Apply__Damage_To_Targets(action);

            return Combat_Action_Conclusion_Type.SUCCESS;
        }

        private void Determine__Invoker_Base_Damage(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_Damage<GameEntity_ServerSide> baseOffset = ability.Calculate__Damage__Ability();
            GameEntity_ServerSide_Quantity hitBonus = action.Action__Hit_Bonus__Of_Invoking_Entity;

            GameEntity_Position[] targetedPositions =
                action.Action__Survey_Target.Get__Targeted_Positions__Survey_Target();

            foreach (GameEntity_Position targetedPosition in targetedPositions)
            {
                GameEntity_ServerSide_Quantity dodgeBonus = action.Action__Survey_Dodge_Bonuses[targetedPosition];
                
                action.Action__Survey_Damage.Set__Damage__Survey_Damage
                    (
                    targetedPosition,
                    Get__Modified_Damage(
                        baseOffset,
                        hitBonus,
                        dodgeBonus
                        )
                    );
            }
        }

        private void Apply__Damage_To_Targets(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_Position[] targetedPositions =
                action.Action__Survey_Target.Get__Targeted_Positions__Survey_Target();

            foreach (GameEntity_Position targetedPosition in targetedPositions)
            {
                GameEntity_ServerSide targetedEntity = Get__Entity__Resolution_Stage(targetedPosition);
                GameEntity_Damage<GameEntity_ServerSide> damage = action.Action__Survey_Damage[targetedPosition];
                
                targetedEntity.React_To__Pre_Resource_Offset__GameEntity(ability.Ability__Targeted_Resource, damage);
                
                targetedEntity.Offset__Resource__GameEntity<GameEntity_ServerSide_Resource>(ability.Ability__Targeted_Resource, damage);
                
                targetedEntity.React_To__Post_Resource_Offset__GameEntity(ability.Ability__Targeted_Resource, damage);
            }
        }

        private GameEntity_Damage<GameEntity_ServerSide> Get__Modified_Damage
        (
            GameEntity_Damage<GameEntity_ServerSide> baseOffset, 
            GameEntity_ServerSide_Quantity hitBonus, 
            GameEntity_ServerSide_Quantity dodgeBonus
        )
        {
            return new GameEntity_Damage<GameEntity_ServerSide>(baseOffset.Damage_Type,
                -1 * baseOffset * hitBonus / dodgeBonus);
        }
    }
}
