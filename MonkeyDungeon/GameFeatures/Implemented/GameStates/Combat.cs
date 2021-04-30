using MonkeyDungeon.Components;
using MonkeyDungeon.Components.Implemented.Enemies.Goblins;
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

    public class Combat_GameState : GameStateHandler
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
        public List<EntityComponent> TurnOrder { get; private set; }
        public EntityComponent Entity_OfCurrentTurn => TurnOrder[TurnIndex];
        internal event Action<EntityController> TurnHasBegun;

        internal CombatAction PendingCombatAction { get; private set; }

        public EntityComponent[] Players => GameWorld.PlayerRoster.Entities;
        public EntityComponent[] Enemies => GameWorld.EnemyRoster.Entities;

        public Combat_GameState(Action stateBegun, Action stateConcluded, Action<EntityController> turnBegun) 
            : base(stateBegun, stateConcluded)
        {
            TurnHasBegun += turnBegun;
            TurnOffset = 1;
        }

        private List<EntityComponent> DictateTurnOrder(List<EntityComponent> initalParticipants)
        {
            return initalParticipants;
        }
        
        protected override void HandleUpdateState(GameWorld_StateMachine gameWorld, double deltaTime)
        {
            switch(CombatState)
            {
                case CombatState.BeginNextTurn:
                    BeginTurn();
                    break;
                case CombatState.PlayCurrentTurn:
                    CombatAction action = Entity_OfCurrentTurn.EntityController.Get_CombatAction(this);
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

        protected override void BeginState(GameWorld_StateMachine gameWorld)
        {
            List<EntityComponent> entities = new List<EntityComponent>();
            entities.AddRange(gameWorld.PlayerRoster.Entities);
            gameWorld.SetEnemyRoster(GenerateNewEnemies().ToArray());
            entities.AddRange(gameWorld.EnemyRoster.Entities);

            CombatState = CombatState.BeginNextTurn;

            TurnOrder = DictateTurnOrder(entities);
        }

        protected override void EndState(GameWorld_StateMachine gameWorld)
        {
            GenerateNewEnemies();
        }

        private List<EntityComponent> GenerateNewEnemies()
        {
            return new List<EntityComponent>()
            {
                new EC_Goblin(1)
            };
        }
    }
}
