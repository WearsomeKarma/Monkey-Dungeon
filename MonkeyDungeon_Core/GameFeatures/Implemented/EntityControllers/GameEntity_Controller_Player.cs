using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.EntityControllers
{
    public class GameEntity_Controller_Player : GameEntity_Controller
    {
        protected override Combat_Action Handle_CombatAction_Request(Combat_GameState combat)
        {
            if (PendingCombatAction?.IsSetupComplete ?? false)
                return PendingCombatAction;

            //throw new NotImplementedException();
            //TODO: SET UP RELAYS
            //Setup_CombatAction_Ability(combat, combat.Combat_Relay.Get_Selected_Ability());
            //Setup_CombatAction_Target(combat.Combat_Relay.Get_Selected_Target());

            return null;
        }

        public override GameEntity_Controller Clone()
        {
            return new GameEntity_Controller_Player();
        }
    }
}
