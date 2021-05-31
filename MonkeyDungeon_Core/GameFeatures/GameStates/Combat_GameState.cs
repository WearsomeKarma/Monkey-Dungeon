using MonkeyDungeon_Core.GameFeatures.GameEntities;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers;
using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;

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
        
        /// <summary>
        /// The current turn of the entities.
        /// </summary>
        public int TurnIndex { get; private set; }
        
        /// <summary>
        /// If 1 - normal turn progresion. If 0 - current entity is taking another turn.
        /// </summary>
        public int TurnOffset { get; private set; }

        public List<GameEntity> TurnOrder { get; private set; }
        public GameEntity Entity_OfCurrentTurn => TurnOrder[TurnIndex];
        public int Entity_OfCurrentTurn_Scene_Id => Entity_OfCurrentTurn.Scene_GameObject_ID;
        public int Entity_OfCurrentTurn_Relay_Id => Entity_OfCurrentTurn.Relay_ID_Of_Owner;

        public GameEntity_EntityField Game_Field => GameState_Machine.GAME_FIELD;
        internal GameEntity_Roster Players => Game_Field.PLAYERS;
        internal GameEntity_Roster Enemies => Game_Field.ENEMIES;

        public Combat_GameState()
        {
            TurnOffset = 1;
        }

        protected override void Handle_AcquiredWorld()
        {
            GameState_Machine.Register_Multiplayer_Handlers(
                new MMH_Set_Entity(this),
                new MMH_Set_Entity_Ready(this),
                new MMH_Set_Combat_Action(this),
                new MMH_Request_EndTurn(this)
                );
        }

        private void DictateTurnOrder()
        {
            TurnOrder = new List<GameEntity>();

            GameEntity[] players = Players.Entities;
            GameEntity[] enemies = Enemies.Entities;

            for(int i=0;i< players.Length;i++)
            {
                TurnOrder.Add(players[i]);
                players[i].Scene_GameObject_ID = GameEntity_ID.IDS[MD_PARTY.MAX_PARTY_SIZE + i];
                players[i].Initative_Position = i;
            }
            for(int i=0;i<enemies.Length;i++)
            {
                TurnOrder.Add(enemies[i]);
                enemies[i].Scene_GameObject_ID = GameEntity_ID.IDS[MD_PARTY.MAX_PARTY_SIZE + i];
                enemies[i].Initative_Position = MD_PARTY.MAX_PARTY_SIZE + i;
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
                    if (Is_EnemyTeam_Incapacitated())
                    {
                        GameState_Machine.Request_Transition_ToState<Traveling_GameState>();
                        break;
                    }
                    if (Is_AllyTeam_Incapacitated())
                    {
                        GameState_Machine.Request_Transition_ToState<GameOver_GameState>();
                        break;
                    }
                    Combat_Action action = Entity_OfCurrentTurn.EntityController.Get_CombatAction(Game_Field);
                    if (action != null)
                    {
                        throw new NotImplementedException();
                        /*
                        if (action.Conduct_Action(this))
                        {
                            GameState_Machine.Broadcast(
                                new MMW_Announcement(action.CombatAction_Ability_Name)
                                );
                        }
                        else
                        {
                            //TODO: implement.
                        }
                        */
                    }
                    break;
                case CombatState.FinishCurrentTurn:
                    Entity_OfCurrentTurn.Combat_EndTurn(Game_Field);
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
            Entity_OfCurrentTurn.Combat_BeginTurn(Game_Field);

            if (Entity_OfCurrentTurn_Relay_Id < 0)
                return;

            GameState_Machine.Relay(
                Entity_OfCurrentTurn.Relay_ID_Of_Owner,
                new MMW_Begin_Turn(Entity_OfCurrentTurn.Scene_GameObject_ID)
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
                    MD_VANILLA_UI.UI_EVENT_MELEE
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
            List<GameEntity> enemies = GenerateNewEnemies(gameWorld);
            GameEntity_Attribute_Name[] enemyRaces = new GameEntity_Attribute_Name[enemies.Count];
            for (int i = 0; i < enemies.Count; i++)
                enemyRaces[i] = enemies[i].Race;

            for (int i = 0; i < enemyRaces.Length; i++)
            {
                GameState_Machine.Broadcast(
                    new MMW_Declare_Entity_Descriptions(GameEntity_ID.IDS[i + MD_PARTY.MAX_PARTY_SIZE], enemyRaces[i])
                    );
            }

            gameWorld.Set_Enemy_Roster(enemies.ToArray());

            gameWorld.Relay_Roster(Enemies);

            CombatState = CombatState.BeginNextTurn;

            DictateTurnOrder();
        }

        protected override void Handle_End_State(GameState_Machine gameWorld)
        {
            gameWorld.Dismiss_Roster(Enemies);
        }

        internal bool Is_AllyTeam_Incapacitated()
        {
            bool ret = true;
            foreach (GameEntity player in Players.Entities)
                ret = ret && player.IsIncapacitated;
            return ret;
        }

        internal bool Is_EnemyTeam_Incapacitated()
        {
            bool ret = true;
            foreach (GameEntity enemy in Enemies.Entities)
                ret = ret && enemy.IsIncapacitated;
            return ret;
        }

        //TODO: FIX THIS
        private List<GameEntity> GenerateNewEnemies(GameState_Machine gameState)
        {
            return new List<GameEntity>()
            {
                GameState_Machine.GameEntity_Factory.Create_NewEntity(GameEntity_ID.ID_FOUR,  Multiplayer_Relay_ID.NULL_ID, MD_VANILLA_RACES.RACE_GOBLIN),
                GameState_Machine.GameEntity_Factory.Create_NewEntity(GameEntity_ID.ID_FIVE,  Multiplayer_Relay_ID.NULL_ID, MD_VANILLA_RACES.RACE_GOBLIN),
                GameState_Machine.GameEntity_Factory.Create_NewEntity(GameEntity_ID.ID_SIX,   Multiplayer_Relay_ID.NULL_ID, MD_VANILLA_RACES.RACE_GOBLIN),
                GameState_Machine.GameEntity_Factory.Create_NewEntity(GameEntity_ID.ID_SEVEN, Multiplayer_Relay_ID.NULL_ID, MD_VANILLA_RACES.RACE_GOBLIN)
            };
        }
    }
}
