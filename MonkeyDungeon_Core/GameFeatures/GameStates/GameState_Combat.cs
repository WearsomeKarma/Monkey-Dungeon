using MonkeyDungeon_Core.GameFeatures.GameComponents;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using MonkeyDungeon_Core.GameFeatures.GameStates.Combat;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameStates
{
    public enum CombatState
    {
        BeginNextTurn,
        PlayCurrentTurn,
        FinishCurrentTurn,
        FinishCombat,
        OutOfCombat
    }

    public class GameState_Combat : GameState
    {
        public CombatState CombatState { get; private set; }

        private readonly Combat_Action_Resolver ACTION_RESOLVER;
        
        /// <summary>
        /// The current turn of the entities.
        /// </summary>
        public int TurnIndex { get; private set; }
        
        /// <summary>
        /// If 1 - normal turn progresion. If 0 - current entity is taking another turn.
        /// </summary>
        public int TurnOffset { get; private set; }

        public List<GameEntity_ID> TurnOrder { get; private set; }
        public GameEntity_ServerSide Entity_Of_Current_Turn => Game_Field.Get_Entity(TurnOrder[TurnIndex]);
        public GameEntity_ServerSide_Controller ServerSideControllerOfCurrentTurn => Entity_Of_Current_Turn.EntityServerSideController;
        public GameEntity_ID Entity_ID_Of_Current_Turn => Entity_Of_Current_Turn.GameEntity_ID;
        public Multiplayer_Relay_ID Entity_Of_Current_Turn_Relay_Id => Entity_Of_Current_Turn.Multiplayer_Relay_ID;

        public GameEntity_ServerSide_Roster Game_Field => GameState_Machine.GameField;
        internal GameEntity_ServerSide[] Players => Game_Field.Get_Entities(GameEntity_Team_ID.TEAM_ONE_ID);
        internal GameEntity_ServerSide[] Enemies => Game_Field.Get_Entities(GameEntity_Team_ID.TEAM_TWO_ID);

        public GameState_Combat()
        {
            TurnOffset = 1;

            ACTION_RESOLVER = new Combat_Action_Resolver(this);
        }

        protected override void Handle_AcquiredWorld()
        {
            GameState_Machine.Register_Multiplayer_Handlers(
                new MMH_Set_Entity(this),
                new MMH_Set_Entity_Ready(this),
                new MMH_Set_Combat_Action(this),
                new MMH_Set_Combat_Target(this),
                new MMH_Request_EndTurn(this)
                );
        }

        private void DictateTurnOrder()
        {
            TurnOrder = new List<GameEntity_ID>();

            GameEntity_ServerSide[] players = Players;
            GameEntity_ServerSide[] enemies = Enemies;

            for(int i=0;i< players.Length;i++)
            {
                TurnOrder.Add(players[i].GameEntity_ID);
            }
            for(int i=0;i<enemies.Length;i++)
            {
                TurnOrder.Add(enemies[i].GameEntity_ID);
            }
        }
        
        protected override void Handle_Update_State(Game_StateMachine gameWorld, double deltaTime)
        {
            switch(CombatState)
            {
                case CombatState.BeginNextTurn:
                    
                    Begin_Turn();
                    break;
                
                case CombatState.PlayCurrentTurn:
                    
                    if (Check_For_Team_Incapacitation())
                        break;
                    
                    GameEntity_ServerSide_Action action = ServerSideControllerOfCurrentTurn.Get_Combat_Action();
                    
                    if (action == GameEntity_ServerSide_Action.END_TURN_ACTION)
                    {
                        Request_EndOfTurn();
                        break;
                    }
                    
                    if (action?.IsSetupComplete ?? false)
                    {
                        GameState_Machine.Broadcast(
                            new MMW_Announcement(action.Action__Selected_Ability.Attribute_Name)
                        );
                        
                        ACTION_RESOLVER.Resolve_Action(action);

                        ServerSideControllerOfCurrentTurn.Action_Performed();
                    }
                    break;
                
                case CombatState.FinishCurrentTurn:
                    Finish_Turn();
                    break;
                case CombatState.FinishCombat:
                    Finish_Combat();
                    break;
                case CombatState.OutOfCombat:
                default: //do nothing.
                    break;
            }
        }

        private void Begin_Turn()
        {
            Entity_Of_Current_Turn.Combat_Begin_Turn__GameEntity();

            //Relay of 0 or higher is a player, so we tell them their turn has begun.
            if (Entity_Of_Current_Turn_Relay_Id > -1)
            {
                GameState_Machine.Relay(
                    Entity_Of_Current_Turn_Relay_Id,
                    new MMW_Begin_Turn(Entity_ID_Of_Current_Turn)
                );
            }

            CombatState = CombatState.PlayCurrentTurn;
        }

        private void Finish_Turn()
        {
            Entity_Of_Current_Turn.Combat_End_Turn__GameEntity();
            Progress_TurnOrder();
            
            CombatState = CombatState.BeginNextTurn;
        }

        private bool Check_For_Team_Incapacitation()
        {
            if (Is_Team_Incapacitated(GameEntity_Team_ID.TEAM_TWO_ID))
            {
                CombatState = CombatState.FinishCombat;
                return true;
            }
            if (Is_Team_Incapacitated(GameEntity_Team_ID.TEAM_ONE_ID))
            {
                GameState_Machine.Request_Transition_ToState<GameState_GameOver>();
                return true;
            }

            return false;
        }
        
        private void Finish_Combat()
        {
            foreach(GameEntity_ServerSide entity in Game_Field.Get_Entities())
                entity.Combat_End__GameEntity();
            
            GameState_Machine.Request_Transition_ToState<GameState_Traveling>();
        }
        
        internal void Act_Melee_Attack(GameEntity_ID scene_GameObject_ID1, GameEntity_ID scene_GameObject_ID2)
        {
            GameEntity_ID ally, enemy;
            if (scene_GameObject_ID1 < MD_PARTY.MAX_PARTY_SIZE)
            {
                ally = scene_GameObject_ID1;
                enemy = scene_GameObject_ID2;
            }
            else
            {
                ally = scene_GameObject_ID2;
                enemy = scene_GameObject_ID1;
            }
            GameState_Machine.Broadcast(
                new MMW_Set_Melee_Combattants(
                    ally,
                    enemy
                    )
                );

            GameState_Machine.Broadcast(
                new MMW_Invoke_UI_Event(
                    MD_VANILLA_UI_EVENT_NAMES.UI_EVENT_MELEE
                    )
                );
        }

        internal void Act_Ranged_Attack(GameEntity_ID shooterId, GameEntity_ID targetId, GameEntity_Attribute_Name particleType)
        {
            GameState_Machine.Broadcast(
                new MMW_Set_Ranged_Particle(
                    shooterId,
                    targetId,
                    particleType
                    )
                );
        }

        public void TakeAnExtraTurn() => TurnOffset = 0;
        private void Normalize_TurnProgression() => TurnOffset = 1;
        private void Progress_TurnOrder()
        {
            TurnIndex = (TurnIndex + TurnOffset) % TurnOrder.Count;
            Normalize_TurnProgression();
        }

        public void Request_EndOfTurn()
        {
            CombatState = CombatState.FinishCurrentTurn;
        }

        protected override void Handle_Begin_State(Game_StateMachine gameWorld)
        {
            List<GameEntity_ServerSide> enemies = GenerateNewEnemies(gameWorld);
            GameEntity_Attribute_Name[] enemyRaces = new GameEntity_Attribute_Name[enemies.Count];
            for (int i = 0; i < enemies.Count; i++)
                enemyRaces[i] = enemies[i].GameEntity_Race;

            for (int i = 0; i < enemyRaces.Length; i++)
            {
                GameState_Machine.Broadcast(
                    new MMW_Declare_Entity_Descriptions(GameEntity_ID.IDS[i + MD_PARTY.MAX_PARTY_SIZE], enemyRaces[i])
                    );
            }

            gameWorld.Set__Entities(enemies.ToArray());

            gameWorld.Relay_Team(GameEntity_Team_ID.TEAM_TWO_ID);

            CombatState = CombatState.BeginNextTurn;

            DictateTurnOrder();
        }

        protected override void Handle_End_State(Game_StateMachine gameWorld)
        {
            gameWorld.Dismiss_Team(GameEntity_Team_ID.TEAM_TWO_ID);
        }

        internal bool Is_Team_Incapacitated(GameEntity_Team_ID teamID)
        {
            bool ret = true;
            foreach (GameEntity_ServerSide entity in Game_Field.Get_Entities(teamID))
                ret = ret && entity.IsIncapacitated;
            return ret;
        }

        //TODO: FIX THIS
        private List<GameEntity_ServerSide> GenerateNewEnemies(Game_StateMachine gameState)
        {
            return new List<GameEntity_ServerSide>()
            {
                GameState_Machine.GameEntityServerSideFactory.Create_NewEntity(GameEntity_ID.ID_FOUR,  Multiplayer_Relay_ID.ID_NULL, GameEntity_Position.TEAM_TWO__FRONT_RIGHT, MD_VANILLA_RACE_NAMES.RACE_GOBLIN),
                GameState_Machine.GameEntityServerSideFactory.Create_NewEntity(GameEntity_ID.ID_FIVE,  Multiplayer_Relay_ID.ID_NULL, GameEntity_Position.TEAM_TWO__FRONT_LEFT, MD_VANILLA_RACE_NAMES.RACE_GOBLIN),
                GameState_Machine.GameEntityServerSideFactory.Create_NewEntity(GameEntity_ID.ID_SIX,   Multiplayer_Relay_ID.ID_NULL, GameEntity_Position.TEAM_TWO__REAR_RIGHT, MD_VANILLA_RACE_NAMES.RACE_GOBLIN),
                GameState_Machine.GameEntityServerSideFactory.Create_NewEntity(GameEntity_ID.ID_SEVEN, Multiplayer_Relay_ID.ID_NULL, GameEntity_Position.TEAM_TWO__REAR_LEFT, MD_VANILLA_RACE_NAMES.RACE_GOBLIN)
            };
        }
    }
}
