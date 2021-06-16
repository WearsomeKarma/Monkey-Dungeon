﻿using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using System;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Hit_Bonus : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity_ServerSide owner = Get_Entity(action);
            GameEntity_Stat scalingStat = owner.Get__Stat__GameEntity<GameEntity_Stat>(action.Stat_Hit_Bonus);

            Combat_Finalized_Factor hitBonus = new Combat_Finalized_Factor(action.Action_Owner);

            hitBonus.Offset_Value(scalingStat);
            
            double statusEffectBonuses = owner.Get_Hit_Bonuses__GameEntity(action);
            hitBonus.Offset_Value(statusEffectBonuses);

            action.Finalized_Hit_Bonus = hitBonus;
        }
    }
}
