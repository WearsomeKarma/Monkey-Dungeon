using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using MonkeyDungeon.Scenes.GameScenes;

namespace MonkeyDungeon.GameFeatures.Implemented.ActingEntities
{
    public class ActingEntity_Player : EntityController
    {
        protected override CombatAction Handle_CombatAction_Request(Combat_GameState combat)
        {
            if (PendingCombatAction?.IsSetupComplete ?? false)
                return PendingCombatAction;
            return null;
        }
    }
}
