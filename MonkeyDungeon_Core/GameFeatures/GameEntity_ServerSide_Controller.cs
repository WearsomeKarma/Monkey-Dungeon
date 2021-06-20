using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
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
    public class GameEntity_ServerSide_Controller : GameEntity_Attribute<GameEntity_ServerSide>
    {
        public bool IsAutomonous { get; private set; }

        protected GameEntity_ID Attached_GameEntity_ID => GameEntity_ID.Nullwrap(Attached_Entity?.GameEntity__ID);
        public GameEntity_ServerSide_Roster GameEntity_Roster => Attached_Entity?.Entity_Field;
        
        internal GameEntity_ServerSide_Action GameEntity_Controller_ServerSide_Action { get; private set; }
        internal Combat_Survey_Target ControllerCombatSurveyTarget => GameEntity_Controller_ServerSide_Action?.Action__Survey_Target;
        
        internal void Controller_Setup__Select_Ability__ServerSide_Controller(GameEntity_Attribute_Name abilityName)
        {
            GameEntity_Controller_ServerSide_Action.Set_Ability(Attached_Entity.Get__Ability__GameEntity<GameEntity_ServerSide_Ability>(abilityName));
        }
        internal bool Combat_Setup__Add_Target__ServerSide_Controller(GameEntity_Position position)
        {
            return GameEntity_Controller_ServerSide_Action.Action__Survey_Target.Add_Target(position);
        }
        
        internal GameEntity_ServerSide_Action Get_Combat_Action()
        {
            if (!Attached_Entity.Has_PlayableMoves__GameEntity())
            {
                return GameEntity_ServerSide_Action.END_TURN_ACTION;
            }

            Handle_Get__Combat_Action__Controller();
            if (!GameEntity_Controller_ServerSide_Action?.IsSetupComplete ?? true)
                return null;

            return GameEntity_Controller_ServerSide_Action; //TODO: review this stuff too in case this is lousy.
        }
        protected virtual void Handle_Get__Combat_Action__Controller() { }

        internal void Begin__Combat__ServerSide_Controller() => Handle_Begin__Combat__ServerSide_Controller();
        protected virtual void Handle_Begin__Combat__ServerSide_Controller() { }

        internal void Begin__Combat_Turn__ServerSide_Controller()
        {
            Reset__Pending_Action__ServerSide_Controller();
            Handle_Begin__Combat_Turn__ServerSide_Controller();
        }
        protected virtual void Handle_Begin__Combat_Turn__ServerSide_Controller() { }

        internal void Combat_End_Turn()
        {
            Handle_Conclude__Combat_Turn__ServerSide_Controller();
        }
        protected virtual void Handle_Conclude__Combat_Turn__ServerSide_Controller() { }

        internal void Conclude__Combat__ServerSide_Controller()
        {
            Handle_Conclude__Combat__ServerSide_Controller();
        }
        protected virtual void Handle_Conclude__Combat__ServerSide_Controller() { }

        internal void Reset__Pending_Action__ServerSide_Controller()
            => GameEntity_Controller_ServerSide_Action = new GameEntity_ServerSide_Action();
        
        
        
        public GameEntity_ServerSide_Controller(bool isAutonomous = false)
        : base(GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME) //TODO: make controller tags.
        {
            IsAutomonous = isAutonomous;
            Reset__Pending_Action__ServerSide_Controller();
        }

        internal void Attach_To__Entity__ServerSide_Controller(GameEntity_ServerSide newEntityServerSide)
            => Attach_To__Entity__Attribute(newEntityServerSide);

        internal void Controller_Detach_From_Entity()
            => Detach_From__Entity__Attribute();
        
        public virtual GameEntity_ServerSide_Controller Clone__Controller()
            => new GameEntity_ServerSide_Controller(IsAutomonous);
    }
}
