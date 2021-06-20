using System;
using System.ComponentModel;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Quantity<T> : GameEntity_Attribute<T> where T : GameEntity
    {
        public double Quantity__Maximal_Value { get; internal set; }
        public double Quantity__Minimal_Value { get; internal set; }

        public double Quantity__Value { get; internal set; }
        
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
            => (Quantity__Value - Quantity__Minimal_Value) < 0.0001;

         protected GameEntity_Quantity
             (
             GameEntity_Attribute_Name attributeName,
             double? initalValue = null, 
             double min = Double.MinValue, 
             double max = Double.MaxValue
             )
            : base(attributeName)
        {
            min = MathHelper.Clampd(min, min, max);
            max = (max > min) ? max : min;

            Quantity__Minimal_Value = min;
            Quantity__Maximal_Value = max;
            
            Set__Value__Quantity(initalValue ?? Quantity__Maximal_Value);
        }

        public double Offset__Value__Quantity(double offsetValue)
        {
            offsetValue = Handle_Pre_Offset_Value(offsetValue);

            Clamp(Quantity__Value + offsetValue);

            Handle_Post_Offset_Value(Quantity__Value);

            return Quantity__Value;
        }

        public double Set__Value__Quantity(double newValue)
        {
            newValue = Handle_Pre_Set_Value(newValue);

            Clamp(newValue);

            Handle_Post_Set_Value(Quantity__Value);

            return Quantity__Value;
        }

        public void Set__Minimal_Value__Quantity(double newMin)
        {
            newMin = Handle_Pre_Set_Min(newMin);

            Quantity__Minimal_Value = (newMin < Quantity__Maximal_Value) ? newMin : Quantity__Maximal_Value;

            Clamp(Quantity__Value);

            Handle_Post_Set_Min(Quantity__Minimal_Value);
        }

        public void Set__Maximal_Value__Quantity(double newMax, bool raiseValue = false)
        {
            newMax = Handle_Pre_Set_Max(newMax);

            double oldMax = Quantity__Maximal_Value;

            Quantity__Maximal_Value = (newMax > Quantity__Minimal_Value) ? newMax : Quantity__Minimal_Value;
            Clamp(Quantity__Value);

            if (raiseValue)
                Offset__Value__Quantity(newMax - oldMax);

            Handle_Post_Set_Max(Quantity__Maximal_Value);
        }

        public virtual void Modify__By_Quantity__Quantity
            (
            GameEntity_Quantity<T> quantity, 
            GameEntity__Quantity_Modification_Type modificationType
            )
        {
            switch (modificationType)
            {
                case GameEntity__Quantity_Modification_Type.Additive:
                    Offset__Value__Quantity(quantity);
                    break;
                case GameEntity__Quantity_Modification_Type.Multiplicative:
                    Set__Value__Quantity(Quantity__Value * quantity);
                    break;
                case GameEntity__Quantity_Modification_Type.Mutate:
                    Set__Value__Quantity(quantity);
                    break;
                case GameEntity__Quantity_Modification_Type.None:
                    break;
            }
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
            
            bool isIncrease = diff > Quantity__Value;

            if (isIncrease)
                Event__Quantity_Increasing__Quantity?.Invoke(this, diff);
            else
                Event__Quantity_Decreasing__Quantity?.Invoke(this, diff);

            Check_For__Depletion();

            return;
        }

        public static implicit operator double(GameEntity_Quantity<T> q)
            => q.Quantity__Value;

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
            double newValue = MathHelper.Clampd(targetValue, Quantity__Minimal_Value, Quantity__Maximal_Value);
            double oldValue = Quantity__Value;
            Quantity__Value = newValue;
            Inspect__Change(newValue - oldValue);
        }

        public override string ToString()
            => string.Format("[Quantity] {0}:value, {1}:{2}", Quantity__Minimal_Value, Quantity__Value, Quantity__Maximal_Value);
    }
}
