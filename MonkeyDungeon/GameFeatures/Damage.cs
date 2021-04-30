using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public enum DamageType
    {
        Abstract,
        Physical,
        Magical,
        Poison
    }

    public class Damage
    {
        public DamageType DamageType { get; private set; }
        public float Amount { get; private set; }

        public Damage(DamageType damageType, float amount)
        {
            DamageType = damageType;
            Amount = (amount > 0) ? amount : 0;
        }
    }
}
