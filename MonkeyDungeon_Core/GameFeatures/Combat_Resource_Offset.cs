
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class Combat_Resource_Offset
    {
        public Combat_Damage_Type DamageType { get; private set; }
        public GameEntity_Quantity Amount { get; private set; }

        public Combat_Resource_Offset(Combat_Damage_Type damageType, double amount)
        {
            DamageType = damageType;
            //TODO: change to damage instead of DEFAULT.
            Amount = new GameEntity_Quantity(GameEntity_Attribute_Name.NULL_ATTRIBUTE_NAME, 0, amount);
        }

        public static implicit operator double(Combat_Resource_Offset offset)
            => offset.Amount;
    }
}
