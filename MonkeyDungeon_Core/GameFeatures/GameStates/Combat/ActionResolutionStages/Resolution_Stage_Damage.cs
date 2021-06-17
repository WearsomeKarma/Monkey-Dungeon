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
            GameEntity_ServerSide valueOwner;
            GameEntity_Ability ability = action.Selected_Ability;

            Combat_Resource_Offset baseOffset = ability.Calculate_Damage__Ability(action);
            Console.WriteLine("[rs_damage.cs:21] baseOffset: " + baseOffset);

            foreach (GameEntity_ServerSide target in Get_Entities(action.Target.Get_Reduced_Fields()))
            {
                Handle_Entity(target, action, baseOffset);
            }
        }
        
        private void Handle_Entity(GameEntity_ServerSide entity, Combat_Action action, Combat_Resource_Offset baseOffset)
        {
            Combat_Resource_Offset finalizedOffset = Get_Finalized_Offset
                (
                baseOffset, 
                action.Finalized_Hit_Bonus, 
                action.Dodge_Bonus_Foreach_Target[entity.GameEntity_ID]
                );
            
            GameEntity_Resource effectedResource =
                entity.Get__Resource__GameEntity<GameEntity_Resource>(action.Target_Affected_Resource);
            
            if (effectedResource == null)
                return;
                
            entity.React_To__Pre_Resource_Offset__GameEntity(effectedResource.Attribute_Name, (double) finalizedOffset);
                
            effectedResource.Offset_Value(finalizedOffset);

            entity.React_To__Post_Resource_Offset__GameEntity(effectedResource.Attribute_Name, (double) finalizedOffset);
        }

        private Combat_Resource_Offset Get_Finalized_Offset
        (
            Combat_Resource_Offset baseOffset, 
            Combat_Finalized_Value hitBonus, 
            Combat_Finalized_Value dodgeBonus
        )
        {
            return new Combat_Resource_Offset(baseOffset.DamageType,
                -1 * baseOffset * hitBonus / dodgeBonus);
        }
    }
}
