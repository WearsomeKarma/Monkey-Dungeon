using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.GameStates
{
    public class Traveling_GameState : GameState
    {
        Timer timer = new Timer(3.5);

        public Traveling_GameState()
        {
        }

        protected override void Handle_Update_State(GameState_Machine gameWorld, double deltaTime)
        {
            timer.Progress_DeltaTime(deltaTime);
            if (timer.IsFinished)
                gameWorld.Request_Transition_ToState<Combat_GameState>();
        }

        protected override void Handle_Begin_State(GameState_Machine gameWorld)
        {
            timer.Set();
            GameWorld.Server.ServerSide_Local_Reciever.Queue_Message(
                new MMH_Set_Traveling_State(true)
                );
        }

        protected override void Handle_End_State(GameState_Machine gameWorld)
        {
            GameWorld.Server.ServerSide_Local_Reciever.Queue_Message(
                new MMH_Set_Traveling_State(false)
                );
        }
    }
}
