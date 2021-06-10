using System;
using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Survey_Target : GameEntity_Survey<bool>
    {
        public bool Get_State_By_Position(GameEntity_Position position)
            => Get_Entry_From_Position(position);
        
        private bool Position_Is_Of_Roster(GameEntity_Position position, GameEntity_Team_ID teamId)
        {
            //private function, no restraint needed.
            
            return FIELD[position]
                &&
                (
                teamId == GameEntity_Team_ID.ID_NULL
                ||
                teamId == position.TeamId
                );
        }
        
        public GameEntity_Position[] Get_Reduced_Field(GameEntity_Team_ID teamId)
        {
            //constaint null
            teamId = teamId ?? GameEntity_Team_ID.ID_NULL;
            
            List<GameEntity_Position> positions = new List<GameEntity_Position>();
            
            foreach(GameEntity_Position pos in FIELD.Keys)
                if (Position_Is_Of_Roster(pos, teamId))
                    positions.Add(pos);

            return positions.ToArray();
        }

        public GameEntity_Survey_Target()
            : base (false)
        {
        }

        private bool IsValid_For_Count
            (
            GameEntity_Position position_In_Question,
            GameEntity_Position ownerPosition, 
            GameEntity_Team_ID teamIdTarget,
            bool invertRosterTarget
            )
        {
            bool ret = FIELD[position_In_Question];
            
            bool ownerPositionExclusive = ownerPosition != GameEntity_Position.ID_NULL;

            bool rosterDependent = teamIdTarget != GameEntity_Team_ID.ID_NULL;

            if (ownerPositionExclusive && ret)
                ret = position_In_Question != ownerPosition;

            if (rosterDependent && ret)
            {
                ret =
                    (position_In_Question.TeamId == teamIdTarget)
                    ||
                    (position_In_Question.TeamId != teamIdTarget && invertRosterTarget);
            }
            
            return ret;
        }
        
        public int Get_Selected_Count(GameEntity_Position ownerPosition = null, GameEntity_Team_ID teamIdTarget = null, bool invertRosterTarget = false)
        {
            //constraint null
            ownerPosition = ownerPosition ?? GameEntity_Position.ID_NULL;
            teamIdTarget = teamIdTarget ?? GameEntity_Team_ID.ID_NULL; 
            
            int count = 0;
            
            foreach (GameEntity_Position position in GameEntity_Position.ALL_NON_NULL__POSITIONS)
            {
                if (IsValid_For_Count(position, ownerPosition, teamIdTarget, invertRosterTarget))
                {
                    count++;
                    continue;
                }
                
                return -1;
            }

            return count;
        }

        private void Set_Position(GameEntity_Position position, bool value)
        {
            if (position == GameEntity_Position.ID_NULL)
                return;
            
            FIELD[position] = value;
        }

        public void Flag_Position(GameEntity_Position position)
            => Set_Position(position, true);

        public void Unflag_Position(GameEntity_Position position)
            => Set_Position(position, false);

        public override string ToString()
        {
            return String.Format("[GameEntity_Target_Field]({0})", String.Join(", ", FIELD.Keys));
        }
    }
}