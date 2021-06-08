using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public class Combat_Ability_Target
    {
        private readonly GameEntity_Target_Field FIELD_1 = new GameEntity_Target_Field();
        private readonly GameEntity_Target_Field FIELD_2 = new GameEntity_Target_Field();

        private readonly GameEntity_Target_Field[] FIELDS;

        private GameEntity_ID Owner_ID;

        public void Bind_To_Owner(GameEntity_ID owner)
        {
            Owner_ID = owner;
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

        private GameEntity_Target_Field Get_Field(bool getAllyField = true)
        {
            if (Owner_ID.Roster_ID == 0)
                return FIELD_1;
            return FIELD_2;
        }

        public Combat_Ability_Target()
        {
            FIELDS = new GameEntity_Target_Field[]
            {
                FIELD_1,
                FIELD_2
            };
        }
        
        private GameEntity_Target_Field Ally_TargetField
            => Get_Field();

        private GameEntity_Target_Field Enemy_TargetField
            => Get_Field(false);

        private void Reset(GameEntity_Roster_Id rosterID)
        {
            foreach (GameEntity_Position position in GameEntity_Position.POSITIONS)
            {
                if (rosterID == GameEntity_Roster_Id.ROSTER_1)
                    FIELD_1.Unflag_Position(position);
                FIELD_2.Unflag_Position(position);
            }
        }

        public bool Has_Legal_Targets()
        {
            switch (Target_Type)
            {
                
            }
        }

        private bool Check_Legal_Targets_For_Roster(GameEntity_Roster_Id rosterID)
        {
            GameEntity_Target_Field inspectedTargetField = FIELDS[rosterID];

            int requiredCountInField = 0;
            bool ownerExclusive = false;
            
            switch (Target_Type)
            {
                case Combat_Target_Type.Self_Or_No_Target:
                    Reset();
                    return true;
                case Combat_Target_Type.Everything:
                case Combat_Target_Type.All_Enemies:
                case Combat_Target_Type.All_Friendlies:
                    requiredCountInField = MD_PARTY.MAX_PARTY_SIZE;
                    break;
                case Combat_Target_Type.One_Ally:
                case Combat_Target_Type.Two_Allies:
                case Combat_Target_Type.Three_Allies:
                    requiredCountInField = (int)Target_Type;
                    ownerExclusive = true;
                    break;
                 default:   
                    requiredCountInField = ((int) Target_Type % MD_PARTY.MAX_PARTY_SIZE) + 1;
                    break;
            }

            return requiredCountInField ==
                   inspectedTargetField.Get_Selected_Count(ownerExclusive ? Owner_ID : GameEntity_ID.ID_NULL);
        }

        public void Reset_Ally_Targets()
            => Reset(GameEntity_Roster_Id.ROSTER_1);

        public void Reset_Enemy_Targets()
            => Reset(GameEntity_Roster_Id.ROSTER_2);
        
        public void Reset()
        {
            Reset_Ally_Targets();
            Reset_Enemy_Targets();
        }
        
        public override string ToString()
        {
            return string.Format("Target_Ids: [{0}]", string.Join< GameEntity_ID>(", ", Get_Targets()));
        }
    }
}
