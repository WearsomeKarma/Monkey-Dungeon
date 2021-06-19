using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Multiplayer.Handlers;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_UI.UI_Events.Implemented;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_UI.Prefabs;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_UI.Scenes.GameScenes
{
    public class World_Layer : GameScene_Layer
    {
        internal UI_MeleeEvent UI_MeleeEvent { get; private set; }
        internal UI_Ranged_Attack UI_Ranged_Particle_Event { get; private set; }

        private readonly GameEntity_Attribute_Name_Race DEFAULT_RACE;

        private GameScene GameScene { get; set; }

        private readonly GameEntity_WorldLayer_Roster WorldLayer_Roster;

        public GameEntity_ClientSide Get_GameEntity(GameEntity_Position position)
            => WorldLayer_Roster.Get_GameEntity(position);
        public GameEntity_ClientSide Get_GameEntity(GameEntity_ID id)
            => WorldLayer_Roster.Get_GameEntity(id);
        
        public UI_EntityObject Get_UI_EntityObject(GameEntity_Position position)
            => WorldLayer_Roster.Get_UI_EntityObject(position);

        public UI_EntityObject Get_UI_EntityObject(GameEntity_ID id)
            => WorldLayer_Roster.Get_UI_EntityObject(id);
        
        public Vector3 Get_Position_From_Id(GameEntity_ID id)
            => Get_UI_EntityObject(id).Position;
        internal void Set_Description(GameEntity_ID id, GameEntity_Attribute_Name_Race @class)
        {
            WorldLayer_Roster.Bind_To_Description(id, @class);
        }
        public void Set_Unique_ID(GameEntity_ID id, uint uid)
        {
            Get_GameEntity(id).Set_Cosmetic_Id(uid);
        }

        internal UI_ParticleObject Ranged_UiParticleObject { get; set; }
        internal UI_DungeonBridge UiDungeonBridge { get; set; }

        internal bool IsTraveling { get; set; }

        internal World_Layer(GameScene parentScene)
            : base(parentScene, WORLD_LAYER_INDEX)
        {
            DEFAULT_RACE = parentScene.MonkeyDungeon_Game_UI.DEFAULT_RACE;

            GameScene = parentScene;

            WorldLayer_Roster = new GameEntity_WorldLayer_Roster(this, UI_Combat_Layer.Get_UI_TargetPositions(Game));

            Add_StaticObject(
                UiDungeonBridge = new UI_DungeonBridge(
                    this, 
                    new Vector3(50,-50,0),
                    Game.Width, 
                    Game.SpriteLibrary.ExtractRenderUnit("BridgePath"))
                );

            GameEntity_Position.For_Each__Position(GameEntity_Team_ID.ID_NULL,
                (p) =>
                {
                    WorldLayer_Roster.Set_Entity(p, MD_VANILLA_RACE_NAMES.RACE_MONKEY, p.TeamId == GameEntity_Team_ID.TEAM_ONE_ID);
                }
            );
            
            GameEntity_Position.For_Each__Position
                (
                GameEntity_Team_ID.TEAM_TWO_ID,
                (p) => WorldLayer_Roster.Get_GameEntity(p).Set_Dismissed_Status(true)
                );
            
            Add_StaticObject(WorldLayer_Roster);
            
            Add_StaticObject(Ranged_UiParticleObject = new UI_ParticleObject(this, new Vector3(0, 0, 0)));
            Ranged_UiParticleObject.Toggle_Sprite(false);
            Ranged_UiParticleObject.Set_Particle(MD_VANILLA_PARTICLE_NAMES.CHAOS_BOLT);

            UI_MeleeEvent = new UI_MeleeEvent(
                EventScheduler,
                1, 
                new Vector3(-20, -20, 0), 
                new Vector3(20, 20, 0),
                WorldLayer_Roster
                );

            UI_Ranged_Particle_Event = new UI_Ranged_Attack(
                EventScheduler,
                Ranged_UiParticleObject,
                Ranged_UiParticleObject.Position
                );

            Add_Handler_Expectation(
                new MMH_Invoke_UI_Event(this),
                new MMH_Declare_Entity_Description(this),
                new MMH_Accept_Client(GameScene.MonkeyDungeon_Game_UI),
                new MMH_Declare_Entity_Resource(this),
                new MMH_Introduce_Entity(this),
                new MMH_Dismiss_Entity(this),
                new MMH_Entity_Death(this),
                new MMH_Update_Entity_Level(this),
                new MMH_Update_Entity_Resource(this),
                new MMH_Update_Entity_Ability(this),
                new MMH_Update_Entity_Ability_Target_Type(this),
                new MMH_Update_Entity_UniqueID(this),
                new MMH_Update_Ability_Point(this),
                new MMH_Set_Melee_Combattants(this),
                new MMH_Set_Ranged_Particle(this),
                new MMH_Set_Traveling_State(this)
                );
        }

        protected override void Handle_UpdateLayer(FrameArgument e)
        {
            base.Handle_UpdateLayer(e);
            if (IsTraveling)
                UiDungeonBridge.Scroll_Bridge(-200, (float)e.DeltaTime);
        }
    }
}
