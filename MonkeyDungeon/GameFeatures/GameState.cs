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
        internal GameState_Machine GameWorld { get; private set; }
        internal void SetGameWorld(GameState_Machine gameWorld) => GameWorld = gameWorld;
        public TransitionState TransitionState { get; private set; }

        public event Action StateConcluded;
        public event Action StateBegun;

        public GameState(Action stateBegun, Action stateConcluded)
        {
            StateBegun = stateBegun;
            StateConcluded = stateConcluded;
            TransitionState = TransitionState.Awaiting;
        }

        internal void Reset(GameState_Machine gameWorld)
        {
            Handle_ResetState(gameWorld);
            TransitionState = TransitionState.Awaiting;
        }

        internal void Begin(GameState_Machine gameWorld)
        {
            TransitionState = TransitionState.Beginning;
            Handle_BeginState(gameWorld);
            StateBegun?.Invoke();
            TransitionState = TransitionState.Acting;
        }

        internal void End(GameState_Machine gameWorld)
        {
            TransitionState = TransitionState.Ending;
            Handle_EndState(gameWorld);
            StateConcluded?.Invoke();
            TransitionState = TransitionState.Finished;
        }

        internal void UpdateState(GameState_Machine gameWorld)
            => Handle_UpdateState(gameWorld);

        protected void End() => TransitionState = TransitionState.Ending;

        protected virtual void Handle_ResetState(GameState_Machine gameWorld) { }
        protected virtual void Handle_BeginState(GameState_Machine gameWorld) { }
        protected virtual void Handle_EndState(GameState_Machine gameWorld) { }
        protected virtual void Handle_UpdateState(GameState_Machine gameWorld) { }
    }
}
