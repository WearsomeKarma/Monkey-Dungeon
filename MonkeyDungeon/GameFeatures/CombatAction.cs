using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using MonkeyDungeon.Scenes.GameScenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class CombatAction
    {
        public GameScene GameScene { get; private set; }

        public EntityComponent Owner_OfCombatAction { get; internal set; }

        public EntityComponent Target { get; internal set; }

        public bool HasTarget => Target != null;
    
        public string CombatAction_Ability_Name { get; private set; }
        public bool Requires_Target { get; private set; }
        private bool ability_HasBeenSet = false;
        public bool Set_Ability(string abilityName)
        {
            CombatAction_Ability_Name = abilityName;
            Ability a = Owner_OfCombatAction?.Get_Ability(abilityName);
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




        public CombatAction(
            GameScene gameScene
            )
        {
            GameScene = gameScene;
        }

        public CombatAction(CombatAction action)
        {
            GameScene = action.GameScene;
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
                  Owner_OfCombatAction.Get_Ability(CombatAction_Ability_Name)
                );
        }
    }
}
