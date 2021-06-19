using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats.Implemented
{
    public class Stinkiness : GameEntity_ServerSide_Stat
    {
        public Stinkiness(float baseValue) 
            : base(MD_VANILLA_STAT_NAMES.STAT_STINKINESS, 0, baseValue)
        {
        }
    }
}
