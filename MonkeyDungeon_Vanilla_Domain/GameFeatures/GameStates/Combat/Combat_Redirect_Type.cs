namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public enum Combat_Redirect_Type
    {
        /// <summary>
        /// Causes no redirection.
        /// </summary>
        No_Redirect = 0,
        /// <summary>
        /// Up and down the battle field.
        /// </summary>
        Redirect_Vertical = 1,
        /// <summary>
        /// Left and right of the battlefield.
        /// </summary>
        Redirect_Horizontal = 2,
        /// <summary>
        /// Both Vertical and Horizontal redirection at the same time.
        /// </summary>
        Redirect_Diagonal = 3,
        /// <summary>
        /// Redirects to no target - potentially resulting in ability termination.
        /// </summary>
        Redirect_Null = -1
    }
}