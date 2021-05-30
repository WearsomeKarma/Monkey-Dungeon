using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented
{
    public class Mana : GameEntity_Resource
    {
        public Mana(double max, double? initalValue = null) 
            : base(MD_VANILLA_RESOURCES.RESOURCE_MANA, 0, max, initalValue)
        {
        }

        public override GameEntity_Resource Clone()
        {
            return new Mana(Max_Quantity, Value);
        }
    }
}
