using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using System;
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
            GameEntity_Ability ability = owner.Ability_Manager.Get_Ability<GameEntity_Ability>(action.Selected_Ability);

            Combat_Resource_Offset baseOffset = ability.Calculate_Damage(action);
            
            Combat_Resource_Offset finalizedOffset;
            foreach (Combat_Finalized_Factor finalizedDodgeValue in action.Finalized_Dodge_Bonuses)
            {
                finalizedOffset =
                    new Combat_Resource_Offset(baseOffset.DamageType, baseOffset * action.Finalized_Hit_Bonus/ finalizedDodgeValue);

                effectedEntityServerSide = Get_Entity(finalizedDodgeValue.FACTOR_OWNER);

                GameEntity_Resource affectedResource =
                    effectedEntityServerSide.Resource_Manager.Get_Resource(action.Target_Affected_Resource);

                if (affectedResource == null)
                    return;
                
                effectedEntityServerSide.StatusEffect_Manager.React_To_Pre_Resource_Offset(effectedResource, (double) finalizedOffset);
                
                affectedResource.Offset_Value(finalizedOffset);

                effectedEntityServerSide.StatusEffect_Manager.React_To_Post_Resource_Offset(effectedResource, (double) finalizedOffset);
            }
        }
    }
}
