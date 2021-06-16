using System;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Position
    {
        public static readonly GameEntity_Position NULL_POSITION         = new GameEntity_Position(-1, GameEntity_Team_ID.ID_NULL);
        
        public static readonly GameEntity_Position TEAM_ONE__FRONT_RIGHT = new GameEntity_Position( 0, GameEntity_Team_ID.TEAM_ONE_ID);
        public static readonly GameEntity_Position TEAM_ONE__FRONT_LEFT  = new GameEntity_Position( 1, GameEntity_Team_ID.TEAM_ONE_ID);
        public static readonly GameEntity_Position TEAM_ONE__REAR_RIGHT  = new GameEntity_Position( 2, GameEntity_Team_ID.TEAM_ONE_ID);
        public static readonly GameEntity_Position TEAM_ONE__REAR_LEFT   = new GameEntity_Position( 3, GameEntity_Team_ID.TEAM_ONE_ID);

        public static readonly GameEntity_Position TEAM_TWO__FRONT_RIGHT = new GameEntity_Position( 0, GameEntity_Team_ID.TEAM_TWO_ID);
        public static readonly GameEntity_Position TEAM_TWO__FRONT_LEFT  = new GameEntity_Position( 1, GameEntity_Team_ID.TEAM_TWO_ID);
        public static readonly GameEntity_Position TEAM_TWO__REAR_RIGHT  = new GameEntity_Position( 2, GameEntity_Team_ID.TEAM_TWO_ID);
        public static readonly GameEntity_Position TEAM_TWO__REAR_LEFT   = new GameEntity_Position( 3, GameEntity_Team_ID.TEAM_TWO_ID);

        public static readonly GameEntity_Position[] TEAM_NULL_POSITIONS = new GameEntity_Position[]
        {
            NULL_POSITION
        };
        
        public static readonly GameEntity_Position[] TEAM_ONE__POSITIONS = new GameEntity_Position[]
        {
            TEAM_ONE__FRONT_RIGHT,
            TEAM_ONE__FRONT_LEFT,
            TEAM_ONE__REAR_RIGHT,
            TEAM_ONE__REAR_LEFT
        };

        public static readonly GameEntity_Position[] TEAM_TWO__POSITIONS = new GameEntity_Position[]
        {
            TEAM_TWO__FRONT_RIGHT,
            TEAM_TWO__FRONT_LEFT,
            TEAM_TWO__REAR_RIGHT,
            TEAM_TWO__REAR_LEFT
        };

        public static readonly GameEntity_Position[] ALL_NON_NULL__POSITIONS = new GameEntity_Position[]
        {
            TEAM_ONE__FRONT_RIGHT,
            TEAM_ONE__FRONT_LEFT,
            TEAM_ONE__REAR_RIGHT,
            TEAM_ONE__REAR_LEFT,
            TEAM_TWO__FRONT_RIGHT,
            TEAM_TWO__FRONT_LEFT,
            TEAM_TWO__REAR_RIGHT,
            TEAM_TWO__REAR_LEFT
        };
        
        private static readonly GameEntity_Position[][] POSITIONS_BY_ROSTER = new GameEntity_Position[][]
        {
            TEAM_ONE__POSITIONS,
            TEAM_TWO__POSITIONS
        };

        public static GameEntity_Position[] Get_Legal_Positions_By_RosterID(GameEntity_Team_ID teamId)
        {
            //constraint null
            teamId = teamId ?? GameEntity_Team_ID.ID_NULL;
            
            if (teamId == GameEntity_Team_ID.ID_NULL)
                return TEAM_NULL_POSITIONS;
            return POSITIONS_BY_ROSTER[teamId];
        }

        public static GameEntity_Position Get_Default_Position_From_EntityID(GameEntity_ID id, GameEntity_Position nullPosition = null)
        {
            //constraint null
            id = id ?? GameEntity_ID.ID_NULL;
            nullPosition = nullPosition ?? NULL_POSITION;
            
            if (id == GameEntity_ID.ID_NULL || id.Team_Id == GameEntity_Team_ID.ID_NULL)
                return nullPosition;

            return Get_Legal_Positions_By_RosterID(id.Team_Id)[id % MD_PARTY.MAX_PARTY_SIZE];
        }

        public static GameEntity_Position Get_Position_From_Type
        (
            GameEntity_Position_Type positionType,
            GameEntity_Team_ID teamId,
            GameEntity_Position nullPosition = null
            )
        {
            nullPosition = nullPosition ?? NULL_POSITION;
            
            if (teamId == null || teamId == GameEntity_Team_ID.ID_NULL)
                return nullPosition;
            
            return POSITIONS_BY_ROSTER[teamId][(int) positionType];
        }
        
        public readonly int WORLD_POSITION;
        public readonly GameEntity_Team_ID TeamId; 
        
        private int Modulo_Position => WORLD_POSITION / 2; //0 or 1
        private int Modulo_Position_Low => (Modulo_Position) * MD_PARTY.MAX_PARTY_SIZE_HALF;
        private int Modulo_Position_High => (Modulo_Position + 1) * MD_PARTY.MAX_PARTY_SIZE_HALF;

        public int Horizontal_Adjacent_Position => (Modulo_Position_Low) + ((WORLD_POSITION + 1) % MD_PARTY.MAX_PARTY_SIZE_HALF);
        public int Vertical_Adjacent_Position => (WORLD_POSITION + MD_PARTY.MAX_PARTY_SIZE_3_2) % MD_PARTY.MAX_PARTY_SIZE;
        
        internal GameEntity_Position(int initalPosition, GameEntity_Team_ID teamId)
        {
            WORLD_POSITION = initalPosition;
            TeamId = teamId;
        }

        public GameEntity_Position Get_Horizontal_Swap()
        {
            if (this == NULL_POSITION)
                return this;
            return Get_Legal_Positions_By_RosterID(TeamId)[Horizontal_Adjacent_Position];
        }

        public GameEntity_Position Get_Vertical_Swap()
        {
            if (this == NULL_POSITION)
                return this;
            return Get_Legal_Positions_By_RosterID(TeamId)[Vertical_Adjacent_Position];
        }

        public GameEntity_Position Get_Swap(GameEntity_Position_Swap_Type swapType)
        {
            switch (swapType)
            {
                case GameEntity_Position_Swap_Type.No_Swap:
                    return this;
                case GameEntity_Position_Swap_Type.Swap_Horizontal:
                    return Get_Horizontal_Swap();
                case GameEntity_Position_Swap_Type.Swap_Vertical:
                    return Get_Vertical_Swap();
                case GameEntity_Position_Swap_Type.Swap_Diagonal:
                    return Get_Horizontal_Swap().Get_Vertical_Swap();
                default:
                    return NULL_POSITION;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}, Team:{1}", ((GameEntity_Position_Type) WORLD_POSITION).ToString(), TeamId);
        }

        public static explicit operator GameEntity_Position_Type(GameEntity_Position position)
            => (GameEntity_Position_Type) (position.WORLD_POSITION);

        public static explicit operator int(GameEntity_Position position)
            => position.WORLD_POSITION + (MD_PARTY.MAX_PARTY_SIZE * position.TeamId);

        /// <summary>
        /// General foreach wrapper function that will operate over team specific positions, or all positions (teamID=ID_NULL)
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="action"></param>
        public static void For_Each_Position(GameEntity_Team_ID teamID, Action<GameEntity_Position> action)
        {
            //function off of possible null!
            GameEntity_Position[] positionField = 
                teamID == null || teamID == GameEntity_Team_ID.ID_NULL 
                    ? ALL_NON_NULL__POSITIONS 
                    : POSITIONS_BY_ROSTER[teamID];
            
            foreach (GameEntity_Position position in positionField)
                action(position);
        }
        
        public static bool Validate(GameEntity_Position position)
            => !((position == null) || (position == NULL_POSITION));

        public static GameEntity_Position From_Type(GameEntity_Position_Type positionType, GameEntity_Team_ID teamId)
            => ALL_NON_NULL__POSITIONS[((int) positionType) + (MD_PARTY.MAX_PARTY_SIZE * teamId)];
    }
}