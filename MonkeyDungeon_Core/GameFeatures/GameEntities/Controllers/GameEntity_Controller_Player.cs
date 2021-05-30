using MonkeyDungeon_Core.GameFeatures.GameEntities;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameStates;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.EntityControllers
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
