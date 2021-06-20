using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources.Implemented
{
    public class Stamina : GameEntity_ServerSide_Resource
    {
        public Stamina(double max, double? initalValue = null) 
            : base(MD_VANILLA_RESOURCE_NAMES.RESOURCE_STAMINA, initalValue, 0, max)
        {
        }

        public override GameEntity_Resource<GameEntity_ServerSide> Clone__Resource()
        {
            return new Stamina(Quantity__Maximal_Value, Quantity__Value);
        }
    }
}
