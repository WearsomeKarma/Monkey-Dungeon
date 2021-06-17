using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats.Implemented
{
    public class Smartypants : GameEntity_Stat
    {
        public Smartypants(float baseValue) 
            : base(MD_VANILLA_STAT_NAMES.STAT_SMARTYPANTS, 0, baseValue)
        {
        }
    }
}
