using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using System;
using System.ComponentModel.Design;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Damage : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity_Attribute_Name effectedResource = action.Target_Affected_Resource;
            
            GameEntity_ServerSide owner = Get_Entity(action.Action_Owner);
            GameEntity_ServerSide effectedEntityServerSide;
            GameEntity_Ability ability = action.Selected_Ability;

            Combat_Resource_Offset baseOffset = ability.Calculate_Damage__Ability(action);
            
            Combat_Resource_Offset finalizedOffset;
            foreach (Combat_Finalized_Factor finalizedDodgeValue in action.Finalized_Dodge_Bonuses)
            {
                finalizedOffset =
                    new Combat_Resource_Offset(baseOffset.DamageType, baseOffset * action.Finalized_Hit_Bonus/ finalizedDodgeValue);

                effectedEntityServerSide = Get_Entity(finalizedDodgeValue.FACTOR_OWNER);

                GameEntity_Resource affectedResource =
                    effectedEntityServerSide.Get__Resource__GameEntity<GameEntity_Resource>(action.Target_Affected_Resource);

                if (affectedResource == null)
                    return;
                
                effectedEntityServerSide.React_To__Pre_Resource_Offset__GameEntity(effectedResource, (double) finalizedOffset);
                
                affectedResource.Offset_Value(finalizedOffset);

                effectedEntityServerSide.React_To__Post_Resource_Offset__GameEntity(effectedResource, (double) finalizedOffset);
            }
        }
    }
}
