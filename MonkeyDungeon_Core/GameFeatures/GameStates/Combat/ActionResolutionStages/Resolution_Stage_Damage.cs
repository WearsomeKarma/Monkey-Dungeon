using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Damage : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity owner = Get_Entity(action.Action_Owner);
            GameEntity_Ability ability = owner.Ability_Manager.Get_Ability<GameEntity_Ability>(action.Selected_Ability);

            Combat_Damage damage = ability.Calculate_Damage(action);

            throw new NotImplementedException();
            /*
             
            GameEntity[] targetedEntities = Get_Targeted_Entities(action.Target);
            foreach
                targetedEntities.Damage(damage);

             */
        }
    }
}
