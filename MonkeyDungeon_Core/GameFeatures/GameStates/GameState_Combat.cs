using MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System.Collections.Generic;
using MonkeyDungeon_Core.GameFeatures.GameStates.Combat;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameStates
{
    public enum CombatState
    {
        CombatState_Progress__Turn_Order,
        CombatState_Begin__Turn,
        CombatState_Play__Current_Turn,
        CombatState_Finish__Current_Turn,
        CombatState_Finish__Combat,
        CombatState_Await__State_Transition
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

        public List<GameEntity_ID> Combat__Turn_Order { get; private set; }
        public GameEntity_ServerSide Combat__Entity__Of_Current_Turn => Combat__Game_Field.Get_Entity(Combat__Turn_Order[TurnIndex]);
        public GameEntity_ServerSide_Controller Combat__Entity_Controller__Of_Current_Turn => Combat__Entity__Of_Current_Turn.Entity_ServerSide_Controller;
        public GameEntity_ID Combat__Entity_ID__Of_Current_Turn => Combat__Entity__Of_Current_Turn.GameEntity__ID;
        public Multiplayer_Relay_ID Combat__Entity_Relay_Id__Of_Current_Turn => Combat__Entity__Of_Current_Turn.GameEntity__Multiplayer_Relay_ID;

        public GameEntity_ServerSide_Roster Combat__Game_Field => GameState_Machine.GameField;
        internal GameEntity_ServerSide[] Combat__Players => Combat__Game_Field.Get_Entities(GameEntity_Team_ID.TEAM_ONE_ID);
        internal GameEntity_ServerSide[] Combat__Enemies => Combat__Game_Field.Get_Entities(GameEntity_Team_ID.TEAM_TWO_ID);

        public GameState_Combat()
        {
            TurnOffset = 1;

            ACTION_RESOLVER = new Combat_Action_Resolver(this);
        }

        protected override void Handle_Acquired__Game_StateMachine__GameState()
        {
            GameState_Machine.Register__Multiplayer_Handlers__StateMachine(
                new MMH_Set_Entity(this),
                new MMH_Set_Entity_Ready(this),
                new MMH_Set_Combat_Action(this),
                new MMH_Set_Combat_Target(this),
                new MMH_Request_EndTurn(this)
                );
        }

        private void Dictate__Turn_Order()
        {
            Combat__Turn_Order = new List<GameEntity_ID>();

            GameEntity_ServerSide[] players = Combat__Players;
            GameEntity_ServerSide[] enemies = Combat__Enemies;

            for(int i=0;i< players.Length;i++)
            {
                Combat__Turn_Order.Add(players[i].GameEntity__ID);
            }
            for(int i=0;i<enemies.Length;i++)
            {
                Combat__Turn_Order.Add(enemies[i].GameEntity__ID);
            }
        }
        
        protected override void Handle_Update__State__GameState_Combat(Game_StateMachine gameWorld, double deltaTime)
        {
            switch(CombatState)
            {
                case CombatState.CombatState_Progress__Turn_Order:

                    Combat_State__Progress_Turn_Order();
                    break;
                
                case CombatState.CombatState_Begin__Turn:
                    
                    Combat_State__Begin_Turn();
                    break;
                
                case CombatState.CombatState_Play__Current_Turn:
                    
                    if (Check_For__Team_Incapacitation())
                        break;
                    
                    GameEntity_ServerSide_Action action = Combat__Entity_Controller__Of_Current_Turn.Get_Combat_Action();
                    
                    if (action == GameEntity_ServerSide_Action.END_TURN_ACTION)
                    {
                        Request_EndOfTurn();
                        break;
                    }
                    
                    if (action?.IsSetupComplete ?? false)
                    {
                        GameState_Machine.Broadcast__Message__StateMachine(
                            new MMW_Announcement(action.Action__Selected_Ability.Attribute_Name)
                        );
                        
                        ACTION_RESOLVER.Resolve__Action__Resolver(action);

                        Combat__Entity_Controller__Of_Current_Turn.Reset__Pending_Action__ServerSide_Controller();
                    }
                    break;
                
                case CombatState.CombatState_Finish__Current_Turn:
                    Combat_State__Finish_Turn();
                    break;
                case CombatState.CombatState_Finish__Combat:
                    Combat_State__Finish_Combat();
                    break;
                case CombatState.CombatState_Await__State_Transition:
                default: //do nothing.
                    break;
            }
        }

        private void Combat_State__Progress_Turn_Order()
        {
            Progress__Turn_Order();
            CombatState = CombatState.CombatState_Begin__Turn;
        }
        
        private void Combat_State__Begin_Turn()
        {
            if (Combat__Entity__Of_Current_Turn.GameEntity__Is_Not_Present)
            {
                CombatState = CombatState.CombatState_Progress__Turn_Order;
                return;
            }

            Combat__Entity__Of_Current_Turn.Combat_Begin_Turn__GameEntity();

            //Relay of 0 or higher is a player, so we tell them their turn has begun.
            if (Combat__Entity_Relay_Id__Of_Current_Turn > -1)
            {
                GameState_Machine.Relay__Message__StateMachine(
                    Combat__Entity_Relay_Id__Of_Current_Turn,
                    new MMW_Begin_Turn(Combat__Entity_ID__Of_Current_Turn)
                );
            }

            CombatState = CombatState.CombatState_Play__Current_Turn;
        }

        private void Combat_State__Finish_Turn()
        {
            Combat__Entity__Of_Current_Turn.Combat_End_Turn__GameEntity();
            
            CombatState = CombatState.CombatState_Progress__Turn_Order;
        }

        private bool Check_For__Team_Incapacitation()
        {
            if (Check_If__Team_Is_Incapacitated(GameEntity_Team_ID.TEAM_TWO_ID))
            {
                CombatState = CombatState.CombatState_Finish__Combat;
                return true;
            }
            if (Check_If__Team_Is_Incapacitated(GameEntity_Team_ID.TEAM_ONE_ID))
            {
                GameState_Machine.Request__State_Transition__StateMachine<GameState_GameOver>();
                return true;
            }

            return false;
        }
        
        private void Combat_State__Finish_Combat()
        {
            foreach(GameEntity_ServerSide entity in Combat__Game_Field.Get_Entities())
                entity.Combat_End__GameEntity();
            
            GameState_Machine.Request__State_Transition__StateMachine<GameState_Traveling>();
        }
        
        internal void Relay__Melee_Attack__GameState_Combat(GameEntity_ID scene_GameObject_ID1, GameEntity_ID scene_GameObject_ID2)
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
            GameState_Machine.Broadcast__Message__StateMachine(
                new MMW_Set_Melee_Combattants(
                    ally,
                    enemy
                    )
                );

            GameState_Machine.Broadcast__Message__StateMachine(
                new MMW_Invoke_UI_Event(
                    MD_VANILLA_UI_EVENT_NAMES.UI_EVENT_MELEE
                    )
                );
        }

        internal void Relay__Ranged_Attack__GameState_Combat(GameEntity_ID shooterId, GameEntity_ID targetId, GameEntity_Attribute_Name particleType)
        {
            GameState_Machine.Broadcast__Message__StateMachine(
                new MMW_Set_Ranged_Particle(
                    shooterId,
                    targetId,
                    particleType
                    )
                );
        }

        public void Register__Extra_Turn__GameState_Combat() => TurnOffset = 0;
        private void Normalize__Turn_Progression() => TurnOffset = 1;
        private void Progress__Turn_Order()
        {
            TurnIndex = (TurnIndex + TurnOffset) % Combat__Turn_Order.Count;
            Normalize__Turn_Progression();
        }

        public void Request_EndOfTurn()
        {
            CombatState = CombatState.CombatState_Finish__Current_Turn;
        }

        protected override void Handle_Begin__State__GameState(Game_StateMachine gameWorld)
        {
            List<GameEntity_ServerSide> enemies = Generate__New_Enemies(gameWorld);
            GameEntity_Attribute_Name[] enemyRaces = new GameEntity_Attribute_Name[enemies.Count];
            for (int i = 0; i < enemies.Count; i++)
                enemyRaces[i] = enemies[i].GameEntity__Race;

            for (int i = 0; i < enemyRaces.Length; i++)
            {
                GameState_Machine.Broadcast__Message__StateMachine(
                    new MMW_Declare_Entity_Descriptions(GameEntity_ID.IDS[i + MD_PARTY.MAX_PARTY_SIZE], enemyRaces[i])
                    );
            }

            gameWorld.Set__Entities(enemies.ToArray());

            gameWorld.Relay__Team__StateMachine(GameEntity_Team_ID.TEAM_TWO_ID);
            

            CombatState = CombatState.CombatState_Begin__Turn;

            Dictate__Turn_Order();
        }

        protected override void Handle_Conclude__State__GameState(Game_StateMachine gameWorld)
        {
            gameWorld.Dismiss__Team__StateMachine(GameEntity_Team_ID.TEAM_TWO_ID);
        }

        internal bool Check_If__Team_Is_Incapacitated(GameEntity_Team_ID teamID)
        {
            bool ret = true;
            foreach (GameEntity_ServerSide entity in Combat__Game_Field.Get_Entities(teamID))
                ret = ret && entity.GameEntity__Is_Incapacitated;
            return ret;
        }

        //TODO: FIX THIS
        private List<GameEntity_ServerSide> Generate__New_Enemies(Game_StateMachine gameState)
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
