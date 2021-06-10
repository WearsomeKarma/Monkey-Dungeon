using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

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
        internal void Setup_Combat_Action_Ability(GameEntity_Attribute_Name abilityName)
        {
            if (!IsAutomonous)
            {
                Reset_Action();
                PendingCombatAction.Selected_Ability = abilityName;
            }
        }
        internal void Setup_Combat_Action_Target(GameEntity_Position position)
        {
            if (!IsAutomonous)
            {
                if (PendingCombatAction == null)
                    throw new InvalidOperationException("Ability is null.");
                PendingCombatAction.Target.Add_Target(position);
            }
        }
        internal Combat_Action Get_Combat_Action(GameEntity_Field_RosterEntry gameFieldRosterEntry)
        {
            if (!Entity.Has_PlayableMoves())
            {
                return new Combat_Action() {Action_Owner = Entity.GameEntity_ID, Action_Ends_Turn = true};
            }

            Combat_Action result = Handle_CombatAction_Request(gameFieldRosterEntry);
            if (result == null)
                return null;

            Combat_Action finishedAction = result.Copy();
            PendingCombatAction = null;
            return finishedAction; //TODO: review this stuff too in case this is lousy.
        }
        protected virtual Combat_Action Handle_CombatAction_Request(GameEntity_Field_RosterEntry gameFieldRosterEntry)
        {
            return null;
        }

        internal void BeginCombat(GameEntity_Field_RosterEntry gameFieldRosterEntry) => Handle_BeginCombat(gameFieldRosterEntry);
        protected virtual void Handle_BeginCombat(GameEntity_Field_RosterEntry gameFieldRosterEntry) { }

        public GameEntity_Controller(bool isAutonomous = false)
        {
            IsAutomonous = isAutonomous;
            Reset_Action();
        }

        internal void Gain_Control(GameEntity newEntity)
        {
            if (Entity != null)
                Lose_Control();
            Handle_Control_NewEntity(newEntity);
            Entity = newEntity;
            //TODO: remove this
            Entity.EntityController = this;

            Reset_Action();
        }
        
        internal void Lose_Control()
        {
            Handle_Control_LoseEntity();
            Entity = null;
            
            Reset_Action();
        }

        private void Reset_Action()
        {
            PendingCombatAction = new Combat_Action {Action_Owner = Entity?.GameEntity_ID ?? GameEntity_ID.ID_NULL};
        }
        
        protected virtual void Handle_Control_NewEntity(GameEntity newEntity) { }
        protected virtual void Handle_Control_LoseEntity() { }
        public virtual GameEntity_Controller Clone()
            => new GameEntity_Controller(IsAutomonous);
    }
}
