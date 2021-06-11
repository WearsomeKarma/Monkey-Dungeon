using System.Collections.Generic;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Field<T> : GameEntity_Survey<T> where T : GameEntity
    {
        public T Get_Entity(GameEntity_ID id)
        {
            if (!GameEntity_ID.Validate(id))
                return DEFAULT_VALUE;
            
            T entity;

            foreach (GameEntity_Position position in FIELD.Keys)
            {
                entity = FIELD[position];
                if (GameEntity.Validate(entity))
                    return entity;
            }
            
            return DEFAULT_VALUE;
        }

        public T Get_Entity(GameEntity_Position position)
            => Get_Entry_From_Position(position);

        public GameEntity_Position Get_Position_From_Id(GameEntity_ID id)
            => Get_Entity(id).GameEntity_Position;
        
        public GameEntity_ID[] Get_Entity_Ids(bool isPlayers)
        {
            GameEntity[] rosterEntries = Get_Reduced_Field();

            List<GameEntity_ID> ids = new List<GameEntity_ID>();
            for (int i = 0; i < MD_PARTY.MAX_PARTY_SIZE; i++)
                ids.Add(rosterEntries[i].GameEntity_ID);

            return ids.ToArray();
        }

        public void Set_Entity(T entity)
        {
            if (GameEntity.Validate(entity))
            {
                FIELD[entity.GameEntity_Position] = entity;
                return;
            }
        }
        
        public void Set_Entities(T[] entities)
        {
            foreach(T entity in entities)
                Set_Entity(entity);
        }
        
        public void Swap_Positions(GameEntity_Position position, GameEntity_Position_Swap_Type swapType)
        {
            Swap_Entries(position, swapType);
        }
        
        protected GameEntity_Field()
            : base(GameEntity.NULL_ENTITY as T)
        {}
    }
}
