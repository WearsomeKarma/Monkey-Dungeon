using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Redirect_Chance : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity owner = Get_Entity(action.Action_Owner);
            GameEntity_Ability ability = owner.Ability_Manager.Get_Ability<GameEntity_Ability>(action.Selected_Ability);

            throw new NotImplementedException();
            double percent = ability.Calculate_Redirect_Chance(action);
        }
    }
}
