using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Schema;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public class Combat_Survey_Target : GameEntity_Survey<bool>
    {
        public GameEntity_Position[] Get__Targeted_Hostile_Positions__Survey_Target(GameEntity_Team_ID friendlyTeamId)
        {
            List<GameEntity_Position> targetedPositions = new List<GameEntity_Position>();

            GameEntity_Position.For_Each__Hostile_Position(friendlyTeamId, (p) =>
            {
                if(FIELD[p])
                    targetedPositions.Add(p);
            });
            
            return targetedPositions.ToArray();
        }
        
        public GameEntity_Position[] Get__Targeted_Positions__Survey_Target(GameEntity_Team_ID teamId = null)
        {
            teamId = teamId ?? GameEntity_Team_ID.ID_NULL;

            List<GameEntity_Position> targetedPositions = new List<GameEntity_Position>();

            GameEntity_Position.For_Each__Position(teamId, (p) =>
            {
                if(FIELD[p])
                    targetedPositions.Add(p);
            });
            
            return targetedPositions.ToArray();
        }

        private GameEntity_Position Owner_Position;
        private GameEntity_Team_ID _ownerTeamId;

        public void Bind_To_Action(GameEntity_Position ownerPosition, GameEntity_Team_ID ownerTeamId, Combat_Target_Type targetType, bool hasStrictTargets)
        {
            Owner_Position = ownerPosition;
            _ownerTeamId = ownerTeamId;

            Has_Strict_Targets = hasStrictTargets;
            Target_Type = targetType;
        }

        private Combat_Target_Type targetType = Combat_Target_Type.Self_Or_No_Target;

        /// <summary>
        /// Changing the target type will reset all established targets if new value is illegal with current targets.
        /// </summary>
        public Combat_Target_Type Target_Type
        {
            get => targetType;
            set
            {
                targetType = value;
                if (!Has_Legal_Targets())
                    Reset();
            }
        }

        public bool Has_Strict_Targets { get; set; }

        public Combat_Survey_Target()
            : base(() => false)
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
            
            bool ownerPositionExclusive = ownerPosition != GameEntity_Position.NULL_POSITION;

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
            ownerPosition = ownerPosition ?? GameEntity_Position.NULL_POSITION;
            teamIdTarget = teamIdTarget ?? GameEntity_Team_ID.ID_NULL; 
            
            int count = 0;
            
            foreach (GameEntity_Position position in GameEntity_Position.ALL_NON_NULL__POSITIONS)
            {
                if (IsValid_For_Count(position, ownerPosition, teamIdTarget, invertRosterTarget))
                {
                    count++;
                }
            }

            return count;
        }
        
        public bool Has_Legal_Targets()
        {
            int requiredCountInField = MD_PARTY.MAX_PARTY_SIZE;
            GameEntity_Position ownerPosition = GameEntity_Position.NULL_POSITION;
            bool invertRosterTarget = false;
            GameEntity_Team_ID targetTeamId = _ownerTeamId;
            
            switch (Target_Type)
            {
                case Combat_Target_Type.Self_Or_No_Target:
                    Reset();
                    return true;
                case Combat_Target_Type.Everything:
                    requiredCountInField = MD_PARTY.MAX_PARTY_SIZE * 2;
                    targetTeamId = GameEntity_Team_ID.ID_NULL;
                    break;
                case Combat_Target_Type.All_Enemies:
                    invertRosterTarget = true;
                    break;
                case Combat_Target_Type.One_Enemy:    
                case Combat_Target_Type.Two_Enemies:
                case Combat_Target_Type.Three_Enemies: 
                    invertRosterTarget = true;
                    requiredCountInField = ((int) Target_Type % MD_PARTY.MAX_PARTY_SIZE) + 1;
                    break;
                case Combat_Target_Type.All_Friendlies:
                    targetTeamId = _ownerTeamId;
                    break;
                case Combat_Target_Type.One_Ally:
                case Combat_Target_Type.Two_Allies:
                case Combat_Target_Type.Three_Allies:
                    requiredCountInField = (int)Target_Type;
                    ownerPosition = Owner_Position;
                    break;
                 default:   
                    requiredCountInField = ((int) Target_Type % MD_PARTY.MAX_PARTY_SIZE) + 1;
                    break;
            }

            int acquiredCount = Get_Selected_Count(ownerPosition, targetTeamId, invertRosterTarget);

            return requiredCountInField == acquiredCount ||
                   (!Has_Strict_Targets && requiredCountInField > 0);
        }

        /// <summary>
        /// Returns true if the target was added. False if it wasn't.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool Add_Target(GameEntity_Position targetPosition)
        {
            Console.WriteLine("[Combat_Ability_Target.cs:103] Adding Target: " + targetPosition);
            
            //constraint null
            if (!GameEntity_Position.Validate(targetPosition))
                return false;
            
            //Check if the position is already added.
            if (FIELD[targetPosition])
                return true;
            
            bool isEnemyPosition = targetPosition.TeamId != _ownerTeamId;
            
            //Validate target type. IE, make sure its not an enemy position if we can only target allies. Vice Versa.
            switch (Target_Type)
            {
                //We don't utilize targets. Ignore add request.
                case Combat_Target_Type.Self_Or_No_Target:
                    return false;
                
                //Our targets are hard set. Ignore add request.
                case Combat_Target_Type.Everything:
                case Combat_Target_Type.All_Enemies:
                case Combat_Target_Type.All_Friendlies:
                    return false;
                
                //Verify position is an enemy position.
                case Combat_Target_Type.One_Enemy:
                case Combat_Target_Type.Two_Enemies:
                case Combat_Target_Type.Three_Enemies:
                    if (!isEnemyPosition)
                        return false;
                    break;
                
                //Verify position is an ally position and not an owner position.
                case Combat_Target_Type.One_Ally:
                case Combat_Target_Type.Two_Allies:
                case Combat_Target_Type.Three_Allies:
                    if (isEnemyPosition && targetPosition == Owner_Position)
                        return false;
                    break;
                
                //Verify position is an ally position.
                default:
                    if (isEnemyPosition)
                        return false;
                    break;
            }
            
            int requiredCount = 0;
            //Check that we do not exceed the count for One-Three target types.
            switch (Target_Type)
            {
                case Combat_Target_Type.One_Ally:
                case Combat_Target_Type.Two_Allies:
                case Combat_Target_Type.Three_Allies:
                    requiredCount = (int) Target_Type;
                    break;
                default:
                    requiredCount = (((int) Target_Type) % MD_PARTY.MAX_PARTY_SIZE) + 1;
                    break;
            }
            
            bool additionExceedsCount = 
                requiredCount < Get_Selected_Count(Owner_Position, _ownerTeamId, isEnemyPosition) + 1;

            if (additionExceedsCount)
                return false;
            
            Set__Entry_By_Position__Survey(targetPosition, true);
            
            return true;
        }

        /// <summary>
        /// Returns true if target was removed. False if it wasn't.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool Remove_Target(GameEntity_Position targetPosition)
        {
            //constraint null
            if (targetPosition == null || targetPosition == GameEntity_Position.NULL_POSITION)
                return false;
            
            //Make sure a target can be removed.
            switch (Target_Type)
            {
                //Cannot remove hard set targets.
                case Combat_Target_Type.Everything:
                case Combat_Target_Type.All_Enemies:
                case Combat_Target_Type.All_Friendlies:
                case Combat_Target_Type.Self_Or_No_Target:    
                    return false;
                default:
                    if (Get_Selected_Count() <= 0)
                        return false;
                    if (!FIELD[targetPosition])
                        return false;
                    break;
            }
            
            Set__Entry_By_Position__Survey(targetPosition, false);
            return true;
        }
        
        private void Reset(GameEntity_Team_ID teamId)
        {
            foreach (GameEntity_Position targetPosition in GameEntity_Position.ALL_NON_NULL__POSITIONS)
            {
                Set__Entry_By_Position__Survey(targetPosition, false);
            }
        }
        
        public void Reset_Ally_Targets()
            => Reset(GameEntity_Team_ID.TEAM_ONE_ID);

        public void Reset_Enemy_Targets()
            => Reset(GameEntity_Team_ID.TEAM_TWO_ID);
        
        public void Reset()
        {
            Reset_Ally_Targets();
            Reset_Enemy_Targets();
        }
        
        public override string ToString()
        {
            return string.Format("Type: [{0}], Target_Ids: [{1}]", Target_Type, string.Join<GameEntity_Position>(", ", Get__Targeted_Positions__Survey_Target()));
        }
    }
}
