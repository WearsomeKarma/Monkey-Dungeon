using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using OpenTK;

namespace MonkeyDungeon_UI.Prefabs
{
    public class GameEntity_WorldLayer_Roster
    {
        private World_Layer World_Layer { get; set; }

        private readonly GameEntity_Position_Vector_Survey VECTOR_SURVEY;
        
        private readonly GameEntity_ClientSide_Roster GAME_ENTITY_ROSTER = new GameEntity_ClientSide_Roster();
        private readonly UI_EntityObject_Roster UI_ENTITY_OBJECT_ROSTER = new UI_EntityObject_Roster();

        public GameEntity_ClientSide Get_GameEntity(GameEntity_Position position)
            => GAME_ENTITY_ROSTER.Get_Entity(position);
        public GameEntity_ClientSide Get_GameEntity(GameEntity_ID id)
            => GAME_ENTITY_ROSTER.Get_Entity(id);
        
        public UI_EntityObject Get_UI_EntityObject(GameEntity_Position position)
            => UI_ENTITY_OBJECT_ROSTER.Get_Entity(position);

        public UI_EntityObject Get_UI_EntityObject(GameEntity_ID id)
            => UI_ENTITY_OBJECT_ROSTER.Get_Entity(Get_GameEntity(id).GameEntity_Position);
        
        public void Swap_Positions(GameEntity_Position position, GameEntity_Position_Swap_Type swapType)
        {
            GAME_ENTITY_ROSTER.Swap_Positions(position, swapType);
            UI_ENTITY_OBJECT_ROSTER.Swap_UI_Elements(position, swapType);
        }

        internal void Set_Entity(GameEntity_Position position, GameEntity_Attribute_Name_Race race, bool isDismissed = false)
        {
            GameEntity_ClientSide entity = new GameEntity_ClientSide(race, position, isDismissed);
            GAME_ENTITY_ROSTER.Set_Entity(entity);
            UI_ENTITY_OBJECT_ROSTER.Get_Entity(position).Bind_To_Description(entity);
        }

        internal void Bind_To_Description(GameEntity_ID id, GameEntity_Attribute_Name_Race race,
            bool isDismissed = false)
        {
            GameEntity_Position position = GAME_ENTITY_ROSTER.Get_Position_From_Id(id);
            
            Set_Entity(position, race, isDismissed);
        }
        
        internal GameEntity_WorldLayer_Roster(World_Layer worldLayer, Vector3[] vectorSpace)
        {
            VECTOR_SURVEY = new GameEntity_Position_Vector_Survey(vectorSpace);
            
            World_Layer = worldLayer;
            
            GameEntity_Position.For_Each_Position(GameEntity_Team_ID.ID_NULL, Fill_UI_Position);
        }

        private void Fill_UI_Position(GameEntity_Position position)
            => UI_ENTITY_OBJECT_ROSTER.Set_Entity(position, new UI_EntityObject(World_Layer, VECTOR_SURVEY.Map(position)));
    }
}