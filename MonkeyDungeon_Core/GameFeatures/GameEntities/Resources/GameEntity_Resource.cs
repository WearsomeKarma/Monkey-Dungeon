
using MonkeyDungeon_Vanilla_Domain;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources
{
    public class GameEntity_Resource : GameEntity_Quantity
    {
        public GameEntity_Resource(GameEntity_Attribute_Name resourceName, double min, double max, double? initalValue = null) 
            : base(resourceName, min, max)
        {
            if (initalValue != null)
                Set_Value((double)initalValue);
        }

        public virtual GameEntity_Resource Clone()
        {
            GameEntity_Resource clone = new GameEntity_Resource(
                ATTRIBUTE_NAME,
                Min_Quantity,
                Max_Quantity,
                Value
                );
            return clone;
        }

        /// <summary>
        /// Returns boolean based on if the offset is possible. That is, it doesn't underflow Min_Quantity, or overflow Max_Quantity.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="peeking">If offset is possible, will modify the resource quantity if peeking is false.</param>
        /// <returns></returns>
        public bool Try_Offset(double offset, bool peeking = false)
        {
            bool breaks = MathHelper.Breaks_Clampd(Value + offset, Min_Quantity, Max_Quantity);
            if (!breaks && !peeking)
                Offset_Value(offset);
            return breaks;
        }

        /// <summary>
        /// Will apply the offset through a clamp between Min_Quantity and Max_Quantity.
        /// Returns boolean based on whether the value changed at all.
        /// </summary>
        /// <param name="offset"></param>
        public bool Force_Offset(double offset)
        {
            double val = Value;
            Offset_Value(offset);
            return (val - Value != 0);
        }
    }
}
