using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented
{
    public class Stamina : GameEntity_Resource
    {
        public Stamina(double max, double? initalValue = null) 
            : base(MD_VANILLA_RESOURCES.RESOURCE_STAMINA, 0, max, initalValue)
        {
        }

        public override GameEntity_Resource Clone()
        {
            return new Stamina(Max_Quantity, Value);
        }
    }
}
