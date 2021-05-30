
namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class Combat_Action
    {
        public GameEntity Action_Owner { get; internal set; }
        public GameEntity_Ability Selected_Ability { get; private set; }
        public Combat_Ability_Target Target { get; private set; }
        
        public string Ability_Name { get; private set; }
        public bool Ability_Set => Selected_Ability != null;
        public bool Requires_Target => Ability_Set && Selected_Ability.Target_Type != Combat_Target_Type.Self_Or_No_Target;
        public bool Has_Targets => Target.Targets_Set() ?? false;
        public bool Targets_Illegal => Target.Targets_Set() == null;

        public bool Set_Ability(string abilityName)
        {
            Ability_Name = abilityName;
            Selected_Ability = Action_Owner?.Ability_Manager.Get_Ability<GameEntity_Ability>(abilityName);

            Target.Set_Ability(Selected_Ability);

            return Ability_Set;
        }

        public bool IsSetupComplete => 
            Action_Owner != null
            &&
            Ability_Set
            &&
            (!Requires_Target || Has_Targets);

        public Combat_Action()
        {
            Target = new Combat_Ability_Target();
        }

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
                  Action_Owner.Ability_Manager.Get_Ability<GameEntity_Ability>(Ability_Name)
                );
        }
    }
}
