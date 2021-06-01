
using MonkeyDungeon_Vanilla_Domain;

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
        public bool Has_Targets => Target?.Targets_Set() ?? false;
        public bool Targets_Illegal => Target?.Targets_Set() == null;

        public bool IsSetupComplete => 
            Action_Owner != null
            &&
            Ability_Set
            &&
            (!Requires_Target || Has_Targets);

        public Combat_Action Copy()
        {
            Combat_Action copy = new Combat_Action();
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
