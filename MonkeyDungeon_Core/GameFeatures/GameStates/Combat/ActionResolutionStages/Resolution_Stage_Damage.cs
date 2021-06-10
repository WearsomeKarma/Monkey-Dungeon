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
            
            GameEntity owner = Get_Owner_Entity_Of_Action(action.Action_Owner).Entity;
            GameEntity effectedEntity;
            GameEntity_Ability ability = owner.Ability_Manager.Get_Ability<GameEntity_Ability>(action.Selected_Ability);

            Combat_Resource_Offset baseOffset = ability.Calculate_Damage(action);
            
            Combat_Resource_Offset finalizedOffset;
            foreach (Combat_Finalized_Factor finalizedDodgeValue in action.Finalized_Dodge_Bonuses)
            {
                finalizedOffset =
                    new Combat_Resource_Offset(baseOffset.DamageType, baseOffset * action.Finalized_Hit_Bonus/ finalizedDodgeValue);

                effectedEntity = Get_Owner_Entity_Of_Action(finalizedDodgeValue.FACTOR_OWNER).Entity;

                GameEntity_Resource affectedResource =
                    effectedEntity.Resource_Manager.Get_Resource(action.Target_Affected_Resource);

                if (affectedResource == null)
                    return;
                
                effectedEntity.StatusEffect_Manager.React_To_Pre_Resource_Offset(effectedResource, (double) finalizedOffset);
                
                affectedResource.Offset_Value(finalizedOffset);

                effectedEntity.StatusEffect_Manager.React_To_Post_Resource_Offset(effectedResource, (double) finalizedOffset);
            }
        }
    }
}
