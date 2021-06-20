namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat
{
    public enum Combat_Action_Conclusion_Type
    {
        /// <summary>
        /// Failure due to missing targets.
        /// </summary>
        FAIL__INVALID_TARGETS = -3,
        /// <summary>
        /// Failure due to lacking taxed resources.
        /// </summary>
        FAIL__INSUFFICENT_RESOURCES = -2,
        /// <summary>
        /// Failure as a result of some effect.
        /// </summary>
        FAIL__REQUEST = -1,
        
        /// <summary>
        /// Success in progressing the action resolution.
        /// </summary>
        SUCCESS = 0
    }
}