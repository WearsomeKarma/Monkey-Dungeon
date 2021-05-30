using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented
{
    public class Level : GameEntity_Resource
    {
        public Level(float baseValue, float max, float min, float replenishRate, float progressionRate) 
            : base(MD_VANILLA_RESOURCES.RESOURCE_LEVEL, baseValue, max, min, replenishRate, progressionRate)
        {
        }

        protected override void Handle_LevelChange()
        {
            if (Entity != null)
            {
                foreach (GameEntity_Resource resource in Entity.Resource_Manager.Get_Resources())
                    resource.Perform_LevelChange();

                foreach (GameEntity_Stat stat in Entity.Stat_Manager.Get_Stats())
                    stat.Perform_LevelChange();

                //TODO: Scale resistances.
                //foreach (GameEntity_Resistance resistance in Entity.Resistance_Manager.Get_Resistances())
                //    resistance.PerformLevelChange();
            }
        }
    }
}
