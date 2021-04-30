using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    /// <summary>
    /// Represents the control over an EntityComponent
    /// </summary>
    public abstract class EntityController
    {
        public bool IsAutomonous { get; private set; }

        protected GameWorld_StateMachine GameWorld { get; private set; }
        public EntityComponent Entity { get; private set; }

        internal CombatAction PendingCombatAction { get; private set; }
        internal void Setup_CombatAction_Ability(string abilityName)
        {
            if(!IsAutomonous)
                PendingCombatAction = new CombatAction(Entity, abilityName);
        }
        internal void Setup_CombatAction_Target(EntityComponent target)
        {
            if (!IsAutomonous)
            {
                if (PendingCombatAction == null)
                    throw new InvalidOperationException("Ability is null.");
                PendingCombatAction.Target = target;
            }
        }
        internal CombatAction Get_CombatAction(Combat_GameState combat)
        {
            if (!Entity.Has_PlayableMoves(combat))
            {
                combat.Request_EndOfTurn();
                return null;
            }

            CombatAction result = Handle_CombatAction_Request(combat);
            if (result == null)
                return null;

            CombatAction finishedAction = new CombatAction(result);
            PendingCombatAction = null;
            return finishedAction;
        }
        protected abstract CombatAction Handle_CombatAction_Request(Combat_GameState combat);

        internal void BeginCombat(Combat_GameState combat) => Handle_BeginCombat(combat);
        protected virtual void Handle_BeginCombat(Combat_GameState combat) { }

        public EntityController(bool isAutonomous = false)
        {
            IsAutomonous = isAutonomous;
        }

        internal void GainControl(EntityComponent newEntity)
        {
            if (Entity != null)
                LoseControl();
            Handle_Control_NewEntity(newEntity);
            Entity = newEntity;
            Entity.EntityController = this;
        }
        
        internal void LoseControl()
        {
            Handle_Control_LoseEntity();
            Entity = null;
        }

        protected virtual void Handle_Control_NewEntity(EntityComponent newEntity) { }
        protected virtual void Handle_Control_LoseEntity() { }
    }
}
