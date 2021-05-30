using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities
{
    public class Quantity
    {
        public double Max_Quantity { get; private set; }
        public double Min_Quantity { get; private set; }

        public double Value { get; private set; }

        public Quantity(double min, double max)
        {
            min = MathHelper.Clampd(min, min, max);
            max = (max > min) ? max : min;

            Min_Quantity = min;
            Max_Quantity = max;

            Value = Max_Quantity;
        }

        public double Offset_Value(double offsetValue)
        {
            offsetValue = Handle_Pre_Offset_Value(offsetValue);

            Value = MathHelper.Clampd(Value + offsetValue, Min_Quantity, Max_Quantity);

            Handle_Post_Offset_Value(Value);

            return Value;
        }

        public double Set_Value(double newValue)
        {
            newValue = Handle_Pre_Set_Value(newValue);

            Value = MathHelper.Clampd(newValue, Min_Quantity, Max_Quantity);

            Handle_Post_Set_Value(Value);

            return Value;
        }

        public void Set_Min(double newMin)
        {
            newMin = Handle_Pre_Set_Min(newMin);

            Min_Quantity = (newMin < Max_Quantity) ? newMin : Max_Quantity;
            Clamp();

            Handle_Post_Set_Min(Min_Quantity);
        }

        public void Set_Max(double newMax, bool raiseValue = false)
        {
            newMax = Handle_Pre_Set_Max(newMax);

            double oldMax = Max_Quantity;

            Max_Quantity = (newMax > Min_Quantity) ? newMax : Min_Quantity;
            Clamp();

            if (raiseValue)
                Offset_Value(newMax - oldMax);

            Handle_Post_Set_Max(Max_Quantity);
        }

        public static implicit operator double(Quantity q)
            => q.Value;

        protected virtual double Handle_Pre_Offset_Value(double offsetValue) => offsetValue;
        protected virtual double Handle_Pre_Set_Value(double newValue) => newValue;

        protected virtual double Handle_Pre_Set_Min(double newMin) => newMin;
        protected virtual double Handle_Pre_Set_Max(double newMax) => newMax;

        protected virtual void Handle_Post_Offset_Value(double newValue) { }
        protected virtual void Handle_Post_Set_Value(double newValue) { }

        protected virtual void Handle_Post_Set_Min(double newMin) { }
        protected virtual void Handle_Post_Set_Max(double newMax) { }

        protected void Clamp()
            => Value = MathHelper.Clampd(Value, Min_Quantity, Max_Quantity);
    }
}
