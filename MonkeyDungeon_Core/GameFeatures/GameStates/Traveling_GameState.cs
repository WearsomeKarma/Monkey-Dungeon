using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain.Tools;

namespace MonkeyDungeon_Core.GameFeatures.GameStates
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
            GameState_Machine.Broadcast(
                new MMW_Set_Traveling_State(true)
                );
        }

        protected override void Handle_End_State(GameState_Machine gameWorld)
        {
            GameState_Machine.Broadcast(
                new MMW_Set_Traveling_State(false)
                );
        }
    }
}
