using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats.Implemented
{
    public class Strength : GameEntity_Stat
    {
        public Strength(float baseValue, float maxProgresionRate) 
            : base(MD_VANILLA_STAT_NAMES.STAT_STRENGTH, baseValue, maxProgresionRate)
        {
        }
    }
}
