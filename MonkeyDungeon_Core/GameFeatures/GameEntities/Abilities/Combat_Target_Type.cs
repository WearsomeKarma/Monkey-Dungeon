
namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public enum Combat_Target_Type
    {
        /// <summary>
        /// No target.
        /// </summary>
        Self_Or_No_Target = 0,

        /// <summary>
        /// Exclusive to caster.
        /// </summary>
        One_Ally = 1,
        /// <summary>
        /// Exclusive to caster.
        /// </summary>
        Two_Allies = 2,
        /// <summary>
        /// Exclusive to caster.
        /// </summary>
        Three_Allies = 3,

        One_Enemy = 4,
        Two_Enemies = 5,
        Three_Enemies = 6,
        All_Enemies = 7,

        /// <summary>
        /// Inclusive to caster.
        /// </summary>
        One_Friendly = 8,
        /// <summary>
        /// Inclusive to caster.
        /// </summary>
        Two_Friendlies = 9,
        /// <summary>
        /// Inclusive to caster.
        /// </summary>
        Three_Friendlies = 10,

        /// <summary>
        /// Inclusive to caster.
        /// </summary>
        All_Friendlies = 11,
        
        /// <summary>
        /// Inclusive to caster, and both teams.
        /// </summary>
        Everything = 12,
    }
}
