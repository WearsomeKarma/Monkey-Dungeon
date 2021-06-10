using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public class Combat_Ability_Target
    {
        private readonly GameEntity_Field_Target FIELD = new GameEntity_Field_Target();

        public GameEntity_Position[] Get_Reduced_Fields(GameEntity_Roster_Id rosterID = null)
            => FIELD.Get_Reduced_Field(rosterID ?? GameEntity_Roster_Id.__NULL____ROSTER_ID);

        private GameEntity_Position Owner_Position;
        private GameEntity_Roster_Id Owner_Roster_ID;

        public void Bind_To_Owner(GameEntity_Position ownerPosition, GameEntity_Roster_Id ownerRosterID)
        {
            Owner_Position = ownerPosition;
            Owner_Roster_ID = ownerRosterID;
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
            GameEntity_Position ownerPosition = GameEntity_Position.__NULL____POSITION;
            bool invertRosterTarget = false;
            GameEntity_Roster_Id target_RosterID = Owner_Roster_ID;
            
            switch (Target_Type)
            {
                case Combat_Target_Type.Self_Or_No_Target:
                    Reset();
                    return true;
                case Combat_Target_Type.Everything:
                    requiredCountInField = MD_PARTY.MAX_PARTY_SIZE * 2;
                    target_RosterID = GameEntity_Roster_Id.__NULL____ROSTER_ID;
                    break;
                case Combat_Target_Type.All_Enemies:
                    invertRosterTarget = true;
                    break;
                case Combat_Target_Type.All_Friendlies:
                    target_RosterID = Owner_Roster_ID;
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
                   FIELD.Get_Selected_Count(Owner_Position, target_RosterID, invertRosterTarget);
        }

        /// <summary>
        /// Returns true if the target was added. False if it wasn't.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool Add_Target(GameEntity_Position targetPosition)
        {
            //constraint null
            if (targetPosition == null || targetPosition == GameEntity_Position.__NULL____POSITION)
                return false;

            //Check if the position is already added.
            if (FIELD.Get_Entry_From_Position(targetPosition))
                return true;
            
            bool isEnemyPosition = targetPosition.ROSTER_ID != Owner_Roster_ID;
            
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
                requiredCount == FIELD.Get_Selected_Count(Owner_Position, Owner_Roster_ID, isEnemyPosition);

            if (!additionDoesNotExceedCount)
                return false;
            
            FIELD.Flag_Position(targetPosition);
            
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
            if (targetPosition == null || targetPosition == GameEntity_Position.__NULL____POSITION)
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
                    if (FIELD.Get_Selected_Count() <= 0)
                        return false;
                    if (!FIELD.Get_Entry_From_Position(targetPosition))
                        return false;
                    break;
            }
            
            FIELD.Unflag_Position(targetPosition);
            return true;
        }
        
        private void Reset(GameEntity_Roster_Id rosterID)
        {
            foreach (GameEntity_Position position in GameEntity_Position.ALL_NON_NULL__POSITIONS)
            {
                FIELD.Unflag_Position(position);
            }
        }
        
        public void Reset_Ally_Targets()
            => Reset(GameEntity_Roster_Id.TEAM_ONE__ROSTER_ID);

        public void Reset_Enemy_Targets()
            => Reset(GameEntity_Roster_Id.TEAM_TWO__ROSTER_ID);
        
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
