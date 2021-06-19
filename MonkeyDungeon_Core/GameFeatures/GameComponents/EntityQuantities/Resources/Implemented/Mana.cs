using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources.Implemented
{
    public class Mana : GameEntity_ServerSide_Resource
    {
        public Mana(double max, double? initalValue = null) 
            : base(MD_VANILLA_RESOURCE_NAMES.RESOURCE_MANA, 0, max, initalValue)
        {
        }

        public override GameEntity_Resource<GameEntity_ServerSide> Clone__Resource()
        {
            return new Mana(Max_Quantity, Value);
        }
    }
}
