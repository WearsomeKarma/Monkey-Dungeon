using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures
{
    /// <summary>
    /// This is a wrapper for a readonly double and associated gameEntity_Id.
    /// It is used for Hit and Dodge bonuses.
    /// </summary>
    public class Combat_Finalized_Value : GameEntity_Quantity
    {
        public readonly GameEntity_ID VALUE_OWNER;

        public Combat_Finalized_Value(GameEntity_ID valueOwner, double? value = null)
        : base (GameEntity_Attribute_Name.NULL_ATTRIBUTE_NAME, double.MinValue, double.MaxValue, value ?? 0)
        {
            VALUE_OWNER = valueOwner;
        }

        public override string ToString()
            => string.Format("[Finalized Value] ID:{0} Value:{1}", VALUE_OWNER, base.ToString());
    }
}