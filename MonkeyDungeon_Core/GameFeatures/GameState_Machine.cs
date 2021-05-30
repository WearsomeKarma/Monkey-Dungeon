using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System.Collections.Generic;
using System.Linq;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameState_Machine
    {
        private readonly List<GameState> gameStates = new List<GameState>();
        private void Add_GameState(GameState gameState) { gameState.Set_GameWorld(this); gameStates.Add(gameState); }
        internal T Get_GameState<T>() where T : GameState 
            => gameStates.OfType<T>().ElementAt(0);
        
        private MonkeyDungeon_Server Server { get; set; }

        public int Level { get; set; }

        public GameEntity_Factory GameEntity_Factory { get; private set; }

        public GameEntity_Roster PlayerRoster { get; internal set; }
        public GameEntity_Roster EnemyRoster { get; internal set; }
        internal void Set_Enemy_Roster(GameEntity[] enemyRoster) => EnemyRoster.Set_Entities(enemyRoster);
        public GameEntity Get_Entity(GameEntity_ID entityId)
        {
            if (entityId < MD_PARTY.MAX_PARTY_SIZE)
                return PlayerRoster.Entities[entityId];
            return EnemyRoster.Entities[entityId % MD_PARTY.MAX_PARTY_SIZE];
        }
        public GameEntity_Controller Get_Entity_Controller(GameEntity_ID entityId)
            => Get_Entity(entityId).EntityController;
        public bool IsMatching_Relay_Id(GameEntity_ID entityId, int relayId)
            => Get_Entity(entityId).Relay_ID_Of_Owner == relayId;
        public GameEntity Set_Entity(GameEntity_ID entityId, int relayId, GameEntity_Attribute_Name factory_Tag)
        {
            GameEntity entity = GameEntity_Factory.Create_NewEntity(entityId, relayId, factory_Tag);

            if (entityId < MD_PARTY.MAX_PARTY_SIZE)
                return PlayerRoster.Set_Entity(entity);
            return EnemyRoster.Set_Entity(entity);
        }
        public void Set_Entity_Ready_State(int entityId, bool state)
            => PlayerRoster.Set_Ready_To_Start(entityId, state);

        public GameState CurrentGameState { get; private set; }
        public GameState RequestedGameState { get; private set; }

        public bool IsCombatHappening => CurrentGameState != null && CurrentGameState is Combat_GameState;
        public bool HasStarted { get; private set; }

        public GameState_Machine(MonkeyDungeon_Server server, GameState[] gameStates)
        {
            Server = server;

            foreach (GameState gameState in gameStates)
                Add_GameState(gameState);

            GameEntity_Factory = new GameEntity_Factory(this);

            PlayerRoster = new GameEntity_Roster(this, new GameEntity[MD_PARTY.MAX_PARTY_SIZE]);
            EnemyRoster = new GameEntity_Roster(this, new GameEntity[MD_PARTY.MAX_PARTY_SIZE]);
            CurrentGameState = gameStates[0];
        }
        
        public void Begin_Game()
        {
            if (HasStarted)
                return; //TODO: Warn in log.
            HasStarted = true;

            Declare_Descriptions(0, PlayerRoster.Get_Races());

            Relay_Roster(PlayerRoster);
            Dismiss_Roster(null);

            //TODO: Make a means to send message to specific client, and all clients.
            Broadcast(
                new MMW_Accept_Client()
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

        public void CheckFor_GameState_Transition(double deltaTime = 0)
        {
            if (!HasStarted)
            {
                if (PlayerRoster.CheckIf_Team_Is_Ready())
                {
                    Begin_Game();
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

            CurrentGameState.UpdateState(this, deltaTime);
        }

        internal void Register_Multiplayer_Handlers(params Multiplayer_Message_Handler[] handlers)
            => Server.Register_Multiplayer_Handlers(handlers);

        internal void Broadcast(Multiplayer_Message_Wrapper msg)
            => Server.Broadcast(msg);

        internal void Relay(int relayId, Multiplayer_Message_Wrapper msg)
            => Server.Relay(relayId, msg);

        internal void Relay_Entity_Resource(GameEntity_Resource resource)
        {
            Server.Broadcast(
                new MMW_Update_Entity_Resource(
                    resource.Internal_Parent.Scene_GameObject_ID,
                    (float)(resource.Value / resource.Max_Quantity),
                    resource.ATTRIBUTE_NAME
                    )
                );
        }

        internal void Relay_Entity(GameEntity entity)
        {
            //send abilities
            GameEntity_Attribute_Name[] abilities = entity.Ability_Manager.Get_Ability_Names();
            for (int i = 0; i < abilities.Length; i++)
            {
                Broadcast(
                    new MMW_Update_Entity_Abilities(entity.Scene_GameObject_ID, abilities[i])
                    );
            }

            //send uid
            Broadcast(
                new MMW_Update_Entity_UniqueID(entity.Scene_GameObject_ID, (uint)entity.Unique_ID)
                );

            //send resource names
            GameEntity_Attribute_Name[] resources = entity.Resource_Manager.Get_Resource_Names();
            for(int i=0;i<resources.Length;i++)
            {
                Broadcast(
                    new MMW_Declare_Entity_Resource(entity.Scene_GameObject_ID, resources[i])
                    );
            }

            //introduce the entity to the scene.
            Broadcast(
                new MMW_Introduce_Entity(entity.Scene_GameObject_ID)
                );
        }

        //TODO: fix
        internal void Declare_Descriptions(int offset, GameEntity_Attribute_Name[] descriptions)
        {
            for (int i = 0; i < descriptions.Length; i++)
            {
                Broadcast(
                    new MMW_Declare_Entity_Descriptions(GameEntity_ID.IDS[offset + i], descriptions[i])
                    );
            }
        }

        internal void Relay_Roster(GameEntity_Roster roster)
        {
            foreach (GameEntity entity in roster.Entities)
                Relay_Entity(entity);
        }

        internal void Dismiss_Roster(GameEntity_Roster roster)
        {
            if (roster == null)
            {
                for (int i = MD_PARTY.MAX_PARTY_SIZE; i < MD_PARTY.MAX_PARTY_SIZE * 2; i++)
                {
                    Broadcast(
                        new MMW_Dismiss_Entity(GameEntity_ID.IDS[i])
                        );
                }
                return;
            }

            foreach (GameEntity entity in roster.Entities)
                Relay_Dismissal(entity);
        }

        internal void Relay_Entity_Static_Resource(GameEntity_Resource resource)
        {
            Broadcast(
                new MMW_Update_Ability_Point(resource.Internal_Parent.Scene_GameObject_ID, (int)resource.Value)
                );
        }

        internal void Relay_Death(GameEntity gameEntity)
        {
            Broadcast(
                new MMW_Entity_Death(gameEntity.Scene_GameObject_ID)
                );
        }

        internal void Relay_Dismissal(GameEntity gameEntity)
        {
            Broadcast(
                new MMW_Dismiss_Entity(gameEntity.Scene_GameObject_ID)
                );
        }
    }
}
