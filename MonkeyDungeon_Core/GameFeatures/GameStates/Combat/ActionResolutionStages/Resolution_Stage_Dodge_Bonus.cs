using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using System;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Dodge_Bonus : Combat_Action_Resolution_Stage
    {
        protected override Combat_Action_Conclusion_Type Handle__Resolve_Action__Resolution_Stage(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_Position[] targetPositions = action.Action__Survey_Target.Get__Targeted_Positions__Survey_Target();
            GameEntity_ServerSide entity;
            
            foreach(GameEntity_Position targetedPosition in targetPositions)
            {
                entity = Get__Entity__Resolution_Stage(targetedPosition);

                if (ability.Get__Dodging_Stat__Ability() == GameEntity_Attribute_Name.NULL__ATTRIBUTE_NAME)
                {
                    action.Action__Survey_Dodge_Bonuses[targetedPosition]
                        .Offset__Value__Quantity(action.Action__Hit_Bonus__Of_Invoking_Entity);
                    
                    return Combat_Action_Conclusion_Type.SUCCESS;
                }
                
                GameEntity_ServerSide_Stat dodgeStat = 
                    entity.Get__Stat__GameEntity<GameEntity_ServerSide_Stat>(ability.Get__Dodging_Stat__Ability());

                action.Action__Survey_Dodge_Bonuses[targetedPosition].Offset__Value__Quantity(dodgeStat);

                action.Action__Survey_Dodge_Bonuses[targetedPosition].Offset__Value__Quantity(entity.Get_Dodge_Bonuses__GameEntity());
            }

            return Combat_Action_Conclusion_Type.SUCCESS;
        }
    }
}
