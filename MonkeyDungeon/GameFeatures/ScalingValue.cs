using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class ScalingValue
    {
        public readonly float InitalValue;
        public float Value { get; private set; }
        public float ScalingRate { get; private set; }
        public void ChangeScalingRate(float rate) { ScalingRate = rate; Adjust(); }
        public int ScalingLevel { get; private set; }
        public void ChangeScalingLevel(int level) { ScalingLevel = level; Adjust(); }

        public ScalingValue(float initalValue, int level, float rate)
        {
            InitalValue = initalValue;
            Value = initalValue;
            ScalingRate = rate;
            ScalingLevel = level;
            Adjust();
        }

        private void Adjust()
        {
            Value = ScalingLevel * ScalingRate + InitalValue;
        }

        public static implicit operator float(ScalingValue sv) => sv.Value;
    }
}
