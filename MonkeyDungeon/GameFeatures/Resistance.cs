using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class Resistance
    {
        public DamageType TypeOfDamage_Resisting { get; private set; }
        public float ResistancePercentage { get; private set; }

        public Resistance(DamageType typeOfDamage_Resisting, float percentage)
        {
            ResistancePercentage = (percentage < 0) ? 0 : percentage;
            TypeOfDamage_Resisting = typeOfDamage_Resisting;
        }

        public Resistance(DamageType typeOfDamage_Resisting, int outOf_OneHundred)
        {
            ResistancePercentage = (outOf_OneHundred < 0) ? 0 : (outOf_OneHundred / 100f);
            TypeOfDamage_Resisting = typeOfDamage_Resisting;
        }

        public virtual Resistance Clone()
        {
            Resistance clone = new Resistance(
                TypeOfDamage_Resisting,
                ResistancePercentage
                );
            return clone;
        }
    }
}
