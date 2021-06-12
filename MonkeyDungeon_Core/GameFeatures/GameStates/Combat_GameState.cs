using MonkeyDungeon_Core.GameFeatures.GameEntities;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
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

    public class Combat_GameState : GameState
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
        public GameEntity_ID Entity_ID_Of_Current_Turn => Entity_Of_Current_Turn.GameEntity_ID;
        public Multiplayer_Relay_ID Entity_Of_Current_Turn_Relay_Id => Entity_Of_Current_Turn.Multiplayer_Relay_ID;

        public GameEntity_ServerSide_Roster Game_Field => GameState_Machine.GameField;
        internal GameEntity_ServerSide[] Players => Game_Field.Get_Entities(GameEntity_Team_ID.TEAM_ONE_ID);
        internal GameEntity_ServerSide[] Enemies => Game_Field.Get_Entities(GameEntity_Team_ID.TEAM_TWO_ID);

        public Combat_GameState()
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
        
        protected override void Handle_Update_State(GameState_Machine gameWorld, double deltaTime)
        {
            switch(CombatState)
            {
                case CombatState.BeginNextTurn:
                    BeginTurn();
                    break;
                case CombatState.PlayCurrentTurn:
                    if (Is_Team_Incapacitated(GameEntity_Team_ID.TEAM_TWO_ID))
                    {
                        GameState_Machine.Request_Transition_ToState<Traveling_GameState>();
                        break;
                    }
                    if (Is_Team_Incapacitated(GameEntity_Team_ID.TEAM_ONE_ID))
                    {
                        GameState_Machine.Request_Transition_ToState<GameOver_GameState>();
                        break;
                    }
                    Combat_Action action = Entity_Of_Current_Turn.EntityController.Get_Combat_Action(Game_Field);
                    if (action != null)
                    {
                        if (action.Action_Ends_Turn)
                        {
                            Request_EndOfTurn();
                            break;
                        }
                        GameState_Machine.Broadcast(
                            new MMW_Announcement(action.Selected_Ability)
                        );
                        ACTION_RESOLVER.Resolve_Action(action);
                    }
                    break;
                case CombatState.FinishCurrentTurn:
                    Entity_Of_Current_Turn.Combat_EndTurn(Game_Field);
                    Progress_TurnOrder();
                    CombatState = CombatState.BeginNextTurn;
                    break;
                case CombatState.FinishCombat:
                    GameState_Machine.Request_Transition_ToState<Traveling_GameState>();
                    break;
                case CombatState.OutOfCombat:
                default: //do nothing.
                    break;
            }
        }

        private void BeginTurn()
        {
            CombatState = CombatState.PlayCurrentTurn;
            Entity_Of_Current_Turn.Combat_BeginTurn(Game_Field);

            if (Entity_Of_Current_Turn_Relay_Id < 0)
                return;

            GameState_Machine.Relay(
                Entity_Of_Current_Turn_Relay_Id,
                new MMW_Begin_Turn(Entity_ID_Of_Current_Turn)
                );
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

        protected override void Handle_Begin_State(GameState_Machine gameWorld)
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

            gameWorld.Set_Enemy_Roster(enemies.ToArray());

            gameWorld.Relay_Team(GameEntity_Team_ID.TEAM_TWO_ID);

            CombatState = CombatState.BeginNextTurn;

            DictateTurnOrder();
        }

        protected override void Handle_End_State(GameState_Machine gameWorld)
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
        private List<GameEntity_ServerSide> GenerateNewEnemies(GameState_Machine gameState)
        {
            return new List<GameEntity_ServerSide>()
            {
                GameState_Machine.GameEntity_Factory.Create_NewEntity(GameEntity_ID.ID_FOUR,  Multiplayer_Relay_ID.ID_NULL, GameEntity_Position.TEAM_TWO__FRONT_RIGHT, MD_VANILLA_RACE_NAMES.RACE_GOBLIN),
                GameState_Machine.GameEntity_Factory.Create_NewEntity(GameEntity_ID.ID_FIVE,  Multiplayer_Relay_ID.ID_NULL, GameEntity_Position.TEAM_TWO__FRONT_LEFT, MD_VANILLA_RACE_NAMES.RACE_GOBLIN),
                GameState_Machine.GameEntity_Factory.Create_NewEntity(GameEntity_ID.ID_SIX,   Multiplayer_Relay_ID.ID_NULL, GameEntity_Position.TEAM_TWO__REAR_RIGHT, MD_VANILLA_RACE_NAMES.RACE_GOBLIN),
                GameState_Machine.GameEntity_Factory.Create_NewEntity(GameEntity_ID.ID_SEVEN, Multiplayer_Relay_ID.ID_NULL, GameEntity_Position.TEAM_TWO__REAR_LEFT, MD_VANILLA_RACE_NAMES.RACE_GOBLIN)
            };
        }
    }
}
