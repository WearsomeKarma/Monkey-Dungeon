using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameStates;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Controllers
{
    public class GameEntity_Controller_Player : GameEntity_Controller
    {
        protected override Combat_Action Handle_CombatAction_Request(GameEntity_ServerSide_Roster gameField)
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
