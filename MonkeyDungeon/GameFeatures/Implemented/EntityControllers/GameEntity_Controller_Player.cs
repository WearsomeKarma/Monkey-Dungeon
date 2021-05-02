using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon.GameFeatures.CombatObjects;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using MonkeyDungeon.Scenes.GameScenes;

namespace MonkeyDungeon.GameFeatures.Implemented.EntityControllers
{
    public class GameEntity_Controller_Player : GameEntity_Controller
    {
        protected override Combat_Action Handle_CombatAction_Request(Combat_GameState combat)
        {
            if (PendingCombatAction?.IsSetupComplete ?? false)
                return PendingCombatAction;
            return null;
        }

        public override GameEntity_Controller Clone()
        {
            return new GameEntity_Controller_Player();
        }
    }
}
