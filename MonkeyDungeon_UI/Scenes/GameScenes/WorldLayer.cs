using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Multiplayer.Handlers;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_UI.Prefabs.UI.EntityData;
using MonkeyDungeon_UI.UI_Events.Implemented;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Scenes.GameScenes
{
    public class World_Layer : GameScene_Layer
    {
        internal UI_MeleeEvent UI_MeleeEvent { get; private set; }
        internal UI_Ranged_Attack UI_Ranged_Particle_Event { get; private set; }

        private readonly string DEFAULT_RACE;

        private GameScene GameScene { get; set; }

        private readonly CreatureGameObject[] Player_LayerObjects = new CreatureGameObject[MD_PARTY.MAX_PARTY_SIZE];
        private readonly CreatureGameObject[] Enemy_LayerObjects = new CreatureGameObject[MD_PARTY.MAX_PARTY_SIZE];
        internal bool CheckIf_TargetId_IsEnemy(int id) => id >= MD_PARTY.MAX_PARTY_SIZE;
        internal int Get_IndexFrom_TargetId(int id) => id % MD_PARTY.MAX_PARTY_SIZE;
        private CreatureGameObject Get_Entity_From_Id(int id)
        {
            bool isEnemy = CheckIf_TargetId_IsEnemy(id);
            return isEnemy
                ? Enemy_LayerObjects[Get_IndexFrom_TargetId(id)]
                : Player_LayerObjects[Get_IndexFrom_TargetId(id)];
        }
        public Vector3 Get_Position_From_Id(int id)
            => Get_Entity_From_Id(id).Position;
        public UI_GameEntity_Descriptor Get_Description_From_Id(int id)
            => Get_Entity_From_Id(id).EntityDescription;
        internal void Set_Descriptions(int isPlayerDescriptions, string[] descriptions)
        {
            CreatureGameObject[] creatures = (isPlayerDescriptions == 0) ? Player_LayerObjects : Enemy_LayerObjects;

            for(int i=0;i<descriptions.Length && i < creatures.Length;i++)
            {
                creatures[i].EntityDescription = new UI_GameEntity_Descriptor(descriptions[i]);
            }
        }
        public void Set_Unique_ID(int id, uint uid)
        {
            Get_Entity_From_Id(id).Set_Unique_ID(uid);
        }

        internal Particle Ranged_Particle { get; set; }
        internal DungeonBridge DungeonBridge { get; set; }

        internal bool IsTraveling { get; set; }

        internal World_Layer(GameScene parentScene)
            : base(parentScene, WORLD_LAYER_INDEX)
        {
            DEFAULT_RACE = parentScene.MonkeyDungeon_Game_UI.DEFAULT_RACE;

            GameScene = parentScene;
            
            Vector3[] positionVectors = UI_Combat_Layer.Get_UI_TargetPositions(Game);
            
            Vector3[] positions = UI_Combat_Layer.Get_UI_TargetPositions(Game);

            Add_StaticObject(
                DungeonBridge = new DungeonBridge(
                    this, 
                    new Vector3(50,-50,0),
                    Game.Width, 
                    Game.SpriteLibrary.ExtractRenderUnit("BridgePath"))
                );

            for(int i=0;i< MD_PARTY.MAX_PARTY_SIZE;i++)
                Add_StaticObject(Player_LayerObjects[i] = new CreatureGameObject(this, -positions[i], new UI_GameEntity_Descriptor(MD_VANILLA_RACES.PLAYER_RACE)));
            
            for(int i=0;i< MD_PARTY.MAX_PARTY_SIZE; i++)
                Add_StaticObject(Enemy_LayerObjects[i] = new CreatureGameObject(this, positions[i], new UI_GameEntity_Descriptor(MD_VANILLA_RACES.PLAYER_RACE, true)));

            Add_StaticObject(Ranged_Particle = new Particle(this, new Vector3(0, 0, 0)));
            Ranged_Particle.Toggle_Sprite(false);
            Ranged_Particle.Set_Particle(MD_VANILLA_PARTICLES.CHAOS_BOLT);

            UI_MeleeEvent = new UI_MeleeEvent(
                EventScheduler,
                1, 
                new Vector3(-20, -20, 0), 
                new Vector3(20, 20, 0),
                Player_LayerObjects,
                Enemy_LayerObjects
                );

            UI_Ranged_Particle_Event = new UI_Ranged_Attack(
                EventScheduler,
                Ranged_Particle,
                Ranged_Particle.Position
                );

            GameScene.MonkeyDungeon_Game_UI.Expectation_Context.Register_Handler(
                new MMH_Invoke_UI_Event(this),
                new MMH_Set_Party_UI_Descriptions(this),
                new MMH_Accept_Client(GameScene.MonkeyDungeon_Game_UI),
                new MMH_Set_MD_VANILLA_RESOURCES(this),
                new MMH_Introduce_Entity(this),
                new MMH_Dismiss_Entity(this),
                new MMH_Entity_Death(this),
                new MMH_Update_Entity_Level(this),
                new MMH_Update_Entity_Resource(this),
                new MMH_Update_Entity_Abilities(this),
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
                DungeonBridge.Scroll_Bridge(-200, (float)e.DeltaTime);
        }
    }
}
