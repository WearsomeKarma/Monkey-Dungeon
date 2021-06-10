using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public class Combat_Ability_Target
    {
        private readonly GameEntity_Survey_Target SURVEY = new GameEntity_Survey_Target();

        public GameEntity_Position[] Get_Reduced_Fields(GameEntity_Team_ID teamId = null)
            => SURVEY.Get_Reduced_Field(teamId ?? GameEntity_Team_ID.ID_NULL);

        private GameEntity_Position Owner_Position;
        private GameEntity_Team_ID _ownerTeamId;

        public void Bind_To_Owner(GameEntity_Position ownerPosition, GameEntity_Team_ID ownerTeamId)
        {
            Owner_Position = ownerPosition;
            _ownerTeamId = ownerTeamId;
            Reset();
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

        public Combat_Ability_Target()
        {
            
        }

        public bool Has_Legal_Targets()
        {
            bool isValid;
            switch (Target_Type)
            {
                case Combat_Target_Type.Everything:
                    isValid = Check_Legal_Targets_For_Field();
                    isValid = isValid && Check_Legal_Targets_For_Field();
                    return isValid;
                case Combat_Target_Type.All_Enemies:
                case Combat_Target_Type.One_Enemy:
                case Combat_Target_Type.Two_Enemies:
                case Combat_Target_Type.Three_Enemies:
                    return Check_Legal_Targets_For_Field();
                default:
                    return Check_Legal_Targets_For_Field();
            }
        }

        private bool Check_Legal_Targets_For_Field()
        {
            int requiredCountInField = MD_PARTY.MAX_PARTY_SIZE;
            GameEntity_Position ownerPosition = GameEntity_Position.ID_NULL;
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

            return requiredCountInField ==
                   SURVEY.Get_Selected_Count(Owner_Position, targetTeamId, invertRosterTarget);
        }

        /// <summary>
        /// Returns true if the target was added. False if it wasn't.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool Add_Target(GameEntity_Position targetPosition)
        {
            //constraint null
            if (targetPosition == null || targetPosition == GameEntity_Position.ID_NULL)
                return false;

            //Check if the position is already added.
            if (SURVEY.Get_State_By_Position(targetPosition))
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
                    if (!isEnemyPosition && targetPosition == Owner_Position)
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
                    requiredCount = ((int) Target_Type) % MD_PARTY.MAX_PARTY_SIZE;
                    break;
            }

            bool additionDoesNotExceedCount = 
                requiredCount == SURVEY.Get_Selected_Count(Owner_Position, _ownerTeamId, isEnemyPosition);

            if (!additionDoesNotExceedCount)
                return false;
            
            SURVEY.Flag_Position(targetPosition);
            
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
            if (targetPosition == null || targetPosition == GameEntity_Position.ID_NULL)
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
                    if (SURVEY.Get_Selected_Count() <= 0)
                        return false;
                    if (!SURVEY.Get_Entry_From_Position(targetPosition))
                        return false;
                    break;
            }
            
            SURVEY.Unflag_Position(targetPosition);
            return true;
        }
        
        private void Reset(GameEntity_Team_ID teamId)
        {
            foreach (GameEntity_Position position in GameEntity_Position.ALL_NON_NULL__POSITIONS)
            {
                SURVEY.Unflag_Position(position);
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
            return string.Format("Target_Ids: [{0}]", string.Join<GameEntity_Position>(", ", Get_Reduced_Fields()));
        }
    }
}
