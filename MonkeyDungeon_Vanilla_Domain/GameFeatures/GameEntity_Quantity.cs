using System;
using MonkeyDungeon_Core.GameFeatures;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Quantity<T> : GameEntity_Attribute<T> where T : GameEntity
    {
        public double Max_Quantity { get; internal set; }
        public double Min_Quantity { get; internal set; }

        public double Value { get; internal set; }

        /// <summary>
        /// Resources can only be manipulated during the beginning of the turn or ability resolution.
        /// </summary>
        public event Action<GameEntity_Quantity<T>> Event__Quantity_Changed__Quantity;

        /// <summary>
        /// Resources can only be manipulated during the beginning of the turn or ability resolution.
        /// DOUBLE: The amount of increase.
        /// </summary>
        public event Action<GameEntity_Quantity<T>, double> Event__Quantity_Increasing__Quantity;

        /// <summary>
        /// Resources can only be manipulated during the beginning of the turn or ability resolution.
        /// DOUBLE: The amount of decrease.
        /// </summary>
        public event Action<GameEntity_Quantity<T>, double> Event__Quantity_Decreasing__Quantity;

        public event Action<GameEntity_Quantity<T>> Event__Quantity_Depleted__Quantity;

        public bool IsDepleted
            => (Value - Min_Quantity) < 0.0001;

         protected GameEntity_Quantity(GameEntity_Attribute_Name attributeName, double min, double max, double? value = null)
            : base(attributeName)
        {
            min = MathHelper.Clampd(min, min, max);
            max = (max > min) ? max : min;

            Min_Quantity = min;
            Max_Quantity = max;

            Value = value ?? Max_Quantity;
        }

        public double Offset__Value__Quantity(double offsetValue)
        {
            offsetValue = Handle_Pre_Offset_Value(offsetValue);

            Clamp(Value + offsetValue);

            Handle_Post_Offset_Value(Value);

            return Value;
        }

        public double Set__Value__Quantity(double newValue)
        {
            newValue = Handle_Pre_Set_Value(newValue);

            Clamp(newValue);

            Handle_Post_Set_Value(Value);

            return Value;
        }

        public void Set__Minimal_Value__Quantity(double newMin)
        {
            newMin = Handle_Pre_Set_Min(newMin);

            Min_Quantity = (newMin < Max_Quantity) ? newMin : Max_Quantity;

            Clamp(Value);

            Handle_Post_Set_Min(Min_Quantity);
        }

        public void Set__Maximal_Value__Quantity(double newMax, bool raiseValue = false)
        {
            newMax = Handle_Pre_Set_Max(newMax);

            double oldMax = Max_Quantity;

            Max_Quantity = (newMax > Min_Quantity) ? newMax : Min_Quantity;
            Clamp(Value);

            if (raiseValue)
                Offset__Value__Quantity(newMax - oldMax);

            Handle_Post_Set_Max(Max_Quantity);
        }
        
        private void Check_For__Depletion()
        {
            if (IsDepleted)
            {
                Handle_Quantity_Depleted();
                Event__Quantity_Depleted__Quantity?.Invoke(this);
            }
        }

        private void Inspect__Change(double diff)
        {
            if (diff == 0)
                return;
            
            Handle_Quantity_Change();
            Event__Quantity_Changed__Quantity?.Invoke(this);
            
            bool isIncrease = diff > Value;

            if (isIncrease)
                Event__Quantity_Increasing__Quantity?.Invoke(this, diff);
            else
                Event__Quantity_Decreasing__Quantity?.Invoke(this, diff);

            Check_For__Depletion();

            return;
        }

        public static implicit operator double(GameEntity_Quantity<T> q)
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
            double oldValue = Value;
            Value = newValue;
            Inspect__Change(newValue - oldValue);
        }

        public override string ToString()
            => string.Format("[Quantity] {0}:value, {1}:{2}", Min_Quantity, Value, Max_Quantity);
    }
}
