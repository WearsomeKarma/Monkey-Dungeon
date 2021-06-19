using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats.Implemented
{
    public class Agility : GameEntity_ServerSide_Stat
    {
        public Agility(float baseValue) 
            : base(MD_VANILLA_STAT_NAMES.STAT_AGILITY, 0, baseValue)
        {
        }
    }
}
