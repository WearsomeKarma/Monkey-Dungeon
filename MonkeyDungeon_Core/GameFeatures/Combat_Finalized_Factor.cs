using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures
{
    /// <summary>
    /// This is a wrapper for a readonly double and associated gameEntity_Id.
    /// It is used for Hit and Dodge bonuses.
    /// </summary>
    public class Combat_Finalized_Factor : GameEntity_Quantity
    {
        public readonly GameEntity_ID FACTOR_OWNER;

        public Combat_Finalized_Factor(GameEntity_ID factorOwner)
        : base (GameEntity_Attribute_Name.DEFAULT, double.MinValue, double.MaxValue)
        {
            FACTOR_OWNER = factorOwner;
        }
    }
}