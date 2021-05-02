using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using MonkeyDungeon.Scenes.GameScenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class GameState_Machine
    {
        public static readonly int MAX_TEAM_SIZE = 4;

        private readonly List<GameState> gameStates = new List<GameState>();
        private void AddGameState(GameState gameState) { gameState.SetGameWorld(this); gameStates.Add(gameState); }

        public readonly GameScene GameScene;
        public int Level { get; set; }

        public readonly GameEntity_Factory GameEntity_Factory;

        public GameEntity_Roster PlayerRoster { get; internal set; }
        public GameEntity_Roster EnemyRoster { get; internal set; }
        internal void SetEnemyRoster(GameEntity[] enemyRoster) => EnemyRoster = new GameEntity_Roster(enemyRoster);

        public GameState CurrentGameState { get; private set; }
        public GameState RequestedGameState { get; private set; }

        public bool IsCombatHappening => CurrentGameState != null && CurrentGameState is Combat_GameState;

        public GameState_Machine(GameScene gameScene, GameState[] gameStates)
        {
            foreach (GameState gameState in gameStates)
                AddGameState(gameState);

            GameEntity_Factory = new GameEntity_Factory(this);

            PlayerRoster = new GameEntity_Roster(new GameEntity[] { });
            EnemyRoster = new GameEntity_Roster(new GameEntity[] { });
            GameScene = gameScene;
            CurrentGameState = gameStates[0];
        }
        
        internal void Establish_PlayerRoster(params string[] factoryTagNames)
        {
            GameEntity[] players = new GameEntity[factoryTagNames.Length];

            for (int i = 0; i < factoryTagNames.Length; i++)
                players[i] = GameEntity_Factory.Create_NewEntity(factoryTagNames[i]);

            PlayerRoster = new GameEntity_Roster(
                players
                );
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
