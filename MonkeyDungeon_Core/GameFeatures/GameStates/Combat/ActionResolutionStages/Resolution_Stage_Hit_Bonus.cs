﻿using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using System;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Hit_Bonus : Combat_Action_Resolution_Stage
    {
        protected override Combat_Action_Conclusion_Type Handle__Resolve_Action__Resolution_Stage(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_ServerSide owner = Get__Entity__Resolution_Stage(action);
            GameEntity_ServerSide_Stat scalingStat = owner.Get__Stat__GameEntity<GameEntity_ServerSide_Stat>(ability.Ability__Primary_Stat_Name);

            GameEntity_ServerSide_Quantity hitBonus = GameEntity_ServerSide_Quantity.Get__Generic_At_Zero__ServerSide_Quantity();

            hitBonus.Offset__Value__Quantity(scalingStat);
            
            double statusEffectBonuses = owner.Get_Hit_Bonuses__GameEntity();
            hitBonus.Offset__Value__Quantity(statusEffectBonuses);

            Console.WriteLine("---------> hitBonus " + hitBonus);
            action.Action__Hit_Bonus__Of_Invoking_Entity = hitBonus;

            return Combat_Action_Conclusion_Type.SUCCESS;
        }
    }
}
