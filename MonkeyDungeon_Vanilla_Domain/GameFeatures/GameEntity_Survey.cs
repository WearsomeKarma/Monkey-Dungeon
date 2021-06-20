using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Survey<T>
    {
        protected readonly Func<T> DEFAULT_QUANTITIZER;
        
        protected readonly Dictionary<GameEntity_Position, T> FIELD;
        
        protected T Get__Entry_From_Position__Survey(GameEntity_Position position)
        {
            //constraint null
            if (GameEntity_Position.Validate(position))
                return FIELD[position];

            return default(T);
        }

        protected T[] Get__Entries_From_Positions__Survey(GameEntity_Position[] positions)
        {
            T[] reducedField = new T[positions.Length];

            for (int i = 0; i < positions.Length; i++)
                reducedField[i] = FIELD[positions[i]];

            return reducedField;
        }

        protected GameEntity_Position[] Get__Reduced_Positions_Of_Hostiles__Survey(GameEntity_Team_ID friendlyTeamId)
        {
            List<GameEntity_Position> reducedPositions = new List<GameEntity_Position>();
            
            GameEntity_Position.For_Each__Hostile_Position(friendlyTeamId, (p) =>
            {
                if (!Check_If__Equivalent_To_Default__Survey(FIELD[p]))
                    reducedPositions.Add(p);
            });

            return reducedPositions.ToArray();
        }

        protected GameEntity_Position[] Get__Reduced_Positions__Survey(GameEntity_Team_ID teamId = null)
        {
            teamId = teamId ?? GameEntity_Team_ID.ID_NULL;

            List<GameEntity_Position> reducedPositions = new List<GameEntity_Position>();
            
            GameEntity_Position.For_Each__Position(teamId, (p) =>
            {
                if (!Check_If__Equivalent_To_Default__Survey(FIELD[p]))
                    reducedPositions.Add(p);
            });

            return reducedPositions.ToArray();
        }

        protected T[] Get__Reduced_Field__Survey(GameEntity_Team_ID teamId = null)
            => Get__Entries_From_Positions__Survey(Get__Reduced_Positions__Survey(teamId));

        protected T[] Get__Reduced_Field_Of_Hostiles__Survey(GameEntity_Team_ID friendlyTeamId)
            => Get__Entries_From_Positions__Survey(Get__Reduced_Positions_Of_Hostiles__Survey(friendlyTeamId));
        
        protected void Set__Entry_By_Position__Survey(GameEntity_Position position, T value)
        {
            if (GameEntity_Position.Validate(position))
                FIELD[position] = value;
        }

        protected void Swap__Entries__Survey(GameEntity_Position position, GameEntity_Position_Swap_Type swapType)
        {
            GameEntity_Position positionSwap = position.Get_Swap(swapType);
            
            T entryOne = FIELD[position];
            T entryTwo = FIELD[positionSwap];

            FIELD[positionSwap] = entryOne;
            FIELD[position] = entryTwo;
        }

        public void Reset()
            => Handle__Reset__Survey();

        protected virtual void Handle__Reset__Survey()
        {
            GameEntity_Position.For_Each__Position(GameEntity_Team_ID.ID_NULL, (p) =>
            {
                FIELD[p] = DEFAULT_QUANTITIZER();
            });
        }

        protected virtual bool Check_If__Equivalent_To_Default__Survey(T value)
        {
            return value?.Equals(DEFAULT_QUANTITIZER()) ?? false;
        }
        
        protected GameEntity_Survey(Func<T> defaultQuantitizer)
        {
            DEFAULT_QUANTITIZER = defaultQuantitizer;
            
            FIELD = new Dictionary<GameEntity_Position, T>()
            {
                { GameEntity_Position.TEAM_ONE__FRONT_RIGHT, defaultQuantitizer() },
                { GameEntity_Position.TEAM_ONE__FRONT_LEFT, defaultQuantitizer() },
                { GameEntity_Position.TEAM_ONE__REAR_RIGHT, defaultQuantitizer() },
                { GameEntity_Position.TEAM_ONE__REAR_LEFT, defaultQuantitizer() },
                
                { GameEntity_Position.TEAM_TWO__FRONT_RIGHT, defaultQuantitizer() },
                { GameEntity_Position.TEAM_TWO__FRONT_LEFT, defaultQuantitizer() },
                { GameEntity_Position.TEAM_TWO__REAR_RIGHT, defaultQuantitizer() },
                { GameEntity_Position.TEAM_TWO__REAR_LEFT, defaultQuantitizer() }
            };
        }
        
        public T this[GameEntity_Position position]
        {
            get => Get__Entry_From_Position__Survey(position);
        }
    }
}