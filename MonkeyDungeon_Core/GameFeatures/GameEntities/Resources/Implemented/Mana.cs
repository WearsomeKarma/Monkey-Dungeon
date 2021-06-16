using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented
{
    public class Mana : GameEntity_Resource
    {
        public Mana(double max, double? initalValue = null) 
            : base(MD_VANILLA_RESOURCE_NAMES.RESOURCE_MANA, 0, max, initalValue)
        {
        }

        public override GameEntity_Resource Clone__Resource()
        {
            return new Mana(Max_Quantity, Value);
        }
    }
}
