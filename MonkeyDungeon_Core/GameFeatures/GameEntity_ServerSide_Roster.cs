using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources;
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
            => Get__Reduced_Field__Survey();
        
        internal GameEntity_ServerSide[] Get_Entities(GameEntity_Position[] positions)
        {
            List<GameEntity_ServerSide> entities = new List<GameEntity_ServerSide>();

            GameEntity_Position.For_Each__Position(
                GameEntity_Team_ID.ID_NULL,
                (p) =>
                {
                    if(positions.Contains(p) && FIELD[p] != null)
                        entities.Add(FIELD[p]);
                });
            
            return entities.ToArray();
        }

        private bool Check_For__Team_Qualification(GameEntity_ServerSide entity, GameEntity_Team_ID teamID, bool checkForConciousness)
        {
            return (
                    entity.GameEntity__Team_ID == teamID
                    )
                &&
                (
                    !checkForConciousness
                    ||
                    !entity.GameEntity__Is_Incapacitated
                )
                ;
        }
        
        internal GameEntity_ServerSide[] Get_Entities(GameEntity_Team_ID teamID, bool checkForConciousness=false)
        {
            GameEntity_ServerSide[] entities = Get__Reduced_Field__Survey(teamID);

            List<GameEntity_ServerSide> ofTeam = new List<GameEntity_ServerSide>();
            
            foreach(GameEntity_ServerSide entity in entities)
                if (Check_For__Team_Qualification(entity, teamID, checkForConciousness))
                    ofTeam.Add(entity);

            return ofTeam.ToArray();
        }
        
        public GameEntity_Attribute_Name_Race[] Get_Races(GameEntity_Team_ID teamId)
        {
            List<GameEntity_Attribute_Name_Race> races = new List<GameEntity_Attribute_Name_Race>();
            
            foreach(GameEntity_ServerSide entity in Get__Reduced_Field__Survey(teamId))
                races.Add(entity.GameEntity__Race);

            return races.ToArray();
        }

        internal void Set_Player_As_Ready(GameEntity_ID id, bool state)
        {
            Get_Entity(id).Set_Ready_State(state);
        }
        
        public bool Check_If_Players_Are_Ready()
        {
            bool ret = true;

            foreach (GameEntity_ServerSide entity in Get__Reduced_Field__Survey(GameEntity_Team_ID.TEAM_ONE_ID))
            {
                Console.WriteLine("{0} -- isReady: {1}", entity, entity.GameEntity__Is_Ready);
                if (entity.GameEntity__Team_ID == GameEntity_Team_ID.TEAM_ONE_ID)
                    ret = entity.GameEntity__Is_Ready;
                if (!ret)
                    return ret;
            }
            
            return ret;
        }
    }
}
