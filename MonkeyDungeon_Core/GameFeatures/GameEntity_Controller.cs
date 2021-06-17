using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures
{
    /// <summary>
    /// Represents the control over an EntityComponent
    /// </summary>
    public class GameEntity_Controller
    {
        public bool IsAutomonous { get; private set; }

        protected GameEntity_ServerSide Attached_GameEntity { get; private set; }
        protected GameEntity_ID Attached_GameEntity_ID => GameEntity_ID.Nullwrap(Attached_GameEntity?.GameEntity_ID);
        public GameState_Machine GameState_Machine => Attached_GameEntity?.Game;
        public GameEntity_ServerSide_Roster GameEntity_Roster => Attached_GameEntity?.Entity_Field;
        
        internal Combat_Action Controller_Combat_Action { get; private set; }
        internal Combat_Target Controller_Combat_Target => Controller_Combat_Action?.Target;
        
        internal void Controller_Setup__Select_Ability(GameEntity_Attribute_Name abilityName)
        {
            Controller_Combat_Action.Set_Ability(Attached_GameEntity.Get__Ability__GameEntity<GameEntity_Ability>(abilityName));
        }
        internal bool Combat_Setup__Add_Target(GameEntity_Position position)
        {
            return Controller_Combat_Action.Target.Add_Target(position);
        }
        
        internal Combat_Action Get_Combat_Action()
        {
            if (!Attached_GameEntity.Has_PlayableMoves__GameEntity())
            {
                return Combat_Action.END_TURN_ACTION;
            }

            Handle_Get__Combat_Action__Controller();
            if (!Controller_Combat_Action?.IsSetupComplete ?? true)
                return null;

            return Controller_Combat_Action; //TODO: review this stuff too in case this is lousy.
        }
        protected virtual void Handle_Get__Combat_Action__Controller() { }

        internal void Action_Performed()
            => Controller_Reset_Pending_Action();

        internal void Combat_Begin() => Handle_Combat_Begin();
        protected virtual void Handle_Combat_Begin() { }

        internal void Combat_Begin_Turn()
        {
            Controller_Reset_Pending_Action();
            Handle_Combat_Begin_Turn();
        }
        protected virtual void Handle_Combat_Begin_Turn() { }

        internal void Combat_End_Turn()
        {
            Handle_Combat_End_Turn();
        }
        protected virtual void Handle_Combat_End_Turn() { }

        internal void Combat_End()
        {
            Handle_Combat_End();
        }
        protected virtual void Handle_Combat_End() { }

        private void Controller_Reset_Pending_Action()
            => Controller_Combat_Action = new Combat_Action();
        
        
        
        public GameEntity_Controller(bool isAutonomous = false)
        {
            IsAutomonous = isAutonomous;
            Controller_Reset_Pending_Action();
        }

        internal void Controller_Attack_To_Entity(GameEntity_ServerSide newEntityServerSide)
        {
            if (Attached_GameEntity != null)
                Controller_Detach_From_Entity();
            Handle_Controller_Attach_To_Entity(newEntityServerSide);
            Attached_GameEntity = newEntityServerSide;
            //TODO: remove this
            Attached_GameEntity.EntityController = this;

            Controller_Reset_Pending_Action();
        }
        
        internal void Controller_Detach_From_Entity()
        {
            Handle_Controller_Detach_From_Entity();
            Attached_GameEntity = null;
            
            Controller_Reset_Pending_Action();
        }
        
        protected virtual void Handle_Controller_Attach_To_Entity(GameEntity_ServerSide newEntityServerSide) { }
        protected virtual void Handle_Controller_Detach_From_Entity() { }
        public virtual GameEntity_Controller Clone__Controller()
            => new GameEntity_Controller(IsAutomonous);
    }
}
