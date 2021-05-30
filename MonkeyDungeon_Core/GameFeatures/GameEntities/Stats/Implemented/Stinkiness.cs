using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats.Implemented
{
    public class Stinkiness : GameEntity_Stat
    {
        public Stinkiness(float baseValue, float maxProgresionRate) 
            : base(MD_VANILLA_STATS.STAT_STINKINESS, baseValue, maxProgresionRate)
        {
        }
    }
}
