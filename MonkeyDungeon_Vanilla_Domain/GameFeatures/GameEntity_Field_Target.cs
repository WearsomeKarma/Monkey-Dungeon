using System;
using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Field_Target : GameEntity_Field<bool>
    {
        private bool Position_Is_Of_Roster(GameEntity_Position position, GameEntity_Roster_Id rosterID)
        {
            //private function, no restraint needed.
            
            return FIELD[position]
                &&
                (
                rosterID == GameEntity_Roster_Id.__NULL____ROSTER_ID
                ||
                rosterID == position.ROSTER_ID
                );
        }
        
        public GameEntity_Position[] Get_Reduced_Field(GameEntity_Roster_Id rosterID)
        {
            //constaint null
            rosterID = rosterID ?? GameEntity_Roster_Id.__NULL____ROSTER_ID;
            
            List<GameEntity_Position> positions = new List<GameEntity_Position>();
            
            foreach(GameEntity_Position pos in FIELD.Keys)
                if (Position_Is_Of_Roster(pos, rosterID))
                    positions.Add(pos);

            return positions.ToArray();
        }

        public GameEntity_Field_Target()
            : base (false)
        {
        }

        private bool IsValid_For_Count
            (
            GameEntity_Position position_In_Question,
            GameEntity_Position ownerPosition, 
            GameEntity_Roster_Id rosterID_Target,
            bool invertRosterTarget
            )
        {
            bool ret = FIELD[position_In_Question];
            
            bool ownerPositionExclusive = ownerPosition != GameEntity_Position.__NULL____POSITION;

            bool rosterDependent = rosterID_Target != GameEntity_Roster_Id.__NULL____ROSTER_ID;

            if (ownerPositionExclusive && ret)
                ret = position_In_Question != ownerPosition;

            if (rosterDependent && ret)
            {
                ret =
                    (position_In_Question.ROSTER_ID == rosterID_Target)
                    ||
                    (position_In_Question.ROSTER_ID != rosterID_Target && invertRosterTarget);
            }
            
            return ret;
        }
        
        public int Get_Selected_Count(GameEntity_Position ownerPosition = null, GameEntity_Roster_Id rosterID_Target = null, bool invertRosterTarget = false)
        {
            //constraint null
            ownerPosition = ownerPosition ?? GameEntity_Position.__NULL____POSITION;
            rosterID_Target = rosterID_Target ?? GameEntity_Roster_Id.__NULL____ROSTER_ID; 
            
            int count = 0;
            
            foreach (GameEntity_Position position in GameEntity_Position.ALL_NON_NULL__POSITIONS)
            {
                if (IsValid_For_Count(position, ownerPosition, rosterID_Target, invertRosterTarget))
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
            if (position == GameEntity_Position.__NULL____POSITION)
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