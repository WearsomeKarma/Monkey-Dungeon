using System;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Resource<T> : GameEntity_Quantity<T> where T : GameEntity
    {
        protected GameEntity_Resource
            (
            GameEntity_Attribute_Name resourceName, 
            double? initalValue = null,
            double min = Double.MinValue, 
            double max = Double.MaxValue
            ) 
            : base
                (
                resourceName, 
                initalValue,
                min, 
                max
                )
        {
            if (initalValue != null)
                Set__Value__Quantity((double)initalValue);
        }

        public virtual GameEntity_Resource<T> Clone__Resource()
        {
            GameEntity_Resource<T> clone = new GameEntity_Resource<T>(
                Attribute_Name,
                Quantity__Minimal_Value,
                Quantity__Maximal_Value,
                Quantity__Value
                );
            return clone;
        }

        /// <summary>
        /// Returns boolean based on if the offset is possible. That is, it doesn't underflow Min_Quantity, or overflow Max_Quantity.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="peeking">If offset is possible, will modify the resource quantity if peeking is false.</param>
        /// <returns></returns>
        public bool Try_Offset__Resource(double offset, bool peeking = false)
        {
            bool breaks = MathHelper.Breaks_Clampd(Quantity__Value + offset, Quantity__Minimal_Value, Quantity__Maximal_Value);
            if (!breaks && !peeking)
                Offset__Value__Quantity(offset);
            return !breaks;
        }

        /// <summary>
        /// Will apply the offset through a clamp between Min_Quantity and Max_Quantity.
        /// Returns boolean based on whether the value changed at all.
        /// </summary>
        /// <param name="offset"></param>
        public bool Force_Offset__Resource(double offset)
        {
            double val = Quantity__Value;
            Offset__Value__Quantity(offset);
            return (val - Quantity__Value != 0);
        }
    }
}
