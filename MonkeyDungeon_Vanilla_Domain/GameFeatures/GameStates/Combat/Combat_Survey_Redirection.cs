﻿namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public class Combat_Survey_Redirection : GameEntity_Survey<Combat_Redirection_Chance>
    {
        public Combat_Redirection_Chance[] Get__Chances_For_Hostiles__Survey_Redirection(GameEntity_Team_ID friendTeamId)
            => Get__Reduced_Field_Of_Hostiles__Survey(friendTeamId);

        public GameEntity_Position[] Get__Positions_Of_Hostiles__Survey_Redirection(GameEntity_Team_ID friendlyTeamId)
            => Get__Reduced_Positions_Of_Hostiles__Survey(friendlyTeamId);

        public GameEntity_Position[] Get__Positions__Survey_Redirection()
            => Get__Reduced_Positions__Survey();

        public Combat_Survey_Redirection()
            : base(() => new Combat_Redirection_Chance(GameEntity_Position_Swap_Type.No_Swap, 1, GameEntity__Quantity_Modification_Type.None))
        {
        }

        protected override bool Check_If__Equivalent_To_Default__Survey(Combat_Redirection_Chance value)
        {
            return value.Redirection_Chance__Redirection_Type == GameEntity_Position_Swap_Type.No_Swap;
        }
    }
}