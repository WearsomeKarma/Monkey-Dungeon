
using MonkeyDungeon_Vanilla_Domain;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class Combat_Action
    {
        public GameEntity_ID Action_Owner { get; internal set; }
        public GameEntity_Attribute_Name Selected_Ability { get; internal set; }
        public Combat_Ability_Target Target { get; private set; }
        
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
