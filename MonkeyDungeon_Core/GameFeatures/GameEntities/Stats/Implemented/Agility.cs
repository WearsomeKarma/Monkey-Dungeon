using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats.Implemented
{
    public class Agility : GameEntity_Stat
    {
        public Agility(float baseValue) 
            : base(MD_VANILLA_STAT_NAMES.STAT_AGILITY, 0, baseValue)
        {
        }
    }
}
