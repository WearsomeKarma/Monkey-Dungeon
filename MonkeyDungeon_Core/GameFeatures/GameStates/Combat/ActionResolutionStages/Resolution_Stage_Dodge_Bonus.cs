using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using System;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Dodge_Bonus : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity_ID[] targetIDs = action.Target.Get_Targets();
            GameEntity targetedEntity;
            
            Combat_Finalized_Factor[] dodgeBonuses = new Combat_Finalized_Factor[targetIDs.Length];
            for (int i = 0; i < targetIDs.Length; i++)
            {
                targetedEntity = Get_Entity(targetIDs[i]);

                if (action.Stat_Dodge_Bonus == GameEntity_Attribute_Name.DEFAULT)
                {
                    dodgeBonuses[i] = action.Finalized_Hit_Bonus; // results in 1/1 in resource offset calculation.
                    continue;
                }
                
                dodgeBonuses[i] = new Combat_Finalized_Factor(targetIDs[i]);
                
                GameEntity_Stat dodgeStat = targetedEntity.Stat_Manager.Get_Stat(action.Stat_Dodge_Bonus);

                dodgeBonuses[i].Offset_Value(dodgeStat);

                dodgeBonuses[i].Offset_Value(targetedEntity.StatusEffect_Manager.Get_Dodge_Bonuses(action));
            }

            action.Finalized_Dodge_Bonuses = dodgeBonuses;
        }
    }
}
