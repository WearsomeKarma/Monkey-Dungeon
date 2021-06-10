using System.Collections.Generic;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Field<T>
    {
        protected readonly Dictionary<GameEntity_Position, T> FIELD;
        
        public T Get_Entry_From_Position(GameEntity_Position position)
        {
            //constraint null
            if (position == null || position == GameEntity_Position.__NULL____POSITION)
                return Get_Default_Return();

            return FIELD[position];
        }

        protected GameEntity_Field(T defaultValue)
        {
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
        
        protected virtual T Get_Default_Return()
            => default(T);
    }
}