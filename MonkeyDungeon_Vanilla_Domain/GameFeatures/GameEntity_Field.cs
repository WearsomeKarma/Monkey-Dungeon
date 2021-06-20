using System;
using System.Collections.Generic;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Field<T> : GameEntity_Survey<T> where T : GameEntity
    {
        public T Get_Entity(GameEntity_ID id)
        {
            if (!GameEntity_ID.Validate(id))
                return null;
            
            T entity;

            foreach (GameEntity_Position position in FIELD.Keys)
            {
                entity = FIELD[position];
                if (GameEntity.Validate(entity) && entity.GameEntity__ID == id)
                {
                    return entity;
                }
            }

            return null;
        }

        public T Get_Entity(GameEntity_Position position)
            => Get__Entry_From_Position__Survey(position);

        public GameEntity_Position Get_Position_From_Id(GameEntity_ID id, GameEntity_Position defaultPosition = null)
            => Get_Entity(id)?.GameEntity__Position ?? (defaultPosition ?? GameEntity_Position.NULL_POSITION);
        
        public GameEntity_ID[] Get_Entity_Ids(bool isPlayers)
        {
            GameEntity[] rosterEntries = Get__Reduced_Field__Survey();

            List<GameEntity_ID> ids = new List<GameEntity_ID>();
            for (int i = 0; i < MD_PARTY.MAX_PARTY_SIZE; i++)
                ids.Add(rosterEntries[i].GameEntity__ID);

            return ids.ToArray();
        }

        public void Set_Entity(T entity)
        {
            if (GameEntity.Validate(entity))
            {
                FIELD[entity.GameEntity__Position] = entity;
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
            Swap__Entries__Survey(position, swapType);
        }
        
        protected GameEntity_Field()
            : base(() => null)
        {}
    }
}
