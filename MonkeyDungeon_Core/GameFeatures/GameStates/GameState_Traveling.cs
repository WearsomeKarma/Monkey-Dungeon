using MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers;
using MonkeyDungeon_Vanilla_Domain.Tools;

namespace MonkeyDungeon_Core.GameFeatures.GameStates
{
    public class GameState_Traveling : GameState
    {
        Timer timer = new Timer(3.5);

        public GameState_Traveling()
        {
        }

        protected override void Handle_Update_State(Game_StateMachine gameWorld, double deltaTime)
        {
            timer.Progress_DeltaTime(deltaTime);
            if (timer.IsFinished)
                gameWorld.Request_Transition_ToState<GameState_Combat>();
        }

        protected override void Handle_Begin_State(Game_StateMachine gameWorld)
        {
            timer.Set();
            GameState_Machine.Broadcast(
                new MMW_Set_Traveling_State(true)
                );
        }

        protected override void Handle_End_State(Game_StateMachine gameWorld)
        {
            GameState_Machine.Broadcast(
                new MMW_Set_Traveling_State(false)
                );
        }
    }
}
