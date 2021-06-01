using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class Combat_Ability_Target
    {
        private readonly GameEntity_ID[] TARGET_POOL = new GameEntity_ID[GameEntity_ID.IDS.Length];
        
        private int Any_Target_Index { get; set; }
        private int Enemy_Target_Index { get; set; }
        private int Ally_Target_Index { get; set; }

        public GameEntity_ID Targeter { get; private set; }

        public Combat_Target_Type Target_Type { get; private set; }
        public bool Has_Strict_Targets { get; private set; }

        public bool Has_Ally_Targets(int requiredCount = -1)
        {
            int count = Of_All_Set_Count(true);
            return Matches_Count(count, requiredCount);
        }

        public bool Has_Enemy_Targets(int requiredCount = -1)
        {
            int count = Of_All_Set_Count(false);
            return Matches_Count(count, requiredCount);
        }

        /// <summary>
        /// If null, this means an invalid target state has occured and a reset is required.
        /// </summary>
        /// <returns></returns>
        public bool? Targets_Set()
            => Has_Valid_Targets(Has_Strict_Targets, Of_All_Set_Count(true), Of_All_Set_Count(false));

        private bool? Has_Valid_Targets(bool isStrict, int allyCount, int enemyCount)
        {
            if (Target_Type == Combat_Target_Type.Self_Or_No_Target)
                return true;

            int requiredCount = (int)Target_Type % MD_PARTY.MAX_PARTY_SIZE;
            requiredCount++;

            int anti_integrityCount = -1;
            int integrityCount = -1;

            bool exclusiveOfSelf =
                Target_Type != Combat_Target_Type.Self_Or_No_Target
                &&
                (int)Target_Type < 4;
            
            if(exclusiveOfSelf)
            {
                bool breaks = false;
                Of_All(true, (i) => breaks = i == Targeter);
                if (breaks)
                    return null;
            }

            switch(Target_Type)
            {
                case Combat_Target_Type.All_Enemies:
                case Combat_Target_Type.Three_Enemies:
                case Combat_Target_Type.Two_Enemies:
                case Combat_Target_Type.One_Enemy:
                    anti_integrityCount = allyCount;
                    integrityCount = enemyCount;
                    break;
                case Combat_Target_Type.All_Friendlies:
                case Combat_Target_Type.One_Ally:
                case Combat_Target_Type.Two_Allies:
                case Combat_Target_Type.Three_Allies:
                case Combat_Target_Type.One_Friendly:
                case Combat_Target_Type.Two_Friendlies:
                case Combat_Target_Type.Three_Friendlies:
                    anti_integrityCount = enemyCount;
                    integrityCount = allyCount;
                    break;
                case Combat_Target_Type.Everything:
                    return allyCount + enemyCount == TARGET_POOL.Length;
            }

            if (anti_integrityCount != 0)
                return null;

            if (isStrict)
                return integrityCount == requiredCount;
            return integrityCount > 0;
        }

        public GameEntity_ID[] Get_Targets()
        {
            List<GameEntity_ID> targets = new List<GameEntity_ID>();
            for (int i = 0; i < TARGET_POOL.Length; i++)
                if (TARGET_POOL[i] > 0 && !targets.Contains(targets[i]))
                    targets.Add(TARGET_POOL[i]);

            return targets.ToArray();
        }

        internal void Set_Target_Criteria(GameEntity_ID targeter, Combat_Target_Type targetType, bool targetsAreStrict = false)
        {
            Targeter = targeter;
            Target_Type = targetType;
            Has_Strict_Targets = targetsAreStrict;

            Any_Target_Index = 0;
            Enemy_Target_Index = MD_PARTY.MAX_PARTY_SIZE;
            Ally_Target_Index = 0;

            switch(Target_Type)
            {
                case Combat_Target_Type.Everything:
                case Combat_Target_Type.All_Enemies:
                case Combat_Target_Type.All_Friendlies:
                    Add_Target(GameEntity_ID.ID_NULL);
                    break;
            }

            for (int i = 0; i < TARGET_POOL.Length; i++)
                TARGET_POOL[i] = GameEntity_ID.ID_NULL;
        }

        public void Add_Target(GameEntity_ID entityId)
        {
            switch(Target_Type)
            {
                case Combat_Target_Type.Self_Or_No_Target:
                default:
                    Flag_Target(Targeter);
                    break;
                case Combat_Target_Type.Three_Allies:
                case Combat_Target_Type.Two_Allies:
                case Combat_Target_Type.One_Ally:
                    if (entityId == Targeter)
                        return;
                    Add_Ally_Target(entityId, (int)Target_Type);
                    break;
                case Combat_Target_Type.Three_Enemies:
                case Combat_Target_Type.Two_Enemies:
                case Combat_Target_Type.One_Enemy:
                    Add_Enemy_Target(entityId, (int)Target_Type);
                    break;
                case Combat_Target_Type.Three_Friendlies:
                case Combat_Target_Type.Two_Friendlies:
                case Combat_Target_Type.One_Friendly:
                    Add_Ally_Target(entityId, (((int)Target_Type) % MD_PARTY.MAX_PARTY_SIZE) + 1);
                    break;
                case Combat_Target_Type.All_Enemies:
                    Flag_Enemies();
                    break;
                case Combat_Target_Type.All_Friendlies:
                    Flag_Allies();
                    break;
                case Combat_Target_Type.Everything:
                    Flag_Enemies();
                    Flag_Allies();
                    break;
            }
        }

        private void Add_Ally_Target(GameEntity_ID entityId, int limit)
        {
            TARGET_POOL[Ally_Target_Index] = entityId;
            Ally_Target_Index = Increment_Target_Count(Ally_Target_Index, 0, limit);
        }

        private void Add_Enemy_Target(GameEntity_ID entityId, int limit)
        {
            TARGET_POOL[Enemy_Target_Index] = entityId;
            Enemy_Target_Index = Increment_Target_Count(Enemy_Target_Index, MD_PARTY.MAX_PARTY_SIZE, limit);
        }

        private int Increment_Target_Count(int count, int offset, int limit)
            => ((count + 1) % limit) + offset;

        private void Of_All(bool allySideOrNot, Action<GameEntity_ID> operation)
        {
            int index = (allySideOrNot) ? 0 : MD_PARTY.MAX_PARTY_SIZE;

            for (int i = index; i < index + MD_PARTY.MAX_PARTY_SIZE; i++)
                operation(GameEntity_ID.IDS[i]);
        }

        private void Of_All_Set(bool allySideOrNot, Action<int, bool> isSetHandler)
        {
            Of_All(allySideOrNot, (i) => isSetHandler(i, TARGET_POOL[i] > 0));
        }

        private int Of_All_Set_Count(bool allySideOrNot)
        {
            int instanceCount = 0;
            Of_All_Set(allySideOrNot, (i, b) => instanceCount += (b) ? 1 : 0);
            return instanceCount;
        }

        private bool Matches_Count(int count, int requiredCount)
        {
            if (requiredCount < 0)
                return count > 0;
            return count == requiredCount;
        }

        private void Flag_Allies()
            => Of_All(true, Flag_Target);

        private void Flag_Enemies()
            => Of_All(false, Unflag_Target);
        
        private void Unflag_Allies()
            => Of_All(true, Flag_Target);
        
        private void Unflag_Enemies()
            => Of_All(false, Unflag_Target);

        private void Flag_Target(GameEntity_ID id)
            => TARGET_POOL[id] = id;

        private void Unflag_Target(GameEntity_ID id)
            => TARGET_POOL[id] = GameEntity_ID.ID_NULL;

        public override string ToString()
        {
            return string.Format("Target_Ids: [{0}]", string.Join< GameEntity_ID>(", ", Get_Targets()));
        }
    }
}
