using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class Game_StateMachine
    {
        private readonly List<GameState> gameStates = new List<GameState>();
        private void Add_GameState(GameState gameState) { gameState.Set_GameWorld(this); gameStates.Add(gameState); }
        internal T Get_GameState<T>() where T : GameState 
            => gameStates.OfType<T>().ElementAt(0);
        
        private MonkeyDungeon_Server Server { get; set; }

        public int Level { get; set; }

        public GameEntity_ServerSide_Factory GameEntityServerSideFactory { get; private set; }

        public readonly GameEntity_ServerSide_Roster GameField;

        public void Create__Entity(GameEntity_ID entityId, Multiplayer_Relay_ID relayId, GameEntity_Attribute_Name factory_Tag)
        {
            Server.Bind_To_Relay(entityId, relayId);
            GameEntity_ServerSide entity = GameEntityServerSideFactory.Create_NewEntity(entityId, relayId, GameEntity_Position.ALL_NON_NULL__POSITIONS[entityId],  factory_Tag);
            
            Set__Entity(entity);
        }

        private void Bind__Entity(GameEntity_ServerSide entity)
        {
            entity.Game = this;
            
            entity.Event__Resource_Updated__GameEntity += Relay__Entity_Resource__StateMachine;
            entity.Event__Ability_Points_Updated__GameEntity += Relay__Entity_Ability_Points__StateMachine;
        }

        private void Unbind__Entity(GameEntity_ServerSide entity)
        {
            entity.Game = null;
            
            entity.Event__Resource_Updated__GameEntity -= Relay__Entity_Resource__StateMachine;
            entity.Event__Ability_Points_Updated__GameEntity -= Relay__Entity_Ability_Points__StateMachine;
        }
        
        public void Set__Entity(GameEntity_ServerSide entity)
        {
            GameEntity_ServerSide existingEntity = GameField.Get_Entity(entity.GameEntity__ID);
            
            if (existingEntity != null)
                Unbind__Entity(existingEntity);
            
            Bind__Entity(entity);
            
            GameField.Set_Entity(entity);
        }

        public void Set__Entities(GameEntity_ServerSide[] entities)
        {
            foreach(GameEntity_ServerSide entity in entities)
                Set__Entity(entity);
        }
        
        public GameState CurrentGameState { get; private set; }
        public GameState RequestedGameState { get; private set; }

        public bool IsCombatHappening => CurrentGameState != null && CurrentGameState is GameState_Combat;
        public bool HasStarted { get; private set; }

        public Game_StateMachine(MonkeyDungeon_Server server, GameState[] gameStates)
        {
            Server = server;

            foreach (GameState gameState in gameStates)
                Add_GameState(gameState);

            GameEntityServerSideFactory = new GameEntity_ServerSide_Factory(this);
            GameField = new GameEntity_ServerSide_Roster();

            CurrentGameState = gameStates[0];
        }
        
        public void Begin__StateMachine()
        {
            if (HasStarted)
                return; //TODO: Warn in log.
            HasStarted = true;

            Relay__Descriptions__StateMachine(GameEntity_Team_ID.TEAM_ONE_ID);

            Relay__Team__StateMachine(GameEntity_Team_ID.TEAM_ONE_ID);
            Dismiss__Team__StateMachine(GameEntity_Team_ID.TEAM_TWO_ID);

            //TODO: Make a means to send message to specific client, and all clients.
            Broadcast__Message__StateMachine(
                new MMW_Accept_Client()
                );
        }

        public void Request__State_Transition__StateMachine<T>() where T : GameState
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

        public void Check_For__GameState_Transition__StateMachine(double deltaTime = 0)
        {
            if (!HasStarted)
            {
                if (GameField.Check_If_Players_Are_Ready())
                {
                    Begin__StateMachine();
                }

                return;
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

        internal void Register__Multiplayer_Handlers__StateMachine(params Multiplayer_Message_Handler[] handlers)
            => Server.Register_Multiplayer_Handlers(handlers);

        internal void Broadcast__Message__StateMachine(Multiplayer_Message_Wrapper msg)
            => Server.Broadcast(msg);

        internal void Relay__Message__StateMachine(Multiplayer_Relay_ID relayId, Multiplayer_Message_Wrapper msg)
            => Server.Relay(relayId, msg);

        internal void Relay__Entity_Resource__StateMachine(GameEntity_ServerSide_Resource resource)
        {
            Server.Broadcast(
                new MMW_Update_Entity_Resource(
                    resource.Attached_Entity.GameEntity__ID,
                    (float)(resource.Quantity__Value / resource.Quantity__Maximal_Value),
                    resource.Attribute_Name
                    )
                );
        }

        internal void Relay__Entity__StateMachine(GameEntity_ServerSide entityServerSide)
        {
            //send abilities
            GameEntity_ServerSide_Ability[] abilities = entityServerSide.Get__Abilities__GameEntity();
            for (int i = 0; i < abilities.Length; i++)
            {
                Broadcast__Message__StateMachine(
                    new MMW_Update_Entity_Ability(entityServerSide.GameEntity__ID, GameEntity_Ability_Index.INDICES[i], abilities[i].Attribute_Name)
                    );
                
                Broadcast__Message__StateMachine(
                    new MMW_Update_Entity_Ability_Target_Type(
                        entityServerSide.GameEntity__ID,
                        abilities[i].Ability__Combat_Target_Type,
                        abilities[i].Attribute_Name
                        )
                    );
            }

            //send uid
            Broadcast__Message__StateMachine(
                new MMW_Update_Entity_UniqueID(entityServerSide.GameEntity__ID, (uint)entityServerSide.Unique_ID)
                );

            //send resource names
            GameEntity_Attribute_Name[] resources = entityServerSide.Get__Resource_Names__GameEntity();
            for(int i=0;i<resources.Length;i++)
            {
                Broadcast__Message__StateMachine(
                    new MMW_Declare_Entity_Resource(entityServerSide.GameEntity__ID, resources[i])
                    );
                Relay__Entity_Resource__StateMachine(
                    entityServerSide.Get__Resource__GameEntity<GameEntity_ServerSide_Resource>(resources[i])
                    );
            }

            //introduce the entity to the scene.
            Relay__Entity_Introduction__StateMachine(entityServerSide.GameEntity__ID);
        }

        //TODO: fix
        internal void Relay__Descriptions__StateMachine(GameEntity_Team_ID teamId)
        {
            GameEntity_Attribute_Name[] descriptions = GameField.Get_Races(teamId);
            
            for (int i = 0; i < descriptions.Length; i++)
            {
                Broadcast__Message__StateMachine(
                    new MMW_Declare_Entity_Descriptions(GameEntity_ID.IDS[teamId + i], descriptions[i])
                    );
            }
        }

        internal void Relay__Team__StateMachine(GameEntity_Team_ID teamID)
        {
            foreach (GameEntity_ServerSide entity in GameField.Get_Entities(teamID))
                Relay__Entity__StateMachine(entity);
        }

        internal void Dismiss__Team__StateMachine(GameEntity_Team_ID teamId)
        {
            //internal, do not constraint argument.

            GameEntity_ID.For_Each__GameEntity_ID(teamId, Relay__Entity_Dismissal__StateMachine);
        }

        internal void Relay__Entity_Ability_Points__StateMachine(GameEntity_ServerSide_Resource resource)
        {
            Broadcast__Message__StateMachine(
                new MMW_Update_Ability_Point(resource.Attached_Entity.GameEntity__ID, (int)resource.Quantity__Value)
                );
        }

        internal void Relay__Entity_Death__StateMachine(GameEntity_ServerSide gameEntityServerSide)
        {
            Broadcast__Message__StateMachine(
                new MMW_Entity_Death(gameEntityServerSide.GameEntity__ID)
                );
        }

        internal void Relay__Entity_Dismissal__StateMachine(GameEntity_ID entityId)
        {
            Broadcast__Message__StateMachine(
                new MMW_Dismiss_Entity(entityId)
                );
        }

        internal void Relay__Entity_Introduction__StateMachine(GameEntity_ID entityId)
        {
            Broadcast__Message__StateMachine(
                new MMW_Introduce_Entity(entityId)
                );
        }
    }
}
