using MonkeyDungeon_Core.GameFeatures.GameEntities;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    public class MMH_Set_Combat_Action : Multiplayer_Message_GameStateHandler
    {
        Combat_GameState Combat { get; set; }

        public MMH_Set_Combat_Action(Combat_GameState gameState) 
            : base(gameState, MD_VANILLA_MMH.MMH_SET_COMBAT_ACTION)
        {
            Combat = gameState;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int relayId = recievedMessage.Relay_ID;

            if (Combat.Entity_Of_Current_Turn_Relay_Id != relayId)
            {
                Handle_Invalid_Message(recievedMessage);
                return;
            }

            GameEntity_ID targetId = GameEntity_ID.IDS[recievedMessage.INT_VALUE];
            GameEntity_Attribute_Name abilityName = recievedMessage.ATTRIBUTE;

            //TODO: remove player controller honestly, just make players not have controllers. Or rethink the system.
            GameEntity_Controller controller = 
                Combat.Game_Field.Get_Entity(recievedMessage.ENTITY_ID).Game_Entity.EntityController;
            
            controller.Setup_CombatAction_Ability(abilityName);
            controller.Setup_CombatAction_Target(targetId);
        }
    }
}
