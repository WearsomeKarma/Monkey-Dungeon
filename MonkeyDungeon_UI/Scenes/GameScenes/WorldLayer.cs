using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Multiplayer.Handlers;
using MonkeyDungeon_UI.Prefabs.Components;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_UI.UI_Events.Implemented;
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
        private UI_MeleeEvent UI_MeleeEvent;

        private readonly string DEFAULT_RACE;

        private GameScene GameScene { get; set; }

        private readonly CreatureGameObject[] Player_LayerObjects = new CreatureGameObject[MonkeyDungeon_Game_Client.MAX_TEAM_SIZE];
        private readonly CreatureGameObject[] Enemy_LayerObjects = new CreatureGameObject[MonkeyDungeon_Game_Client.MAX_TEAM_SIZE];
        internal bool CheckIf_TargetId_IsEnemy(int id) => id >= MonkeyDungeon_Game_Client.MAX_TEAM_SIZE;
        internal int Get_IndexFrom_TargetId(int id) => id % MonkeyDungeon_Game_Client.MAX_TEAM_SIZE;
        private CreatureGameObject Get_Entity_From_Id(int id)
        {
            bool isEnemy = CheckIf_TargetId_IsEnemy(id);
            return isEnemy
                ? Enemy_LayerObjects[Get_IndexFrom_TargetId(id)]
                : Player_LayerObjects[Get_IndexFrom_TargetId(id)];
        }
        public UI_GameEntity_Descriptor Get_Description_From_Id(int id)
            => Get_Entity_From_Id(id).EntityDescription;
        internal void Set_Descriptions(int isPlayerDescriptions, string[] descriptions)
        {
            CreatureGameObject[] creatures = (isPlayerDescriptions == 0) ? Player_LayerObjects : Enemy_LayerObjects;

            for(int i=0;i<descriptions.Length && i < creatures.Length;i++)
            {
                creatures[i].EntityDescription = new UI_GameEntity_Descriptor(descriptions[i], 1);
            }
        }
        public void Set_Unique_ID(int id, uint uid)
        {
            Get_Entity_From_Id(id).Set_Unique_ID(uid);
        }

        internal World_Layer(GameScene parentScene)
            : base(parentScene, WORLD_LAYER_INDEX)
        {
            DEFAULT_RACE = parentScene.MonkeyDungeon_Game_UI.DEFAULT_RACE;

            GameScene = parentScene;
            
            Vector3[] positionVectors = UI_Combat_Layer.Get_UI_TargetPositions(Game);
            
            Vector3[] positions = UI_Combat_Layer.Get_UI_TargetPositions(Game);
            
            for(int i=0;i< MonkeyDungeon_Game_Client.MAX_TEAM_SIZE;i++)
                Add_StaticObject(Player_LayerObjects[i] = new CreatureGameObject(this, -positions[i]));
            
            for(int i=0;i< MonkeyDungeon_Game_Client.MAX_TEAM_SIZE; i++)
                Add_StaticObject(Enemy_LayerObjects[i] = new CreatureGameObject(this, positions[i]));

            UI_MeleeEvent = new UI_MeleeEvent(
                EventScheduler,
                1, 
                new Vector3(-20, -20, 0), 
                new Vector3(20, 20, 0)
                );

            GameScene.MonkeyDungeon_Game_UI.Expectation_Context.Register_Handler(
                new MMH_Set_Party_UI_Descriptions(this)
                );
            GameScene.MonkeyDungeon_Game_UI.Expectation_Context.Register_Handler(
                new MMH_Accept_Client(GameScene.MonkeyDungeon_Game_UI)
                );
            GameScene.MonkeyDungeon_Game_UI.Expectation_Context.Register_Handler(
                new MMH_Update_Entity_Abilities(this)
                );
            GameScene.MonkeyDungeon_Game_UI.Expectation_Context.Register_Handler(
                new MMH_Update_Entity_UniqueID(this)
                );
        }

        internal void RemovePlayer(int playerIndex)
        {
            //throw new NotImplementedException();
            //resolve primitive obsession.
        }

        internal void BeginCombat(UI_GameEntity_Descriptor[] players, UI_GameEntity_Descriptor[] enemies)
        {
            Set_IfInbounds(Player_LayerObjects, players, Player_LayerObjects.Length, players.Length);
            
            Set_IfInbounds(Enemy_LayerObjects, enemies, Enemy_LayerObjects.Length, enemies.Length);
        }

        internal void Act_MeleeAttack(int eventOwnerId, int targetId)
        {
            CreatureGameObject owner = Get_Entity_From_Id(eventOwnerId);
            CreatureGameObject target = Get_Entity_From_Id(targetId);

            owner.Melee_MovementController.Invoke();
            target.Melee_MovementController.Invoke();
        }

        //TODO: FIX THIS
        internal void Update_Entity(UI_GameEntity_Descriptor entity)
        {
            int trueId = entity.SCENE_ID % MonkeyDungeon_Game_Client.MAX_TEAM_SIZE;
            if (entity.SCENE_ID >= MonkeyDungeon_Game_Client.MAX_TEAM_SIZE)
            {
                Enemy_LayerObjects[trueId].EntityDescription = entity;
            }
            else
            {
                Player_LayerObjects[trueId].EntityDescription = entity;
            }
        }

        private void Set_IfInbounds<T>(T[] objs, UI_GameEntity_Descriptor[] descriptions, int length, int comparingLength) where T : CreatureGameObject
        {
            for (int i = 0; i < length; i++)
            {
                if (comparingLength <= i)
                {
                    objs[i].SpriteComponent.Enabled = false;
                    continue;
                }
                objs[i].SpriteComponent.Enabled = true;
                objs[i].Set_Race(descriptions[i]);
            }
        }
    }
}
