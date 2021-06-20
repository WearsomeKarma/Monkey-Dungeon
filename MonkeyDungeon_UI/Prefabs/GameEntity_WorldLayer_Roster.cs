using System;
using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using OpenTK;

namespace MonkeyDungeon_UI.Prefabs
{
    public class GameEntity_WorldLayer_Roster : GameObject
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
            => UI_ENTITY_OBJECT_ROSTER.Get_Entity(Get_GameEntity(id).GameEntity__Position);
        
        public void Swap_Positions(GameEntity_Position position, GameEntity_Position_Swap_Type swapType)
        {
            GAME_ENTITY_ROSTER.Swap_Positions(position, swapType);
            UI_ENTITY_OBJECT_ROSTER.Swap_UI_Elements(position, swapType);
        }

        internal void Set_Entity(GameEntity_Position position, GameEntity_Attribute_Name_Race race, bool isDismissed = false)
        {
            GameEntity_ID idAtPosition = GAME_ENTITY_ROSTER.Get_Entity(position).GameEntity__ID;
            GameEntity_ClientSide entity = new GameEntity_ClientSide(race, position, idAtPosition, isDismissed);
            GAME_ENTITY_ROSTER.Set_Entity(entity);
            entity.Bind_To__UI_EntityObject(UI_ENTITY_OBJECT_ROSTER.Get_Entity(position));
        }

        internal void Bind_To_Description(GameEntity_ID id, GameEntity_Attribute_Name_Race race,
            bool isDismissed = false)
        {
            GameEntity_Position position = GAME_ENTITY_ROSTER.Get_Position_From_Id(id, GameEntity_Position.ALL_NON_NULL__POSITIONS[id]);
            
            Set_Entity(position, race, isDismissed);
        }
        
        internal GameEntity_WorldLayer_Roster(World_Layer worldLayer, Vector3[] vectorSpace)
            : base(worldLayer, Vector3.Zero)
        {
            VECTOR_SURVEY = new GameEntity_Position_Vector_Survey(vectorSpace);
            
            World_Layer = worldLayer;
            
            GameEntity_Position.For_Each__Position(GameEntity_Team_ID.ID_NULL, Fill_Position);
        }

        public override void OnUpdate(FrameArgument args)
        {
            foreach(UI_EntityObject entity in UI_ENTITY_OBJECT_ROSTER.Get_Entities())
                entity.OnUpdate(args);
        }

        protected override void HandleDraw(RenderService renderService)
        {
            foreach(UI_EntityObject entity in UI_ENTITY_OBJECT_ROSTER.Get_Entities())
                renderService.DrawObj(entity);
        }

        private void Fill_Position(GameEntity_Position position)
        {
            UI_ENTITY_OBJECT_ROSTER.Set_Entity(position, new UI_EntityObject(World_Layer, VECTOR_SURVEY.Map(position)));
            GAME_ENTITY_ROSTER.Set_Entity(new GameEntity_ClientSide(MD_VANILLA_RACE_NAMES.RACE_MONKEY, position, GameEntity_ID.Default_ID_From_Position(position)));
        }
    }
}