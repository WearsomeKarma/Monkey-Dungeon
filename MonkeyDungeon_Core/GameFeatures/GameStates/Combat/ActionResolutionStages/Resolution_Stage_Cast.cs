using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Cast : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity entity = Get_Owner_Entity_Of_Action(action).Entity;

            GameEntity_Ability ability = entity.Ability_Manager.Get_Ability<GameEntity_Ability>(action.Selected_Ability);
            Combat_Assault_Type assaultType = ability.Assault_Type;

            //TOOD: fix
            switch (assaultType)
            {
                case Combat_Assault_Type.Melee:
                    Resolver.Combat.Act_Melee_Attack(entity.GameEntity_ID, ability.Target.Get_Targets()[0]);
                    break;
                case Combat_Assault_Type.Ranged:
                    Resolver.Combat.Act_Ranged_Attack(entity.GameEntity_ID, ability.Target.Get_Targets()[0], ability.Particle_Type);
                    break;
            }

            ability.Cast(action);

            entity.StatusEffect_Manager.React_To_Cast(action);
        }
    }
}
