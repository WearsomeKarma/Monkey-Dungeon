using MonkeyDungeon.GameFeatures.CombatObjects;
using MonkeyDungeon.GameFeatures.Implemented.Entities.Enemies.Goblins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonkeyDungeon.GameFeatures.Implemented.GameStates
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
        public int Entity_OfCurrentTurn_Id => Entity_OfCurrentTurn.Scene_GameObject_ID;
        internal event Action<GameEntity_Controller> TurnHasBegun;

        internal Combat_Action PendingCombatAction { get; private set; }

        public GameEntity[] Players => GameWorld.PlayerRoster.Entities;
        public GameEntity[] ConsciousPlayers { get { List<GameEntity> cp = new List<GameEntity>(); foreach (GameEntity player in Players) if (!player.IsIncapacitated) cp.Add(player); return cp.ToArray(); } }
        public GameEntity[] Enemies => GameWorld.EnemyRoster.Entities;

        public Combat_GameState(Action stateBegun, Action stateConcluded, Action<GameEntity_Controller> turnBegun) 
            : base(stateBegun, stateConcluded)
        {
            TurnHasBegun += turnBegun;
            TurnOffset = 1;
        }

        private void DictateTurnOrder()
        {
            TurnOrder = new List<GameEntity>();

            GameEntity[] players = Players;
            GameEntity[] enemies = Enemies;

            for(int i=0;i< players.Length;i++)
            {
                TurnOrder.Add(players[i]);
                players[i].Scene_GameObject_ID = i;
                players[i].Initative_Position = i;
            }
            for(int i=0;i<enemies.Length;i++)
            {
                TurnOrder.Add(enemies[i]);
                enemies[i].Scene_GameObject_ID = GameState_Machine.MAX_TEAM_SIZE + i;
                enemies[i].Initative_Position = GameState_Machine.MAX_TEAM_SIZE + i;
            }
        }
        
        protected override void Handle_UpdateState(GameState_Machine gameWorld)
        {
            switch(CombatState)
            {
                case CombatState.BeginNextTurn:
                    BeginTurn();
                    break;
                case CombatState.PlayCurrentTurn:
                    if (Is_EnemyTeam_Incapacitated())
                    {
                        GameWorld.Request_Transition_ToState<Traveling_GameState>();
                        break;
                    }
                    if (Is_AllyTeam_Incapacitated())
                    {
                        GameWorld.Request_Transition_ToState<GameOver_GameState>();
                        break;
                    }
                    Combat_Action action = Entity_OfCurrentTurn.EntityController.Get_CombatAction(this);
                    if (action != null)
                    {
                        if (action.Conduct_Action(this))
                            GameWorld.GameScene.Announce_Action(action);
                        else
                            GameWorld.GameScene.Announce_ActionFailure(action);
                    }
                    break;
                case CombatState.FinishCurrentTurn:
                    Entity_OfCurrentTurn.Combat_EndTurn(this);
                    Progress_TurnOrder();
                    CombatState = CombatState.BeginNextTurn;
                    break;
                case CombatState.FinishCombat:
                    GameWorld.Request_Transition_ToState<Traveling_GameState>();
                    break;
                case CombatState.OutOfCombat:
                default: //do nothing.
                    break;
            }
        }

        private void BeginTurn()
        {
            CombatState = CombatState.PlayCurrentTurn;
            Entity_OfCurrentTurn.Combat_BeginTurn(this);
            TurnHasBegun?.Invoke(Entity_OfCurrentTurn.EntityController);
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

        protected override void Handle_BeginState(GameState_Machine gameWorld)
        {
            List<GameEntity> enemies = GenerateNewEnemies();
            gameWorld.SetEnemyRoster(enemies.ToArray());

            CombatState = CombatState.BeginNextTurn;

            DictateTurnOrder();
        }

        protected override void Handle_EndState(GameState_Machine gameWorld)
        {
            GenerateNewEnemies();
        }

        internal bool Is_AllyTeam_Incapacitated()
        {
            bool ret = true;
            foreach (GameEntity player in Players)
                ret = ret && player.IsIncapacitated;
            return ret;
        }

        internal bool Is_EnemyTeam_Incapacitated()
        {
            bool ret = true;
            foreach (GameEntity enemy in Enemies)
                ret = ret && enemy.IsIncapacitated;
            return ret;
        }

        private List<GameEntity> GenerateNewEnemies()
        {
            return new List<GameEntity>()
            {
                new EC_Goblin(1),
                new EC_Goblin(1),
                new EC_Goblin(1),
                new EC_Goblin(1)
            };
        }
    }
}
