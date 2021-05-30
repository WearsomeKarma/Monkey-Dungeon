using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented
{
    public class Stamina : GameEntity_Resource
    {
        public Stamina(double baseValue, double max, double replenishRate, double progressionRate) 
            : base(MD_VANILLA_RESOURCES.RESOURCE_STAMINA, baseValue, max, 0, replenishRate, progressionRate)
        {
        }

        protected override void Handle_Add_To_Entity(GameEntity_Resource_Manager resource_Manager)
        {
            resource_Manager.Add_Resource<Stamina>(this);
        }

        public override GameEntity_Resource Clone()
        {
            return new Stamina(Get_BaseValue(), Max_Value, Rate_Replenish, Max_Value.Scaling_Rate);
        }
    }
}
