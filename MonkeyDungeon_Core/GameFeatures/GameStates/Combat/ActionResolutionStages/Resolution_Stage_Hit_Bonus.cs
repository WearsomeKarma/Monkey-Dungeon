using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using System;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Hit_Bonus : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity owner = Get_Owner_Entity_Of_Action(action).Game_Entity;
            GameEntity_Stat scalingStat = owner.Stat_Manager.Get_Stat(action.Stat_Hit_Bonus);

            Combat_Finalized_Factor hitBonus = new Combat_Finalized_Factor(action.Action_Owner);

            hitBonus.Offset_Value(scalingStat);
            
            double statusEffectBonuses = owner.StatusEffect_Manager.Get_Hit_Bonuses(action);
            hitBonus.Offset_Value(statusEffectBonuses);

            action.Finalized_Hit_Bonus = hitBonus;
        }
    }
}
