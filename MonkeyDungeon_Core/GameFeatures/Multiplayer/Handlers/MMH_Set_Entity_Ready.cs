using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    /// <summary>
    /// This message handler is responsible for establishing character 
    /// information recieved from the client.
    /// INT - 0: ready | !0: not ready.
    /// </summary>
    public class MMH_Set_Entity_Ready : Multiplayer_Message_GameStateHandler
    {
        public MMH_Set_Entity_Ready(GameState_Machine gameStateMachine) 
            : base(gameStateMachine, MD_VANILLA_MMH.MMH_SET_ENTITY_READY)
        {
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            GameState_Machine.PlayerRoster.Set_Ready_To_Start(recievedMessage.ENTITY_ID, recievedMessage.INT_VALUE);
        }
    }
}
