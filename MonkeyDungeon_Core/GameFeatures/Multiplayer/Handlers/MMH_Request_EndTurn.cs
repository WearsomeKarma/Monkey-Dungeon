﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    public class MMH_Request_EndTurn : Multiplayer_Message_GameStateHandler
    {
        Combat_GameState Combat { get; set; }

        public MMH_Request_EndTurn(Combat_GameState gameState) 
            : base(gameState, MD_VANILLA_MMH.MMH_REQUEST_ENDTURN)
        {
            Combat = gameState;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int relayId = recievedMessage.Relay_ID;
            if (Combat.Entity_OfCurrentTurn_Relay_Id != relayId)
            {
                Handle_Invalid_Message(recievedMessage);
                return;
            }

            Combat.Request_EndOfTurn();
        }
    }
}
