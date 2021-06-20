namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public enum GameEntity__Quantity_Modification_Type
    {
        /// <summary>
        /// This does nothing.
        /// </summary>
        None,
        /// <summary>
        /// When used to modify another quantity, it will set --that-- quantity's value to **this** quantity's.
        /// </summary>
        Mutate,
        /// <summary>
        /// When used to modify another quantity, it will add **this** quantity's value to --that-- quantity's.
        /// </summary>
        Additive,
        /// <summary>
        /// When used to modify another quantity, it will multiply --that-- quantity's value by **this** quantity's.
        /// </summary>
        Multiplicative
    }
}