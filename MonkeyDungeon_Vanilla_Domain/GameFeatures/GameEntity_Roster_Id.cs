namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Roster_Id
    {
        public static readonly GameEntity_Roster_Id ROSTER_1 = new GameEntity_Roster_Id(0);
        public static readonly GameEntity_Roster_Id ROSTER_2 = new GameEntity_Roster_Id(1);

        public static readonly GameEntity_Roster_Id[] ROSTER_IDS = new GameEntity_Roster_Id[]
        {
            ROSTER_1,
            ROSTER_2
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