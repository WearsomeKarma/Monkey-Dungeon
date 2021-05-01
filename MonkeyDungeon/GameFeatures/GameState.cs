using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
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
        internal GameWorld_StateMachine GameWorld { get; private set; }
        internal void SetGameWorld(GameWorld_StateMachine gameWorld) => GameWorld = gameWorld;
        public TransitionState TransitionState { get; private set; }

        public event Action StateConcluded;
        public event Action StateBegun;

        public GameState(Action stateBegun, Action stateConcluded)
        {
            StateBegun = stateBegun;
            StateConcluded = stateConcluded;
            TransitionState = TransitionState.Awaiting;
        }

        internal void Reset(GameWorld_StateMachine gameWorld)
        {
            Handle_ResetState(gameWorld);
            TransitionState = TransitionState.Awaiting;
        }

        internal void Begin(GameWorld_StateMachine gameWorld)
        {
            TransitionState = TransitionState.Beginning;
            Handle_BeginState(gameWorld);
            StateBegun?.Invoke();
            TransitionState = TransitionState.Acting;
        }

        internal void End(GameWorld_StateMachine gameWorld)
        {
            TransitionState = TransitionState.Ending;
            Handle_EndState(gameWorld);
            StateConcluded?.Invoke();
            TransitionState = TransitionState.Finished;
        }

        internal void UpdateState(GameWorld_StateMachine gameWorld)
            => Handle_UpdateState(gameWorld);

        protected void End() => TransitionState = TransitionState.Ending;

        protected virtual void Handle_ResetState(GameWorld_StateMachine gameWorld) { }
        protected virtual void Handle_BeginState(GameWorld_StateMachine gameWorld) { }
        protected virtual void Handle_EndState(GameWorld_StateMachine gameWorld) { }
        protected virtual void Handle_UpdateState(GameWorld_StateMachine gameWorld) { }
    }
}
