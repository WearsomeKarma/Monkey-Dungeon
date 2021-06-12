using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Vanilla_Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_ServerSide_Roster : GameEntity_Field<GameEntity_ServerSide>
    {
        internal GameEntity_ServerSide[] Get_Entities()
            => Get_Reduced_Field();
        
        internal GameEntity_ServerSide[] Get_Entities(GameEntity_Position[] positions)
        {
            List<GameEntity_ServerSide> entities = new List<GameEntity_ServerSide>();

            foreach(GameEntity_ServerSide entity in entities)
                if (positions.Contains(entity.GameEntity_Position))
                    entities.Add(entity);
            
            return entities.ToArray();
        }

        private bool Check_For_Team_Qualification(GameEntity_ServerSide entity, GameEntity_Team_ID teamID, bool checkForConciousness)
        {
            return (
                    entity.GameEntity_Team_ID == teamID
                    )
                &&
                (
                    !checkForConciousness
                    ||
                    !entity.IsIncapacitated
                )
                ;
        }
        
        internal GameEntity_ServerSide[] Get_Entities(GameEntity_Team_ID teamID, bool checkForConciousness=false)
        {
            GameEntity_ServerSide[] entities = Get_Reduced_Field();

            List<GameEntity_ServerSide> ofTeam = new List<GameEntity_ServerSide>();
            
            foreach(GameEntity_ServerSide entity in entities)
                if (Check_For_Team_Qualification(entity, teamID, checkForConciousness))
                    ofTeam.Add(entity);

            return ofTeam.ToArray();
        }
        
        public GameEntity_Attribute_Name_Race[] Get_Races()
        {
            List<GameEntity_Attribute_Name_Race> races = new List<GameEntity_Attribute_Name_Race>();
            
            foreach(GameEntity_ServerSide entity in Get_Reduced_Field())
                races.Add(entity.GameEntity_Race);

            return races.ToArray();
        }

        internal void Set_Player_As_Ready(GameEntity_ID id, bool state)
        {
            Get_Entity(id).Set_Ready_State(state);
        }
        
        public bool Check_If_Players_Are_Ready()
        {
            bool ret = true;

            foreach (GameEntity_ServerSide entity in Get_Reduced_Field())
            {
                Console.WriteLine("{0} -- isReady: {1}", entity, entity.IsReady);
                if (entity.GameEntity_Team_ID == GameEntity_Team_ID.TEAM_ONE_ID)
                    ret = entity.IsReady;
                if (!ret)
                    return ret;
            }
            
            return ret;
        }
    }
}
