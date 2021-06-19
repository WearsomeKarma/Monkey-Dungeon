using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    public class MMH_Set_Combat_Target : Multiplayer_Message_CombatState_Handler
    {
        public MMH_Set_Combat_Target(GameState_Combat gameState) 
            : base(gameState, MD_VANILLA_MMH.MMH_COMBAT_ADD_TARGET)
        {
            
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            if (!IsValid_Message(recievedMessage))
                return;

            GameEntity_ServerSide_Controller serverSideController = GameStateCombat.ServerSideControllerOfCurrentTurn;

            serverSideController.Combat_Setup__Add_Target(GameEntity_Position.ALL_NON_NULL__POSITIONS[recievedMessage.INT_VALUE]);
        }
    }
}