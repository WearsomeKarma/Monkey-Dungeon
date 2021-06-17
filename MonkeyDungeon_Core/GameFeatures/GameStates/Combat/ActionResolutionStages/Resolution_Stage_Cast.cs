using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Cast : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity_ServerSide entity = Get_Entity(action);

            GameEntity_Ability ability = action.Selected_Ability;

            GameEntity_Resource taxedResource =
                entity.Get__Resource__GameEntity<GameEntity_Resource>(ability.Ability__Affecting_Resource);

            if (CheckIf_Can_Use_Action(ability, entity, taxedResource))
            {
                ability.Cast__Ability(action);

                entity.React_To__Cast__GameEntity(action);
            }
        }

        private bool CheckIf_Can_Use_Action(GameEntity_Ability ability, GameEntity_ServerSide entity, GameEntity_Resource taxedResource)
        {
            bool hasAbilityPoints = entity.Try_Offset__Ability_Point__GameEntity(-ability.Ability__Point_Cost, true);
            bool hasTaxedResource = taxedResource.Try_Offset__Resource(-ability.Ability__Resource_Cost, true);
            
            bool canPerform =
                hasAbilityPoints
                &&
                hasTaxedResource;

            if (canPerform)
            {
                entity.Try_Offset__Ability_Point__GameEntity(-ability.Ability__Point_Cost);
                taxedResource.Try_Offset__Resource(-ability.Ability__Resource_Cost);
            }
            
            return canPerform;
        }
    }
}
