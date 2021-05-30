
namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class Combat_Damage
    {
        public Combat_Damage_Type DamageType { get; private set; }
        public double Amount { get; private set; }

        public Combat_Damage(Combat_Damage_Type damageType, double amount)
        {
            DamageType = damageType;
            Amount = (amount > 0) ? amount : 0;
        }
    }
}
