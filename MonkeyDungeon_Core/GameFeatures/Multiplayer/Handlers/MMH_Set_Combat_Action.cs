using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    public class MMH_Set_Combat_Action : Multiplayer_Message_GameStateHandler
    {
        Combat_GameState Combat { get; set; }

        public MMH_Set_Combat_Action(Combat_GameState combat) 
            : base(combat.GameWorld, MD_VANILLA_MMH.MMH_SET_COMBAT_ACTION)
        {
            Combat = combat;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int targetId = recievedMessage.INT_VALUE;
            string abilityName = recievedMessage.STRING_VALUE;

            GameEntity_Controller controller = 
                Combat.Entity_OfCurrentTurn.EntityController;
            
            controller.Setup_CombatAction_Ability(abilityName);
            controller.Setup_CombatAction_Target(targetId);
        }
    }
}
