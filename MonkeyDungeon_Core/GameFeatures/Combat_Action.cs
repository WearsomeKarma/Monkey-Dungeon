
using System;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class Combat_Action
    {
        /// <summary>
        /// The entity owner of the action.
        /// </summary>
        public GameEntity_ID Action_Owner { get; internal set; }
        /// <summary>
        /// The ability selected for the action.
        /// </summary>
        public GameEntity_Attribute_Name Selected_Ability { get; internal set; }
        /// <summary>
        /// The selected targets.
        /// </summary>
        public Combat_Ability_Target Target { get; private set; }
        
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
        
        public Combat_Finalized_Factor Finalized_Hit_Bonus { get; internal set; }
        
        public Combat_Finalized_Factor[] Finalized_Dodge_Bonuses { get; internal set; }
        
        public bool Ability_Set => Selected_Ability != null;
        public bool Requires_Target => (Target != null) ? (Target?.Target_Type != Combat_Target_Type.Self_Or_No_Target) : false;
        public bool Has_Targets => Target?.Has_Valid_Targets() ?? false;
        public bool Action_Ends_Turn { get; set; }

        public bool IsSetupComplete => 
            Action_Owner != null
            &&
            Ability_Set
            &&
            (!Requires_Target || Has_Targets);

        public Combat_Action()
        {
            Target = new Combat_Ability_Target();
            Action_Owner = GameEntity_ID.ID_NULL;
        }

        internal void Finalized_Action(GameEntity_EntityField field)
        {
            System.Console.WriteLine(this);
            GameEntity_RosterEntry owner = field.Get_Entity(Action_Owner);
            GameEntity ownerEntity = owner.Game_Entity;
            GameEntity_Ability ability = ownerEntity.Ability_Manager.Get_Ability<GameEntity_Ability>(Selected_Ability);
            
            Target.Bind_To_Fields(ability.Owner_ID, ability.Target_Type, ability.Has_Strict_Targets);
        }
        
        public Combat_Action Copy()
        {
            Combat_Action copy = new Combat_Action();
            copy.Action_Owner = Action_Owner;
            copy.Selected_Ability = Selected_Ability;
            copy.Target = Target;
            return copy;
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
