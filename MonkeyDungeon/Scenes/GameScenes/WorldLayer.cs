using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.Implemented.ActingEntities;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
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
        internal GameWorld_StateMachine GameWorld { get; set; }

        private readonly Player[] Player_LayerObjects = new Player[GameWorld_StateMachine.MAX_PLAYER_COUNT];
        private readonly CreatureGameObject[] Enemy_LayerObjects = new CreatureGameObject[GameWorld_StateMachine.MAX_PLAYER_COUNT];

        private Combat_GameState Combat { get; set; }

        public World_Layer(GameScene parentScene, Combat_GameState combat)
            : base(parentScene)
        {
            Combat = combat;
            Combat.StateBegun += BeginCombat;
            Vector3[] positionVectors = UI_Combat_Layer.Get_UI_TargetPositions(Game);

            GameWorld = parentScene.GameWorld;

            Vector3[] positions = UI_Combat_Layer.Get_UI_TargetPositions(Game);

            for(int i=0;i<GameWorld_StateMachine.MAX_PLAYER_COUNT;i++)
                Add_StaticObject(Player_LayerObjects[i] = new Player(this, -positions[i], new EntityComponent()));
            
            for(int i=0;i<GameWorld_StateMachine.MAX_PLAYER_COUNT; i++)
                Add_StaticObject(Enemy_LayerObjects[i] = new CreatureGameObject(this, positions[i], new EntityComponent()));
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
