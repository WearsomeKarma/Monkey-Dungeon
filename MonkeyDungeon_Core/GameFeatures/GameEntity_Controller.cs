using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using System;

namespace MonkeyDungeon_Core.GameFeatures
{
    /// <summary>
    /// Represents the control over an EntityComponent
    /// </summary>
    public class GameEntity_Controller
    {
        public bool IsAutomonous { get; private set; }

        protected GameState_Machine GameWorld { get; private set; }
        public GameEntity Entity { get; private set; }

        internal Combat_Action PendingCombatAction { get; private set; }
        internal void Setup_CombatAction_Ability(string abilityName)
        {
            if (!IsAutomonous)
            {
                PendingCombatAction = new Combat_Action();
                PendingCombatAction.Action_Owner = Entity;
                PendingCombatAction.Set_Ability(abilityName);
            }
        }
        internal void Setup_CombatAction_Target(int index)
        {
            if (!IsAutomonous)
            {
                if (PendingCombatAction == null)
                    throw new InvalidOperationException("Ability is null.");
                PendingCombatAction.Target.Add_Target(index);
            }
        }
        internal Combat_Action Get_CombatAction(Combat_GameState combat)
        {
            if (Entity.CheckIf_IsTurnUnplayable(combat))
            {
                return null;
            }

            Combat_Action result = Handle_CombatAction_Request(combat);
            if (result == null)
                return null;

            Combat_Action finishedAction = result.Copy();
            PendingCombatAction = null;
            return finishedAction;
        }
        protected virtual Combat_Action Handle_CombatAction_Request(Combat_GameState combat)
        {
            return null;
        }

        internal void BeginCombat(Combat_GameState combat) => Handle_BeginCombat(combat);
        protected virtual void Handle_BeginCombat(Combat_GameState combat) { }

        public GameEntity_Controller(bool isAutonomous = false)
        {
            IsAutomonous = isAutonomous;
        }

        internal void GainControl(GameEntity newEntity)
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

        protected virtual void Handle_Control_NewEntity(GameEntity newEntity) { }
        protected virtual void Handle_Control_LoseEntity() { }
        public virtual GameEntity_Controller Clone()
            => new GameEntity_Controller(IsAutomonous);
    }
}
