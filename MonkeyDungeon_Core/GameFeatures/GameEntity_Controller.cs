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
        internal void Setup_CombatAction_Ability(GameEntity_Attribute_Name abilityName)
        {
            if (!IsAutomonous)
            {
                PendingCombatAction = new Combat_Action();
                PendingCombatAction.Action_Owner = Entity.GameEntity_ID;
                PendingCombatAction.Selected_Ability = abilityName;
            }
        }
        internal void Setup_CombatAction_Target(GameEntity_ID index)
        {
            if (!IsAutomonous)
            {
                if (PendingCombatAction == null)
                    throw new InvalidOperationException("Ability is null.");
                PendingCombatAction.Target.Add_Target(index);
            }
        }
        internal Combat_Action Get_CombatAction(GameEntity_EntityField gameField)
        {
            if (!Entity.Has_PlayableMoves())
                throw new NotImplementedException(); //TODO: send a combat_action which requests EOT.
                //return null;

            Combat_Action result = Handle_CombatAction_Request(gameField);
            if (result == null)
                return null;

            Combat_Action finishedAction = result.Copy();
            PendingCombatAction = null;
            return finishedAction; //TODO: review this stuff too in case this is lousy.
        }
        protected virtual Combat_Action Handle_CombatAction_Request(GameEntity_EntityField gameField)
        {
            return null;
        }

        internal void BeginCombat(GameEntity_EntityField gameField) => Handle_BeginCombat(gameField);
        protected virtual void Handle_BeginCombat(GameEntity_EntityField gameField) { }

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
