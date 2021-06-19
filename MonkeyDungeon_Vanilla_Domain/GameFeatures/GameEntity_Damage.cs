using System;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Damage<T> : GameEntity_Quantity<T> where T : GameEntity
    {
        public GameEntity_Damage_Type Damage_Type { get; set; }

        public GameEntity_Damage(GameEntity_Damage_Type damageType, double damageQuantity)
            : base(GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME, double.MinValue, double.MaxValue, damageQuantity)
        {
            Damage_Type = damageType;
        }
    }
}