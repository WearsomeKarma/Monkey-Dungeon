
namespace MonkeyDungeon_Core.GameFeatures.GameStates
{
    public enum TransitionState
    {
        Awaiting,
        Beginning,      //Start up the state
        Acting,     //Play out the state
        Ending,     //Cleanup the state
        Finished    //Wait to be put on standby for next state.
    }

    public class GameState
    {
        public Game_StateMachine GameState_Machine { get; private set; }
        internal void Set_GameWorld(Game_StateMachine gameWorld)
        {
            GameState_Machine = gameWorld;
            Handle_Acquired__Game_StateMachine__GameState();
        }
        protected virtual void Handle_Acquired__Game_StateMachine__GameState() { }
        public TransitionState TransitionState { get; private set; }
        
        //TODO: Add relays.
        //protected UI_Relay UI_Relay { get; private set; }
        //internal virtual void Set_UI_Relay(UI_Relay ui_relay) => UI_Relay = ui_relay;

        public GameState()
        {
            TransitionState = TransitionState.Awaiting;
        }

        internal void Reset(Game_StateMachine gameWorld)
        {
            Handle_ResetState(gameWorld);
            TransitionState = TransitionState.Awaiting;
        }

        internal void Begin(Game_StateMachine gameWorld)
        {
            TransitionState = TransitionState.Beginning;
            Handle_Begin__State__GameState(gameWorld);
            TransitionState = TransitionState.Acting;
        }

        internal void End(Game_StateMachine gameWorld)
        {
            TransitionState = TransitionState.Ending;
            Handle_Conclude__State__GameState(gameWorld);
            TransitionState = TransitionState.Finished;
        }

        internal void UpdateState(Game_StateMachine gameWorld, double deltaTime=0)
        {
            Handle_Update__State__GameState_Combat(gameWorld, deltaTime);
        }

        protected void End() => TransitionState = TransitionState.Ending;

        protected virtual void Handle_ResetState(Game_StateMachine gameWorld)
        {

        }
        protected virtual void Handle_Begin__State__GameState(Game_StateMachine gameWorld)
        {

        }
        protected virtual void Handle_Conclude__State__GameState(Game_StateMachine gameWorld)
        {

        }
        protected virtual void Handle_Update__State__GameState_Combat(Game_StateMachine gameWorld, double deltaTime)
        {

        }
    }
}
