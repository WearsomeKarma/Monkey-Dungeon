using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using OpenTK;

namespace MonkeyDungeon_UI.Prefabs
{
    public class GameEntity_Position_Vector_Survey : GameEntity_Survey<Vector3>
    {
        private readonly Vector3 BASIS_X, BASIS_Y;
        private readonly Vector3[] Roster_Positions = new Vector3[MD_PARTY.MAX_PARTY_SIZE];
        
        internal GameEntity_Position_Vector_Survey(Vector3 basisX, Vector3 basisY) 
            : base(Vector3.Zero)
        {
            BASIS_X = basisX;
            BASIS_Y = basisY;
            
            GameEntity_Position.For_Each_Position(GameEntity_Team_ID.ID_NULL, Set__Position_With_Basis);
        }

        internal GameEntity_Position_Vector_Survey(Vector3[] fixedPositions)
            : base(Vector3.Zero)
        {
            for (int i = 0; i < Roster_Positions.Length; i++)
                Roster_Positions[i] = fixedPositions[i];
            
            GameEntity_Position.For_Each_Position(GameEntity_Team_ID.ID_NULL, Set__Position_With_FixedPositions);
        }
        
        public Vector3 Map(GameEntity_Position position)
        {
            return Get_Entry_From_Position(position);
        }

        private void Set__Position_With_Basis(GameEntity_Position position)
        {
            GameEntity_Position_Type typeFromPosition = (GameEntity_Position_Type) position;

            int scalar_HorizontalOffset = (int) typeFromPosition % MD_PARTY.MAX_PARTY_SIZE_HALF;
            int scalar_VerticalOffset = (int) typeFromPosition / MD_PARTY.MAX_PARTY_SIZE_HALF;

            int scalar_Flip = position.TeamId == GameEntity_Team_ID.TEAM_ONE_ID ? -1 : 1;
            
            Vector3 vec = scalar_Flip * ((scalar_HorizontalOffset * BASIS_X) + (scalar_VerticalOffset * BASIS_Y));
            
            Set_Entry_By_Position(position, vec);
        }

        private void Set__Position_With_FixedPositions(GameEntity_Position position)
        {
            int scalar_Flip = position.TeamId == GameEntity_Team_ID.TEAM_ONE_ID ? -1 : 1;
            Set_Entry_By_Position(position, scalar_Flip * Roster_Positions[position.WORLD_POSITION]);
        }
    }
}