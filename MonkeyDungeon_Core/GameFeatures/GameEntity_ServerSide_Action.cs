
using System;
using System.Collections.Generic;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_ServerSide_Action
    {
        public static readonly GameEntity_ServerSide_Action END_TURN_ACTION = new GameEntity_ServerSide_Action();
        
        /// <summary>
        /// The ability selected for the action.
        /// </summary>
        public GameEntity_ServerSide_Ability Action__Selected_Ability { get; private set; }
        /// <summary>
        /// The entity owner of the action.
        /// </summary>
        public GameEntity_ID Action__Invoking_Entity => GameEntity_ID.Nullwrap(Action__Selected_Ability?.Ability__Invoker__GameEntity_ID);
        /// <summary>
        /// The selected targets.
        /// </summary>
        public Combat_Survey_Target Action__Survey_Target { get; private set; }

        public GameEntity_ServerSide_Quantity Action__Hit_Bonus__Of_Invoking_Entity { get; internal set; }
        public Combat_Survey_Redirection Action__Survey_Redirection { get; private set; }
        public GameEntity_Survey_Quantity<GameEntity_ServerSide, GameEntity_ServerSide_Quantity> Action__Survey_Dodge_Bonuses { get; private set; }
        public Combat_Survey_Damage<GameEntity_ServerSide> Action__Survey_Damage { get; private set; }
        
        public bool Ability_Set => Action__Selected_Ability != null;
        public bool Requires_Target => Action__Survey_Target?.Target_Type != Combat_Target_Type.Self_Or_No_Target;
        public bool Has_Targets => Action__Survey_Target?.Has_Legal_Targets() ?? false;
        public bool Action_Ends_Turn { get; set; }

        public bool IsSetupComplete => 
            GameEntity_ID.Validate(Action__Invoking_Entity)
            &&
            Ability_Set
            &&
            (!Requires_Target || Has_Targets);

        public GameEntity_ServerSide_Action()
        {
            Action__Survey_Target = new Combat_Survey_Target();
            Action__Survey_Redirection = new Combat_Survey_Redirection();
            Action__Survey_Damage = new Combat_Survey_Damage<GameEntity_ServerSide>
                (
                () => new GameEntity_Damage<GameEntity_ServerSide>(GameEntity_Damage_Type.Abstract, 0)
                );
            Action__Survey_Dodge_Bonuses =
                new GameEntity_Survey_Quantity<GameEntity_ServerSide, GameEntity_ServerSide_Quantity>
                (
                    ()=> GameEntity_ServerSide_Quantity.Get__Generic_At_Zero__ServerSide_Quantity()
                );
        }

        internal void Set_Ability(GameEntity_ServerSide_Ability ability)
        {
            Action__Selected_Ability = ability;
            Action__Survey_Target.Target_Type = ability.Ability__Combat_Target_Type;
            Action__Survey_Target.Has_Strict_Targets = ability.Ability__Combat_Enforces_Strict_Targetting;
        }
        
        //TODO: remove field argument.
        internal void Begin_Action_Resolution(GameEntity_ServerSide_Roster field)
        {
            GameEntity_ServerSide owner = field.Get_Entity(Action__Invoking_Entity);
            
            Console.WriteLine("--[Combat_Action.cs:63]--\n" + this);
            Action__Survey_Target.Bind_To_Action(owner.GameEntity__Position, owner.GameEntity__Team_ID, Action__Selected_Ability.Ability__Combat_Target_Type, Action__Selected_Ability.Ability__Combat_Enforces_Strict_Targetting);
        }

        public override string ToString()
        {
            return string.Format(
                  "Action Owner: {0}" +
                "\nAction Target: {1}" +
                "\nAbility: {2}",
                  Action__Invoking_Entity,
                  Action__Survey_Target,
                  Action__Selected_Ability
                );
        }
    }
}
