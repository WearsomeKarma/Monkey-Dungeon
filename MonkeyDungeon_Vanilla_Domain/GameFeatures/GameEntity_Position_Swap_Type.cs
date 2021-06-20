namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public enum GameEntity_Position_Swap_Type
    {
        /// <summary>
        /// Causes no redirection.
        /// </summary>
        No_Swap = 0,
        /// <summary>
        /// Up and down the battle field.
        /// </summary>
        Swap_Vertical = 1,
        /// <summary>
        /// Left and right of the battlefield.
        /// </summary>
        Swap_Horizontal = 2,
        /// <summary>
        /// Both Vertical and Horizontal redirection at the same time.
        /// </summary>
        Swap_Diagonal = 3,
        /// <summary>
        /// Redirects to no target - potentially resulting in termination of action or swapped object.
        /// </summary>
        Swap_To_Null = -1,
        /// <summary>
        /// Use this to indicate that no redirection modification is desired.
        /// For example, if the base redirection is 70% vertical, use this to leave
        /// the current redirection type alone. Set value to 0 additive or 1 multiplicative
        /// to leave the chance percentage alone.
        /// </summary>
        Swap_Is_Null = -2
    }
}