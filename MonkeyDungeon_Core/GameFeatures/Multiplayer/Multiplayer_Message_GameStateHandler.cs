using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer
{
    public abstract class Multiplayer_Message_GameStateHandler : Multiplayer_Message_Handler
    {
        protected GameState_Machine GameState_Machine { get; private set; }

        public Multiplayer_Message_GameStateHandler(GameState_Machine gameStateMachine, params string[] acceptedTypes)
            : base(acceptedTypes)
        {
            GameState_Machine = gameStateMachine;
        }
    }
}
