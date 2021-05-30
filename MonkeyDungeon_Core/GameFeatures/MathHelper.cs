
namespace MonkeyDungeon_Core.GameFeatures
{
    public static class MathHelper
    {
        public static bool Breaks_Clampd(double value, double min, double max)
            => (value < min) || (value > max);

        public static double Clampd(double value, double min, double max)
            => (value < min) ? min : ((value > max) ? max : value);

        internal static double ClampMaxd(double value, double max)
            => (value < max) ? value : max;
    }
}
