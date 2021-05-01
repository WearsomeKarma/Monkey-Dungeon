using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using MonkeyDungeon.Scenes.GameScenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class GameWorld_StateMachine
    {
        public static readonly int MAX_TEAM_SIZE = 4;

        private List<GameState> gameStates = new List<GameState>();
        private void AddGameState(GameState gameState) { gameState.SetGameWorld(this); gameStates.Add(gameState); }

        public GameScene GameScene { get; private set; }
        public int Level { get; set; }
        
        public EntityRoster PlayerRoster { get; internal set; }
        public EntityRoster EnemyRoster { get; internal set; }
        internal void SetEnemyRoster(EntityComponent[] enemyRoster) => EnemyRoster = new EntityRoster(enemyRoster);

        public GameState CurrentGameState { get; private set; }
        public GameState RequestedGameState { get; private set; }

        public bool IsCombatHappening => CurrentGameState != null && CurrentGameState is Combat_GameState;

        public GameWorld_StateMachine(GameScene gameScene, GameState[] gameStates)
        {
            foreach (GameState gameState in gameStates)
                AddGameState(gameState);
            PlayerRoster = new EntityRoster(new EntityComponent[] { });
            EnemyRoster = new EntityRoster(new EntityComponent[] { });
            GameScene = gameScene;
            CurrentGameState = gameStates[0];
        }
        
        public void Request_Transition_ToState<T>() where T : GameState
        {
            RequestedGameState = null;
            foreach (GameState gameState in gameStates)
            {
                if (gameState is T)
                {
                    RequestedGameState = gameState;
                    break;
                }
            }
            if (CurrentGameState == null)
            {
                CurrentGameState = RequestedGameState;
                return;
            }
            CurrentGameState.End(this);
        }

        internal void CheckFor_GameState_Transition()
        {
            if (CurrentGameState.TransitionState == TransitionState.Finished)
            {
                CurrentGameState.Reset(this);
                CurrentGameState = RequestedGameState;
            }

            if (CurrentGameState.TransitionState == TransitionState.Awaiting)
            {
                CurrentGameState.Begin(this);
            }

            CurrentGameState.UpdateState(this);
        }
    }
}
