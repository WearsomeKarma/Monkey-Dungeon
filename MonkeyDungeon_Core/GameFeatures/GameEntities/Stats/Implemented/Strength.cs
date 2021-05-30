using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats.Implemented
{
    public class Strength : GameEntity_Stat
    {
        public Strength(float baseValue, float maxProgresionRate) 
            : base(MD_VANILLA_STATS.STAT_STRENGTH, baseValue, maxProgresionRate)
        {
        }
    }
}
