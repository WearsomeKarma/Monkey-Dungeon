namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Roster_Id
    {
        public static readonly GameEntity_Roster_Id __NULL____ROSTER_ID = new GameEntity_Roster_Id(-1);
        public static readonly GameEntity_Roster_Id TEAM_ONE__ROSTER_ID = new GameEntity_Roster_Id(0);
        public static readonly GameEntity_Roster_Id TEAM_TWO__ROSTER_ID = new GameEntity_Roster_Id(1);

        public static readonly GameEntity_Roster_Id[] ROSTER_IDS = new GameEntity_Roster_Id[]
        {
            TEAM_ONE__ROSTER_ID,
            TEAM_TWO__ROSTER_ID
        };
        
        public readonly int ROSTER_ID;

        internal GameEntity_Roster_Id(int rosterID)
        {
            ROSTER_ID = rosterID;
        }

        public static implicit operator int(GameEntity_Roster_Id rosterID)
            => rosterID.ROSTER_ID;
    }
}