
using System;
using System.Collections.Generic;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class Combat_Action
    {
        public static readonly Combat_Action END_TURN_ACTION = new Combat_Action();
        
        /// <summary>
        /// The ability selected for the action.
        /// </summary>
        public GameEntity_Ability Selected_Ability { get; private set; }
        /// <summary>
        /// The entity owner of the action.
        /// </summary>
        public GameEntity_ID Action_Owner => GameEntity_ID.Nullwrap(Selected_Ability?.Ability_Owner__GameEntity_ID);
        /// <summary>
        /// The selected targets.
        /// </summary>
        public Combat_Target Target { get; private set; }
        
        /// <summary>
        /// The resource of each target that is effected by this action.
        /// </summary>
        public GameEntity_Attribute_Name Target_Affected_Resource { get; internal set; }
        /// <summary>
        /// The stat which this ability utilizes as a hit bonus.
        /// </summary>
        public GameEntity_Attribute_Name Stat_Hit_Bonus { get; internal set; }
        //TODO: ^ make this a new type to allow for multiple scalings. 30% strength, 70% agility.
        /// <summary>
        /// The stat which this ability utilizes for enemies to dodge by. DEFAULT: no bonus.
        /// </summary>
        public GameEntity_Attribute_Name Stat_Dodge_Bonus { get; internal set; }
        
        public Combat_Finalized_Value Finalized_Hit_Bonus { get; internal set; }

        public readonly Dictionary<GameEntity_ID, Combat_Finalized_Value> Dodge_Bonus_Foreach_Target =
            new Dictionary<GameEntity_ID, Combat_Finalized_Value>();
        
        public bool Ability_Set => Selected_Ability != null;
        public bool Requires_Target => Target?.Target_Type != Combat_Target_Type.Self_Or_No_Target;
        public bool Has_Targets => Target?.Has_Legal_Targets() ?? false;
        public bool Action_Ends_Turn { get; set; }

        public bool IsSetupComplete => 
            GameEntity_ID.Validate(Action_Owner)
            &&
            Ability_Set
            &&
            (!Requires_Target || Has_Targets);

        public Combat_Action()
        {
            Target = new Combat_Target();
        }

        internal void Set_Ability(GameEntity_Ability ability)
        {
            Selected_Ability = ability;
            Target.Target_Type = ability.Ability__Combat_Target_Type;
            Target.Has_Strict_Targets = ability.Ability__Combat_Enforces_Strict_Targetting;
            Target_Affected_Resource = ability.Ability__Affecting_Resource;
        }
        
        //TODO: remove field argument.
        internal void Begin_Action_Resolution(GameEntity_ServerSide_Roster field)
        {
            GameEntity_ServerSide owner = field.Get_Entity(Action_Owner);
            
            Console.WriteLine("--[Combat_Action.cs:63]--\n" + this);
            Target.Bind_To_Action(owner.GameEntity_Position, owner.GameEntity_Team_ID, Selected_Ability.Ability__Combat_Target_Type, Selected_Ability.Ability__Combat_Enforces_Strict_Targetting);
        }

        public override string ToString()
        {
            return string.Format(
                  "Action Owner: {0}" +
                "\nAction Target: {1}" +
                "\nAbility: {2}",
                  Action_Owner,
                  Target,
                  Selected_Ability
                );
        }
    }
}
