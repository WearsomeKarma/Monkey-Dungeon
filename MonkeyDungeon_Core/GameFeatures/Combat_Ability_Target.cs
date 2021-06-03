using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class Combat_Ability_Target
    {
        private readonly GameEntity_ID[] LOW_IDs = new GameEntity_ID[MD_PARTY.MAX_PARTY_SIZE];
        private readonly GameEntity_ID[] HIGH_IDs = new GameEntity_ID[MD_PARTY.MAX_PARTY_SIZE];
        
        public bool Is_Target_Melee(GameEntity_ID id)
            => id % MD_PARTY.MAX_PARTY_SIZE < (MD_PARTY.MAX_PARTY_SIZE / 2);

        public bool Is_Target_Range(GameEntity_ID id)
            => id % MD_PARTY.MAX_PARTY_SIZE >= (MD_PARTY.MAX_PARTY_SIZE / 2);

        private GameEntity_ID[] Get_Team_By_Targeter_Perspective(bool getAllyTeam)
            => ((GameEntity_ID.ID_ONE.Roster_ID == Targeter.Roster_ID) == getAllyTeam) ? LOW_IDs : HIGH_IDs;

        private void Reset_Field(GameEntity_ID[] ids)
        {
            for(int i=0;i<ids.Length;i++)
                ids[i] = GameEntity_ID.ID_NULL;
        }
        
        private void Reset()
        {
            Reset_Field(LOW_IDs);
            Reset_Field(HIGH_IDs);
        }
        
        private int Count(GameEntity_ID[] ids)
        {
            int ret = 0;

            for(int i=0;i<ids.Length;i++)
                if (ids[i] != GameEntity_ID.ID_NULL)
                    ret++;
            
            return ret;
        }

        public int Ally_Target_Count
            => Count(Get_Team_By_Targeter_Perspective(true));

        public int Enemy_Target_Count
            => Count(Get_Team_By_Targeter_Perspective(false));
        
        private void Flag(int index, GameEntity_ID id)
        {
            if (index > MD_PARTY.MAX_PARTY_SIZE)
            {
                HIGH_IDs[index % MD_PARTY.MAX_PARTY_SIZE] = id;
                return;
            }

            LOW_IDs[index] = id;
        }
        
        internal void Flag_Target(GameEntity_ID id)
        {
            Flag(id, id);
        }

        internal void Unflag_Target(GameEntity_ID id)
        {
            Flag(id, GameEntity_ID.ID_NULL);
        }

        public GameEntity_ID Targeter { get; private set; }

        public Combat_Target_Type Target_Type { get; private set; }
        public bool Has_Strict_Targets { get; private set; }

        internal bool Has_Valid_Targets()
        {
            int allyCount = Ally_Target_Count;
            int enemyCount = Enemy_Target_Count;
            int totalCount = allyCount + enemyCount;

            int strictCount, requiredCount;
            switch (Target_Type)
            {
                default:
                    return true;
                case Combat_Target_Type.One_Friendly:
                case Combat_Target_Type.Two_Friendlies:
                case Combat_Target_Type.Three_Friendlies:
                case Combat_Target_Type.One_Ally:
                case Combat_Target_Type.Two_Allies:
                case Combat_Target_Type.Three_Allies:
                    strictCount = allyCount;
                    requiredCount = (int) Target_Type % MD_PARTY.MAX_PARTY_SIZE + ((int) Target_Type/(MD_PARTY.MAX_PARTY_SIZE*2));
                    break;
                case Combat_Target_Type.One_Enemy:
                case Combat_Target_Type.Two_Enemies:
                case Combat_Target_Type.Three_Enemies:
                    strictCount = enemyCount;
                    requiredCount = ((int) Target_Type % MD_PARTY.MAX_PARTY_SIZE) + 1;
                    break;
            }

            if (Has_Strict_Targets)
                return strictCount == requiredCount;
            return !MathHelper.Breaks_Clampd(strictCount, 1, requiredCount);
        }

        private void Get_Targets_From_Field(List<GameEntity_ID> list, GameEntity_ID[] ids)
        {
            foreach (GameEntity_ID t in ids)
                if (t != GameEntity_ID.ID_NULL)
                    list.Add(t);
        }
        
        public GameEntity_ID[] Get_Targets()
        {
            List<GameEntity_ID> targets = new List<GameEntity_ID>();
            Get_Targets_From_Field(targets, LOW_IDs);
            Get_Targets_From_Field(targets, HIGH_IDs);

            return targets.ToArray();
        }

        internal void Set_Target_Criteria(GameEntity_ID targeter, Combat_Target_Type targetType, bool targetsAreStrict = false)
        {
            Targeter = targeter;
            Target_Type = targetType;
            Has_Strict_Targets = targetsAreStrict;
            
            Reset();

            switch(Target_Type)
            {
                case Combat_Target_Type.Self_Or_No_Target:
                    Flag_Target(Targeter);
                    break;
                case Combat_Target_Type.Everything:
                    Flag_Allies();
                    Flag_Enemies();
                    break;
                case Combat_Target_Type.All_Enemies:
                    Flag_Enemies();
                    break;
                case Combat_Target_Type.All_Friendlies:
                    Flag_Allies();
                    break;
            }
        }

        public bool Add_Target(GameEntity_ID entityId)
        {
            switch(Target_Type)
            {
                case Combat_Target_Type.Three_Allies:
                case Combat_Target_Type.Two_Allies:
                case Combat_Target_Type.One_Ally:
                    if (entityId == Targeter || entityId.Roster_ID != Targeter.Roster_ID)
                        return false;
                    break;
                case Combat_Target_Type.Three_Enemies:
                case Combat_Target_Type.Two_Enemies:
                case Combat_Target_Type.One_Enemy:
                    if (entityId.Roster_ID == Targeter.Roster_ID)
                        return false;
                    break;
                case Combat_Target_Type.Three_Friendlies:
                case Combat_Target_Type.Two_Friendlies:
                case Combat_Target_Type.One_Friendly:
                    if (entityId.Roster_ID != Targeter.Roster_ID)
                        return false;
                    break;
                default:
                    return false;
            }

            Flag_Target(entityId);
            return true;
        }

        private int Increment_Target_Count(int count, int offset, int limit)
            => ((count + 1) % limit) + offset;

        private void Of_All(bool allySideOrNot, Action<GameEntity_ID> operation)
        {
            GameEntity_ID[] inspectedField = Get_Team_By_Targeter_Perspective(allySideOrNot);
            
            foreach(GameEntity_ID id in inspectedField)
                if (
                    id != GameEntity_ID.ID_NULL
                    )
                    operation(id);
        }

        private void Flag_Allies()
            => Of_All(true, Flag_Target);

        private void Flag_Enemies()
            => Of_All(false, Unflag_Target);

        public override string ToString()
        {
            return string.Format("Target_Ids: [{0}]", string.Join< GameEntity_ID>(", ", Get_Targets()));
        }
    }
}
