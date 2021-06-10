namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Team_ID
    {
        public static readonly GameEntity_Team_ID ID_NULL = new GameEntity_Team_ID(-1);
        public static readonly GameEntity_Team_ID TEAM_ONE_ID = new GameEntity_Team_ID(0);
        public static readonly GameEntity_Team_ID TEAM_TWO_ID = new GameEntity_Team_ID(1);

        public static readonly GameEntity_Team_ID[] TEAM_IDS = new GameEntity_Team_ID[]
        {
            TEAM_ONE_ID,
            TEAM_TWO_ID
        };
        
        public readonly int TEAM_ID;

        internal GameEntity_Team_ID(int teamID)
        {
            TEAM_ID = teamID;
        }

        public static implicit operator int(GameEntity_Team_ID teamID)
            => teamID.TEAM_ID;
    }
}