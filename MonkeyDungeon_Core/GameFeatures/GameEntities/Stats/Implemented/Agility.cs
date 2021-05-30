using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats.Implemented
{
    public class Agility : GameEntity_Stat
    {
        public Agility(float baseValue, float maxProgresionRate) 
            : base(MD_VANILLA_STATS.STAT_AGILITY, baseValue, maxProgresionRate)
        {
        }
    }
}
