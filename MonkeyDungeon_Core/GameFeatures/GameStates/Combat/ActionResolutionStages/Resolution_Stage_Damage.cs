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
        protected override void Handle_Stage(GameEntity_ServerSide_Action action)
        {
            Determine__Damage_Per_Target(action);
        }

        private void Determine__Damage_Per_Target(GameEntity_ServerSide_Action action)
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
