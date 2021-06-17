using System;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;

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

        public readonly GameEntity_ServerSide_Roster GameField;

        public void Create__Entity(GameEntity_ID entityId, Multiplayer_Relay_ID relayId, GameEntity_Attribute_Name factory_Tag)
        {
            Server.Bind_To_Relay(entityId, relayId);
            GameEntity_ServerSide entity = GameEntity_Factory.Create_NewEntity(entityId, relayId, GameEntity_Position.ALL_NON_NULL__POSITIONS[entityId],  factory_Tag);
            
            Set__Entity(entity);
        }

        private void Bind__Entity(GameEntity_ServerSide entity)
        {
            entity.Game = this;
            
            entity.Event__Resource_Updated__GameEntity += Relay_Entity_Resource;
            entity.Event__Ability_Points_Updated__GameEntity += Relay_Entity_Static_Resource;
        }

        private void Unbind__Entity(GameEntity_ServerSide entity)
        {
            entity.Game = null;
            
            entity.Event__Resource_Updated__GameEntity -= Relay_Entity_Resource;
            entity.Event__Ability_Points_Updated__GameEntity -= Relay_Entity_Static_Resource;
        }
        
        public void Set__Entity(GameEntity_ServerSide entity)
        {
            GameEntity_ServerSide existingEntity = GameField.Get_Entity(entity.GameEntity_ID);
            
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

        public bool IsCombatHappening => CurrentGameState != null && CurrentGameState is Combat_GameState;
        public bool HasStarted { get; private set; }

        public GameState_Machine(MonkeyDungeon_Server server, GameState[] gameStates)
        {
            Server = server;

            foreach (GameState gameState in gameStates)
                Add_GameState(gameState);

            GameEntity_Factory = new GameEntity_Factory(this);
            GameField = new GameEntity_ServerSide_Roster();

            CurrentGameState = gameStates[0];
        }
        
        public void Begin_Game()
        {
            if (HasStarted)
                return; //TODO: Warn in log.
            HasStarted = true;

            Declare_Descriptions(0, GameField.Get_Races());

            Relay_Team(GameEntity_Team_ID.TEAM_ONE_ID);
            Dismiss_Team(GameEntity_Team_ID.TEAM_TWO_ID);

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
                if (GameField.Check_If_Players_Are_Ready())
                {
                    Begin_Game();
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

        internal void Register_Multiplayer_Handlers(params Multiplayer_Message_Handler[] handlers)
            => Server.Register_Multiplayer_Handlers(handlers);

        internal void Broadcast(Multiplayer_Message_Wrapper msg)
            => Server.Broadcast(msg);

        internal void Relay(Multiplayer_Relay_ID relayId, Multiplayer_Message_Wrapper msg)
            => Server.Relay(relayId, msg);

        internal void Relay_Entity_Resource(GameEntity_Resource resource)
        {
            Server.Broadcast(
                new MMW_Update_Entity_Resource(
                    resource.Internal_Parent.GameEntity_ID,
                    (float)(resource.Value / resource.Max_Quantity),
                    resource.Attribute_Name
                    )
                );
        }

        internal void Relay_Entity(GameEntity_ServerSide entityServerSide)
        {
            //send abilities
            GameEntity_Ability[] abilities = entityServerSide.Get__Abilities__GameEntity();
            for (int i = 0; i < abilities.Length; i++)
            {
                Broadcast(
                    new MMW_Update_Entity_Ability(entityServerSide.GameEntity_ID, GameEntity_Ability_Index.INDICES[i], abilities[i].Attribute_Name)
                    );
                
                Broadcast(
                    new MMW_Update_Entity_Ability_Target_Type(
                        entityServerSide.GameEntity_ID,
                        abilities[i].Ability__Combat_Target_Type,
                        abilities[i].Attribute_Name
                        )
                    );
            }

            //send uid
            Broadcast(
                new MMW_Update_Entity_UniqueID(entityServerSide.GameEntity_ID, (uint)entityServerSide.Unique_ID)
                );

            //send resource names
            GameEntity_Attribute_Name[] resources = entityServerSide.Get__Resource_Names__GameEntity();
            for(int i=0;i<resources.Length;i++)
            {
                Broadcast(
                    new MMW_Declare_Entity_Resource(entityServerSide.GameEntity_ID, resources[i])
                    );
            }

            //introduce the entity to the scene.
            Broadcast(
                new MMW_Introduce_Entity(entityServerSide.GameEntity_ID)
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

        internal void Relay_Team(GameEntity_Team_ID teamID)
        {
            foreach (GameEntity_ServerSide entity in GameField.Get_Entities(teamID))
                Relay_Entity(entity);
        }

        internal void Dismiss_Team(GameEntity_Team_ID teamID)
        {
            //internal, do not constraint argument.

            foreach (GameEntity_ServerSide entity in GameField.Get_Entities(teamID))
                Relay_Dismissal(entity);
        }

        internal void Relay_Entity_Static_Resource(GameEntity_Resource resource)
        {
            Broadcast(
                new MMW_Update_Ability_Point(resource.Internal_Parent.GameEntity_ID, (int)resource.Value)
                );
        }

        internal void Relay_Death(GameEntity_ServerSide gameEntityServerSide)
        {
            Broadcast(
                new MMW_Entity_Death(gameEntityServerSide.GameEntity_ID)
                );
        }

        internal void Relay_Dismissal(GameEntity_ServerSide gameEntityServerSide)
        {
            Broadcast(
                new MMW_Dismiss_Entity(gameEntityServerSide.GameEntity_ID)
                );
        }
    }
}
