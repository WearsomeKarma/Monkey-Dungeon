using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures
{
    public static class MathHelper
    {
        public static double Clampd(double value, double min, double max)
        {
            return (value < min) ? min : ((value > max) ? max : value);
        }

        internal static double ClampMaxd(double value, double max)
        {
            return (value < max) ? value : max;
        }
    }
}
