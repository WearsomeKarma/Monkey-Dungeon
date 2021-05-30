using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat
{
    public class GameEntity_Combat_Ability : GameEntity_Ability
    {
        public Combat_Damage_Type Damage_Type { get; protected set; }
        public Combat_Assault_Type Assault_Type { get; protected set; }
        public Combat_Target_Type Target_Type { get; protected set; }
        public bool Has_Strict_Targets { get; protected set; }

        public GameEntity_Combat_Ability(string name, string resourceName, string statName) 
            : base(name, resourceName, statName)
        {
        }

        protected virtual void Handle_Begin_Ability_Resolution(Combat_Action combatAction)
        {

        }

        protected virtual double Handle_Calculate_Hit_Bonus()
        {
            return 0;
        }

        protected virtual double Handle_Calculate_Redirect_Chance()
        {
            return 0;
        }

        protected virtual double Handle_Calculate_Damage()
        {
            return 0;
        }
    }
}
