using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class ScalingValue
    {
        public readonly double Inital_Value;
        public double Value { get; private set; }
        public double Scaling_Rate { get; private set; }
        public void Change_ScalingRate(float rate) { Scaling_Rate = rate; Adjust(); }
        public int Scaling_Level { get; private set; }
        public void Change_ScalingLevel(int level) { Scaling_Level = level; Adjust(); }

        public ScalingValue(double initalValue, int level, double rate)
        {
            Inital_Value = initalValue;
            Value = initalValue;
            Scaling_Rate = rate;
            Scaling_Level = level;
            Adjust();
        }

        private void Adjust()
        {
            Value = (Scaling_Level * Scaling_Rate) + Inital_Value;
        }

        public static implicit operator double(ScalingValue sv) => sv.Value;
    }
}
