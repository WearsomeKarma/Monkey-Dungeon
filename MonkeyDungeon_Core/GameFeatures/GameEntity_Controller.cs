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
        public GameEntity_ServerSide EntityServerSide { get; private set; }

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
        internal Combat_Action Get_Combat_Action(GameEntity_ServerSide_Roster gameField)
        {
            if (!EntityServerSide.Has_PlayableMoves())
            {
                return new Combat_Action() {Action_Owner = EntityServerSide.GameEntity_ID, Action_Ends_Turn = true};
            }

            Combat_Action result = Handle_CombatAction_Request(gameField);
            if (result == null)
                return null;

            Combat_Action finishedAction = result.Copy();
            PendingCombatAction = null;
            return finishedAction; //TODO: review this stuff too in case this is lousy.
        }
        protected virtual Combat_Action Handle_CombatAction_Request(GameEntity_ServerSide_Roster gameField)
        {
            return null;
        }

        internal void BeginCombat(GameEntity_ServerSide_Roster gameField) => Handle_BeginCombat(gameField);
        protected virtual void Handle_BeginCombat(GameEntity_ServerSide_Roster gameField) { }

        public GameEntity_Controller(bool isAutonomous = false)
        {
            IsAutomonous = isAutonomous;
            Reset_Action();
        }

        internal void Gain_Control(GameEntity_ServerSide newEntityServerSide)
        {
            if (EntityServerSide != null)
                Lose_Control();
            Handle_Control_NewEntity(newEntityServerSide);
            EntityServerSide = newEntityServerSide;
            //TODO: remove this
            EntityServerSide.EntityController = this;

            Reset_Action();
        }
        
        internal void Lose_Control()
        {
            Handle_Control_LoseEntity();
            EntityServerSide = null;
            
            Reset_Action();
        }

        private void Reset_Action()
        {
            PendingCombatAction = new Combat_Action {Action_Owner = EntityServerSide?.GameEntity_ID ?? GameEntity_ID.ID_NULL};
        }
        
        protected virtual void Handle_Control_NewEntity(GameEntity_ServerSide newEntityServerSide) { }
        protected virtual void Handle_Control_LoseEntity() { }
        public virtual GameEntity_Controller Clone()
            => new GameEntity_Controller(IsAutomonous);
    }
}
