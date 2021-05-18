using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameState_Machine
    {
        public static readonly int MAX_TEAM_SIZE = 4;
        
        private readonly List<GameState> gameStates = new List<GameState>();
        private void AddGameState(GameState gameState) { gameState.Set_GameWorld(this); gameStates.Add(gameState); }
        
        public MonkeyDungeon_Game_Server Server { get; private set; }

        public int Level { get; set; }

        public GameEntity_Factory GameEntity_Factory { get; private set; }

        public GameEntity_Roster PlayerRoster { get; internal set; }
        public GameEntity_Roster EnemyRoster { get; internal set; }
        internal void Set_Enemy_Roster(GameEntity[] enemyRoster) => EnemyRoster = new GameEntity_Roster(enemyRoster);
        public GameEntity Get_Entity(int entityScene_Id)
        {
            if (entityScene_Id < 0)
                return null;
            if (entityScene_Id < MAX_TEAM_SIZE)
                return PlayerRoster.Entities[entityScene_Id];
            return EnemyRoster.Entities[entityScene_Id % MAX_TEAM_SIZE];
        }
        public GameEntity Set_Entity(int entityScene_Id, string factory_Tag)
        {
            if (entityScene_Id < 0)
                return null;

            if (entityScene_Id < MAX_TEAM_SIZE)
                return (PlayerRoster.Set_Entity(entityScene_Id, GameEntity_Factory.Create_NewEntity(factory_Tag)));
            return (EnemyRoster.Set_Entity(entityScene_Id, GameEntity_Factory.Create_NewEntity(factory_Tag)));
        }

        public GameState CurrentGameState { get; private set; }
        public GameState RequestedGameState { get; private set; }

        public bool IsCombatHappening => CurrentGameState != null && CurrentGameState is Combat_GameState;
        public bool HasStarted { get; private set; }

        public GameState_Machine(MonkeyDungeon_Game_Server server, GameState[] gameStates)
        {
            Server = server;

            foreach (GameState gameState in gameStates)
                AddGameState(gameState);

            GameEntity_Factory = new GameEntity_Factory(this);

            PlayerRoster = new GameEntity_Roster(new GameEntity[MAX_TEAM_SIZE]);
            EnemyRoster = new GameEntity_Roster(new GameEntity[MAX_TEAM_SIZE]);
            CurrentGameState = gameStates[0];
        }
        
        public void Begin_Game()
        {
            if (HasStarted)
                return; //TODO: Warn in log.
            HasStarted = true;

            Server.ServerSide_Local_Reciever.Queue_Message(
                new MMW_Set_Party_UI_Descriptions(0, PlayerRoster.Get_Races())
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

        public void CheckFor_GameState_Transition()
        {
            if (!HasStarted)
            {
                if (PlayerRoster.CheckIf_Team_Is_Ready())
                {
                    Begin_Game();
                    //TODO: Make a means to send message to specific client, and all clients.
                    Server.ServerSide_Local_Reciever.Queue_Message(
                        new MMW_Accept_Client()
                        );
                }
            }

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
