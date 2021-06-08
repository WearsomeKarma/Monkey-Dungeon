using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Target_Field
    {
        internal readonly Dictionary<GameEntity_Position, bool> FIELD;

        internal GameEntity_Target_Field()
        {
            FIELD = new Dictionary<GameEntity_Position, bool>()
            {
                { GameEntity_Position.FRONT_RIGHT, false },
                { GameEntity_Position.FRONT_LEFT, false },
                { GameEntity_Position.REAR_RIGHT, false },
                { GameEntity_Position.REAR_LEFT, false }
            };
        }

        public int Get_Selected_Count(GameEntity_Position ownerPosition = null)
        {
            ownerPosition = ownerPosition ??GameEntity_Position.NULL_POSITION;
            bool comparePosition = ownerPosition != GameEntity_Position.NULL_POSITION;
            
            int count = 0;
            
            foreach (GameEntity_Position position in GameEntity_Position.POSITIONS)
            {
                bool positionIsSelected = FIELD[position];
                if (positionIsSelected && position == ownerPosition)
                    return -1;
                count += (positionIsSelected) ? 1 : 0;
            }

            return count;
        }

        private void Set_Position(GameEntity_Position position, bool value)
        {
            if (position == GameEntity_Position.NULL_POSITION)
                return;
            
            FIELD[position] = value;
        }

        public void Flag_Position(GameEntity_Position position)
            => Set_Position(position, true);

        public void Unflag_Position(GameEntity_Position position)
            => Set_Position(position, false);
    }
}