using System.Collections.Generic;
using System.Diagnostics;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Survey<T>
    {
        protected readonly T DEFAULT_VALUE;
        
        protected readonly Dictionary<GameEntity_Position, T> FIELD;
        
        public T Get_Entry_From_Position(GameEntity_Position position)
        {
            //constraint null
            if (GameEntity_Position.Validate(position))
                return FIELD[position];

            return DEFAULT_VALUE;
        }

        protected T[] Get_Reduced_Field()
        {
            List<T> reducedField = new List<T>();

            foreach(GameEntity_Position position in GameEntity_Position.ALL_NON_NULL__POSITIONS)
                if (!FIELD[position].Equals(DEFAULT_VALUE))
                    reducedField.Add(FIELD[position]);
            
            return reducedField.ToArray();
        }

        protected void Set_Entry_By_Position(GameEntity_Position position, T value)
        {
            if (GameEntity_Position.Validate(position))
                FIELD[position] = value;
        }
        
        protected GameEntity_Survey(T defaultValue)
        {
            DEFAULT_VALUE = defaultValue;
            
            FIELD = new Dictionary<GameEntity_Position, T>()
            {
                { GameEntity_Position.TEAM_ONE__FRONT_RIGHT, defaultValue },
                { GameEntity_Position.TEAM_ONE__FRONT_LEFT, defaultValue },
                { GameEntity_Position.TEAM_ONE__REAR_RIGHT, defaultValue },
                { GameEntity_Position.TEAM_ONE__REAR_LEFT, defaultValue },
                
                { GameEntity_Position.TEAM_ONE__FRONT_RIGHT, defaultValue },
                { GameEntity_Position.TEAM_ONE__FRONT_LEFT, defaultValue },
                { GameEntity_Position.TEAM_ONE__REAR_RIGHT, defaultValue },
                { GameEntity_Position.TEAM_ONE__REAR_LEFT, defaultValue }
            };
        }
    }
}