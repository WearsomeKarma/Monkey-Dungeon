using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Cast : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide entity = Get_Entity(action);

            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;

            GameEntity_ServerSide_Resource taxedResource =
                entity.Get__Resource__GameEntity<GameEntity_ServerSide_Resource>(ability.Ability__Targeted_Resource);

            if (CheckIf_Can_Use_Action(ability, entity, taxedResource))
            {
                ability.Cast__ServerSide_Ability();

                entity.React_To__Cast__GameEntity();
            }
        }

        private bool CheckIf_Can_Use_Action(GameEntity_ServerSide_Ability ability, GameEntity_ServerSide entity, GameEntity_ServerSide_Resource taxedResource)
        {
            bool hasAbilityPoints = entity.Try_Offset__Ability_Point__GameEntity(-ability.Ability__Point_Cost, true);
            bool hasTaxedResource = taxedResource.Try_Offset__Resource(-ability.Ability__Taxed_Resource_Cost, true);
            
            bool canPerform =
                hasAbilityPoints
                &&
                hasTaxedResource;

            if (canPerform)
            {
                entity.Try_Offset__Ability_Point__GameEntity(-ability.Ability__Point_Cost);
                taxedResource.Try_Offset__Resource(-ability.Ability__Taxed_Resource_Cost);
            }
            
            return canPerform;
        }
    }
}
