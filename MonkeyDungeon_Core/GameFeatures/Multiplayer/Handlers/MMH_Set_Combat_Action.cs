using MonkeyDungeon_Core.GameFeatures.GameEntities;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    public class MMH_Set_Combat_Action : Multiplayer_Message_CombatState_Handler
    {
        Combat_GameState Combat { get; set; }

        public MMH_Set_Combat_Action(Combat_GameState gameState) 
            : base(gameState, MD_VANILLA_MMH.MMH_COMBAT_SET_SELECTED_ABILITY)
        {
            Combat = gameState;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int relayId = recievedMessage.Relay_ID;

            //TODO: do not make conditional checker method handle invalid message logic as well.
            if (!IsValid_Message(recievedMessage))
                return;

            GameEntity_Attribute_Name abilityName = recievedMessage.ATTRIBUTE;

            //TODO: remove player controller honestly, just make players not have controllers. Or rethink the system.
            GameEntity_Controller controller =
                Combat.Controller_Of_Current_Turn;

            controller.Controller_Setup__Select_Ability(abilityName);
        }
    }
}
