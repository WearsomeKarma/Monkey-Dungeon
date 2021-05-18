using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.CombatObjects
{
    public class Combat_Action
    {
        public Combat_GameState Combat_GameState { get; private set; }

        public GameEntity Owner_OfCombatAction { get; internal set; }

        public GameEntity Target { get; internal set; }
        public void Set_Target(int index)
        {
            if (index >= 0)
                Target = Combat_GameState.Get_Target(index);
        }

        public bool HasTarget => Target != null;
    
        public string CombatAction_Ability_Name { get; private set; }
        public bool Requires_Target { get; private set; }
        private bool ability_HasBeenSet = false;
        public bool Set_Ability(string abilityName)
        {
            CombatAction_Ability_Name = abilityName;
            GameEntity_Ability a = Owner_OfCombatAction?.Ability_Manager.Get_Ability(abilityName);
            ability_HasBeenSet = a != null;
            if (ability_HasBeenSet)
            {
                Requires_Target = a.Requires_Target;
            }
            return ability_HasBeenSet;
        }

        public bool IsSetupComplete => 
            Owner_OfCombatAction != null
            &&
            ability_HasBeenSet
            &&
            (!Requires_Target || HasTarget);

        public Combat_Action(Combat_GameState combat_GameState)
        {
            Combat_GameState = combat_GameState;
        }

        public Combat_Action(Combat_Action action)
        {
            Combat_GameState = action.Combat_GameState;
            Owner_OfCombatAction = action.Owner_OfCombatAction;
            Target = action.Target;
            Set_Ability(action.CombatAction_Ability_Name);
        }
        
        internal bool Conduct_Action(Combat_GameState combat)
        {
            return Owner_OfCombatAction.Use_Ability(this);
        }

        public override string ToString()
        {
            return string.Format(
                  "Action Owner: {0}" +
                "\nAction Target: {1}" +
                "\nAbility: {2}",
                  Owner_OfCombatAction,
                  Target,
                  Owner_OfCombatAction.Ability_Manager.Get_Ability(CombatAction_Ability_Name)
                );
        }
    }
}
