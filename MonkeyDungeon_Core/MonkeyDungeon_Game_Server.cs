using MonkeyDungeon_Core.GameFeatures;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using MonkeyDungeon_Core.GameFeatures.Multiplayer;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core
{
    public class MonkeyDungeon_Game_Server
    {
        public GameState_Machine GameState_Machine { get; private set; }
        public Multiplayer_Reciever ServerSide_Local_Reciever { get; private set; }

        public MonkeyDungeon_Game_Server(
            Multiplayer_Reciever localEndpoint
            )
        {
            ServerSide_Local_Reciever = localEndpoint;
            GameState_Machine = new GameState_Machine(
                this,
                new GameState[]
                {
                    new Traveling_GameState(),
                    new Combat_GameState(),
                    new Shopping_GameState(),
                    new GameOver_GameState()
                }
                );
        }

        public void On_Update_Frame()
        {
            ServerSide_Local_Reciever.CheckFor_NewMessages();
            GameState_Machine.CheckFor_GameState_Transition();

            ServerSide_Local_Reciever.Flush_Messages();
        }
    }
}
