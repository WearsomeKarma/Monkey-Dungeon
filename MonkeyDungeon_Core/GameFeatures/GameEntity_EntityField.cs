using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_EntityField
    {
        internal readonly GameEntity_Roster PLAYERS;
        internal readonly GameEntity_Roster ENEMIES;

        private GameEntity Get_Entity(GameEntity_ID id)
        {
            bool isPlayers = id < MD_PARTY.MAX_PARTY_SIZE;

            if (isPlayers)
                return PLAYERS.Entities[id];
            return ENEMIES.Entities[id % MD_PARTY.MAX_PARTY_SIZE];
        }

        internal void Set_Enemies(GameEntity_Roster enemyTeam)
            => ENEMIES.Set_Entities(enemyTeam.Entities);
        
        internal void Set_Players(GameEntity_Roster playerTeam)
            => PLAYERS.Set_Entities(playerTeam.Entities);

        public GameEntity_EntityField(GameState_Machine gameStateMachine)
        {
            PLAYERS = new GameEntity_Roster(gameStateMachine, new GameEntity[MD_PARTY.MAX_PARTY_SIZE]);
            ENEMIES = new GameEntity_Roster(gameStateMachine, new GameEntity[MD_PARTY.MAX_PARTY_SIZE]);
        }

        public int[] Get_Entity_Ids(bool isPlayers)
        {
            GameEntity[] Entities = (isPlayers) ? PLAYERS.Entities : ENEMIES.Entities;

            List<int> ids = new List<int>();
            for (int i = 0; i < PLAYERS.EntityCount; i++)
                ids.Add(Entities[i].Scene_GameObject_ID);

            return ids.ToArray();
        }

        public double Get_Resource_Value(GameEntity_Attribute_Name resourceName, GameEntity_ID id)
        {
            GameEntity entity = Get_Entity(id);

            return entity.Resource_Manager.Get_Resource(resourceName);
        }
    }
}
