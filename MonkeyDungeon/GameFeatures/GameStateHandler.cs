using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public enum TransitionState
    {
        Beginning,      //Start up the state
        Acting,     //Play out the state
        Ending,     //Cleanup the state
        Finished    //Wait to be put on standby for next state.
    }

    public class GameStateHandler
    {
        internal GameWorld_StateMachine GameWorld { get; private set; }
        internal void SetGameWorld(GameWorld_StateMachine gameWorld) => GameWorld = gameWorld;
        public TransitionState TransitionState { get; private set; }

        public event Action StateConcluded;
        public event Action StateBegun;

        public GameStateHandler(Action stateBegun, Action stateConcluded)
        {
            StateBegun = stateBegun;
            StateConcluded = stateConcluded;
        }

        internal void Begin(GameWorld_StateMachine gameWorld)
        {
            TransitionState = TransitionState.Beginning;
            BeginState(gameWorld);
            StateBegun?.Invoke();
            TransitionState = TransitionState.Acting;
        }

        internal void End(GameWorld_StateMachine gameWorld)
        {
            TransitionState = TransitionState.Ending;
            EndState(gameWorld);
            StateConcluded?.Invoke();
            TransitionState = TransitionState.Finished;
        }

        internal void UpdateState(GameWorld_StateMachine gameWorld, double deltaTime)
            => HandleUpdateState(gameWorld, deltaTime);

        protected void End() => TransitionState = TransitionState.Ending;

        protected virtual void BeginState(GameWorld_StateMachine gameWorld) { }
        protected virtual void EndState(GameWorld_StateMachine gameWorld) { }
        protected virtual void HandleUpdateState(GameWorld_StateMachine gameWorld, double deltaTime) { }
    }
}
