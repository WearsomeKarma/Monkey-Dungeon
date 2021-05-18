using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.CombatObjects
{
    public enum DamageType
    {
        Abstract,
        Physical,
        Magical,
        Poison
    }

    public class Combat_Damage
    {
        public DamageType DamageType { get; private set; }
        public double Amount { get; private set; }

        public Combat_Damage(DamageType damageType, double amount)
        {
            DamageType = damageType;
            Amount = (amount > 0) ? amount : 0;
        }
    }
}
