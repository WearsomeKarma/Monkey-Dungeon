using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon.Components;
using MonkeyDungeon.Events.Implemented;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.Implemented.ActingEntities;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using MonkeyDungeon.Prefabs.Components;
using MonkeyDungeon.Prefabs.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.GameScenes
{
    public class World_Layer : SceneLayer
    {
        public static readonly string EVENT_TAG_MELEE = "event_meleeAttack";

        private GameScene GameScene { get; set; }
        internal GameWorld_StateMachine GameWorld { get; set; }

        private readonly Player[] Player_LayerObjects = new Player[GameWorld_StateMachine.MAX_TEAM_SIZE];
        private readonly CreatureGameObject[] Enemy_LayerObjects = new CreatureGameObject[GameWorld_StateMachine.MAX_TEAM_SIZE];
        internal bool CheckIf_TargetId_IsEnemy(int id) => id >= GameWorld_StateMachine.MAX_TEAM_SIZE;
        internal int Get_IndexFrom_TargetId(int id) => id % GameWorld_StateMachine.MAX_TEAM_SIZE;
        private CreatureGameObject GetEntity_From_Id(int id)
        {
            bool isEnemy = CheckIf_TargetId_IsEnemy(id);
            return isEnemy
                ? Enemy_LayerObjects[Get_IndexFrom_TargetId(id)]
                : Player_LayerObjects[Get_IndexFrom_TargetId(id)];
        }

        private Combat_GameState Combat { get; set; }

        private EventScheduler EventScheduler { get; set; }
        private MeleeEvent MeleeEvent { get; set; }

        public World_Layer(GameScene parentScene, Combat_GameState combat)
            : base(parentScene, 2)
        {
            GameScene = parentScene;
            EventScheduler = parentScene.EventScheduler;

            Combat = combat;
            Combat.StateBegun += BeginCombat;
            Vector3[] positionVectors = UI_Combat_Layer.Get_UI_TargetPositions(Game);

            GameWorld = parentScene.GameWorld;

            Vector3[] positions = UI_Combat_Layer.Get_UI_TargetPositions(Game);

            for(int i=0;i<GameWorld_StateMachine.MAX_TEAM_SIZE;i++)
                Add_StaticObject(Player_LayerObjects[i] = new Player(this, -positions[i], new EntityComponent()));
            
            for(int i=0;i<GameWorld_StateMachine.MAX_TEAM_SIZE; i++)
                Add_StaticObject(Enemy_LayerObjects[i] = new CreatureGameObject(this, positions[i], new EntityComponent()));

            EventScheduler.Register_Event(EVENT_TAG_MELEE, 
                MeleeEvent = new MeleeEvent()
                );
        }

        internal void RemovePlayer(int playerIndex)
        {
            throw new NotImplementedException();
            //resolve primitive obsession.
        }

        internal int AddPlayer(EntityComponent player_EC)
        {
            int index = GameWorld.PlayerRoster.ToggleEntity(true);
            if (index > -1)
                GameWorld.PlayerRoster.Mutate_EntityComponent(index, player_EC);
            return index;
        }

        internal void BeginCombat()
        {
            EntityComponent[] players = Combat.Players;
            Set_IfInbounds(Player_LayerObjects, players, Player_LayerObjects.Length, players.Length);

            EntityComponent[] enemies = Combat.Enemies;
            Set_IfInbounds(Enemy_LayerObjects, enemies, Enemy_LayerObjects.Length, enemies.Length);
        }

        internal void Act_MeleeAttack(int eventOwnerId, int targetId)
        {
            CreatureGameObject owner = GetEntity_From_Id(eventOwnerId);
            CreatureGameObject target = GetEntity_From_Id(targetId);

            MovementController allySide = (owner.EntityComponent.Scene_GameObject_ID >= GameWorld_StateMachine.MAX_TEAM_SIZE)
                ? target.Melee_MovementController
                : owner.Melee_MovementController
                ;

            MovementController enemySide = (owner.Melee_MovementController != allySide)
                ? owner.Melee_MovementController
                : target.Melee_MovementController
                ;

            MeleeEvent.Set_Combattants(
                allySide,
                enemySide
                );
            EventScheduler.Invoke_Event(MeleeEvent.EVENT_TAG);
        }

        private void Set_IfInbounds<T>(T[] objs, EntityComponent[] ECs, int length, int comparingLength) where T : CreatureGameObject
        {
            for (int i = 0; i < length; i++)
            {
                if (comparingLength <= i)
                {
                    objs[i].SpriteComponent.Enabled = false;
                    continue;
                }
                objs[i].SpriteComponent.Enabled = true;
                objs[i].EntityComponent = ECs[i];
            }
        }
    }
}
