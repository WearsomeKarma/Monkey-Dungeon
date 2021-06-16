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
            GameEntity_ServerSide targetedEntityServerSide;
            
            Combat_Finalized_Factor[] dodgeBonuses = new Combat_Finalized_Factor[targetPositions.Length];
            for (int i = 0; i < targetPositions.Length; i++)
            {
                targetedEntityServerSide = Get_Entity(targetPositions[i]);

                if (action.Stat_Dodge_Bonus == GameEntity_Attribute_Name.NULL_ATTRIBUTE_NAME)
                {
                    dodgeBonuses[i] = action.Finalized_Hit_Bonus; // results in 1/1 in resource offset calculation.
                    continue;
                }
                
                dodgeBonuses[i] = new Combat_Finalized_Factor(Get_Entity(targetPositions[i]).GameEntity_ID);
                
                GameEntity_Stat dodgeStat = targetedEntityServerSide.Get__Stat__GameEntity<GameEntity_Stat>(action.Stat_Dodge_Bonus);

                dodgeBonuses[i].Offset_Value(dodgeStat);

                dodgeBonuses[i].Offset_Value(targetedEntityServerSide.Get_Dodge_Bonuses__GameEntity(action));
            }

            action.Finalized_Dodge_Bonuses = dodgeBonuses;
        }
    }
}
