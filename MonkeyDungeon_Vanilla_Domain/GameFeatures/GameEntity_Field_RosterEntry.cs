using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Field_RosterEntry : GameEntity_Field<GameEntity_RosterEntry>
    {
        internal GameEntity_RosterEntry Get_Entity(GameEntity_ID id)
        {
            
        }

        internal GameEntity_RosterEntry Get_Entity(GameEntity_Position position)
        {
            
        }

        internal void Set_Enemies(GameEntity[] enemyTeam)
            => ENEMIES.Set_Entities(enemyTeam);
        
        internal void Set_Players(GameEntity[] playerTeam)
            => PLAYERS.Set_Entities(playerTeam);

        public GameEntity_Field_RosterEntry(GameState_Machine gameStateMachine)
        {
            PLAYERS = new GameEntity_Roster(gameStateMachine, new GameEntity[MD_PARTY.MAX_PARTY_SIZE]);
            ENEMIES = new GameEntity_Roster(gameStateMachine, new GameEntity[MD_PARTY.MAX_PARTY_SIZE]);
        }

        public GameEntity_ID[] Get_Entity_Ids(bool isPlayers)
        {
            GameEntity_RosterEntry[] rosterEntries = (isPlayers) ? PLAYERS.Get_Roster_Entries() : ENEMIES.Get_Roster_Entries();

            List<GameEntity_ID> ids = new List<GameEntity_ID>();
            for (int i = 0; i < MD_PARTY.MAX_PARTY_SIZE; i++)
                ids.Add(rosterEntries[i].Entity.GameEntity_ID);

            return ids.ToArray();
        }

        public double Get_Resource_Value(GameEntity_Attribute_Name resourceName, GameEntity_ID id)
        {
            GameEntity_RosterEntry rosterEntry = Get_Entity(id);

            //TODO: reduce
            return rosterEntry.Entity.Resource_Manager.Get_Resource(resourceName);
        }
    }
}
