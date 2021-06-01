using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Cast : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity entity = Get_Entity(action);

            GameEntity_Ability ability = entity.Ability_Manager.Get_Ability<GameEntity_Ability>(action.Selected_Ability);

            ability.Cast(action);

            entity.StatusEffect_Manager.React_To_Cast(action);
        }
    }
}
