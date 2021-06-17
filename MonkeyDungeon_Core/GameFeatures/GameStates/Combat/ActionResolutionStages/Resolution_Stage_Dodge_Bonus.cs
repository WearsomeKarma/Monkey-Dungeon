using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using System;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Dodge_Bonus : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity_Position[] targetPositions = action.Target.Get_Reduced_Fields();
            GameEntity_ServerSide entity;

            Combat_Finalized_Value dodgeBonus;
            for (int i = 0; i < targetPositions.Length; i++)
            {
                entity = Get_Entity(targetPositions[i]);

                if (action.Stat_Dodge_Bonus == GameEntity_Attribute_Name.NULL_ATTRIBUTE_NAME)
                {
                    dodgeBonus = new Combat_Finalized_Value(entity.GameEntity_ID);
                    dodgeBonus.Offset_Value(action.Finalized_Hit_Bonus);
                    Record_Dodge_Bonus(action, dodgeBonus);
                    return;
                }
                
                dodgeBonus = new Combat_Finalized_Value(Get_Entity(targetPositions[i]).GameEntity_ID);
                
                GameEntity_Stat dodgeStat = entity.Get__Stat__GameEntity<GameEntity_Stat>(action.Stat_Dodge_Bonus);

                dodgeBonus.Offset_Value(dodgeStat);

                dodgeBonus.Offset_Value(entity.Get_Dodge_Bonuses__GameEntity(action));
                
                Record_Dodge_Bonus(action, dodgeBonus);
            }
        }

        private void Record_Dodge_Bonus(Combat_Action action, Combat_Finalized_Value dodgeBonus)
        {
            if (action.Dodge_Bonus_Foreach_Target.ContainsKey(dodgeBonus.VALUE_OWNER))
            {
                action.Dodge_Bonus_Foreach_Target[dodgeBonus.VALUE_OWNER] = dodgeBonus;
                return;
            }
            action.Dodge_Bonus_Foreach_Target.Add(dodgeBonus.VALUE_OWNER, dodgeBonus);
        }
    }
}
