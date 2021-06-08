using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    public class MMH_Set_Combat_Target : Multiplayer_Message_CombatState_Handler
    {
        public MMH_Set_Combat_Target(Combat_GameState gameState) 
            : base(gameState, MD_VANILLA_MMH.MMH_SET_COMBAT_TARGET)
        {
            
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            if (IsValid_Message(recievedMessage))
                return;

            GameEntity_RosterEntry entityEntry =
                GameState_Machine.GAME_FIELD.Get_Entity(recievedMessage.Local_Entity_ID);

            entityEntry.Controller.PendingCombatAction.Target.Add_Target(GameEntity_ID.IDS[recievedMessage.INT_VALUE]);
        }
    }
}