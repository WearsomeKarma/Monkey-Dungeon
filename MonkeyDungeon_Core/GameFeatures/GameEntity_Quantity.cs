using MonkeyDungeon_Vanilla_Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Quantity : GameEntity_Attribute
    {
        public double Max_Quantity { get; internal set; }
        public double Min_Quantity { get; internal set; }

        public double Value { get; internal set; }

        /// <summary>
        /// Resources can only be manipulated during the beginning of the turn or ability resolution.
        /// </summary>
        public event Action<GameEntity_Quantity> Quantity_Changed;

        /// <summary>
        /// Resources can only be manipulated during the beginning of the turn or ability resolution.
        /// DOUBLE: The amount of increase.
        /// </summary>
        public event Action<GameEntity_Quantity, double> Quantity_Increasing;

        /// <summary>
        /// Resources can only be manipulated during the beginning of the turn or ability resolution.
        /// DOUBLE: The amount of decrease.
        /// </summary>
        public event Action<GameEntity_Quantity, double> Quantity_Decreasing;

        public event Action<GameEntity_Quantity> Quantity_Depleted;

        public bool IsDepleted
            => (Value - Min_Quantity) < 0.0001;

        public GameEntity_Quantity(GameEntity_Attribute_Name attributeName, double min, double max)
            : base(attributeName)
        {
            min = MathHelper.Clampd(min, min, max);
            max = (max > min) ? max : min;

            Min_Quantity = min;
            Max_Quantity = max;

            Value = Max_Quantity;
        }

        internal double Offset_Value(double offsetValue)
        {
            offsetValue = Handle_Pre_Offset_Value(offsetValue);

            Clamp(Value + offsetValue);

            Handle_Post_Offset_Value(Value);

            return Value;
        }

        internal double Set_Value(double newValue)
        {
            newValue = Handle_Pre_Set_Value(newValue);

            Clamp(newValue);

            Handle_Post_Set_Value(Value);

            return Value;
        }

        internal void Set_Min(double newMin)
        {
            newMin = Handle_Pre_Set_Min(newMin);

            Min_Quantity = (newMin < Max_Quantity) ? newMin : Max_Quantity;

            Clamp(Value);

            Handle_Post_Set_Min(Min_Quantity);
        }

        internal void Set_Max(double newMax, bool raiseValue = false)
        {
            newMax = Handle_Pre_Set_Max(newMax);

            double oldMax = Max_Quantity;

            Max_Quantity = (newMax > Min_Quantity) ? newMax : Min_Quantity;
            Clamp(Value);

            if (raiseValue)
                Offset_Value(newMax - oldMax);

            Handle_Post_Set_Max(Max_Quantity);
        }
        
        private void Check_For_Depletion()
        {
            if (IsDepleted)
            {
                Handle_Quantity_Depleted();
                Quantity_Depleted?.Invoke(this);
            }
        }

        private void Inspect_Change(double diff)
        {
            if (diff == 0)
                return;
            
            Handle_Quantity_Change();
            Quantity_Changed?.Invoke(this);
            
            bool isIncrease = diff > Value;

            if (isIncrease)
                Quantity_Increasing?.Invoke(this, diff);
            else
                Quantity_Decreasing?.Invoke(this, diff);

            Check_For_Depletion();

            return;
        }

        public static implicit operator double(GameEntity_Quantity q)
            => q.Value;

        protected virtual double Handle_Pre_Offset_Value(double offsetValue) => offsetValue;
        protected virtual double Handle_Pre_Set_Value(double newValue) => newValue;

        protected virtual double Handle_Pre_Set_Min(double newMin) => newMin;
        protected virtual double Handle_Pre_Set_Max(double newMax) => newMax;

        protected virtual void Handle_Post_Offset_Value(double newValue) { }
        protected virtual void Handle_Post_Set_Value(double newValue) { }

        protected virtual void Handle_Post_Set_Min(double newMin) { }
        protected virtual void Handle_Post_Set_Max(double newMax) { }

        protected virtual void Handle_Quantity_Change() { }
        protected virtual void Handle_Quantity_Depleted() { }

        protected void Clamp(double targetValue)
        {
            double newValue = MathHelper.Clampd(targetValue, Min_Quantity, Max_Quantity);
            Inspect_Change(newValue - Value);
            Value = newValue;
        }
    }
}
