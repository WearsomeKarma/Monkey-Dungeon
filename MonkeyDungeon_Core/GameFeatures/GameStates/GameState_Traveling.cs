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

        protected override void Handle_Update__State__GameState_Combat(Game_StateMachine gameWorld, double deltaTime)
        {
            timer.Progress_DeltaTime(deltaTime);
            if (timer.IsFinished)
                gameWorld.Request__State_Transition__StateMachine<GameState_Combat>();
        }

        protected override void Handle_Begin__State__GameState(Game_StateMachine gameWorld)
        {
            timer.Set();
            GameState_Machine.Broadcast__Message__StateMachine(
                new MMW_Set_Traveling_State(true)
                );
        }

        protected override void Handle_Conclude__State__GameState(Game_StateMachine gameWorld)
        {
            GameState_Machine.Broadcast__Message__StateMachine(
                new MMW_Set_Traveling_State(false)
                );
        }
    }
}
