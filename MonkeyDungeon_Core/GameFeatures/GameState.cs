using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures
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
        public GameState_Machine GameState_Machine { get; private set; }
        internal void Set_GameWorld(GameState_Machine gameWorld)
        {
            GameState_Machine = gameWorld;
            Handle_AcquiredWorld();
        }
        protected virtual void Handle_AcquiredWorld() { }
        public TransitionState TransitionState { get; private set; }
        
        //TODO: Add relays.
        //protected UI_Relay UI_Relay { get; private set; }
        //internal virtual void Set_UI_Relay(UI_Relay ui_relay) => UI_Relay = ui_relay;

        public GameState()
        {
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
            Handle_Begin_State(gameWorld);
            TransitionState = TransitionState.Acting;
        }

        internal void End(GameState_Machine gameWorld)
        {
            TransitionState = TransitionState.Ending;
            Handle_End_State(gameWorld);
            TransitionState = TransitionState.Finished;
        }

        internal void UpdateState(GameState_Machine gameWorld, double deltaTime=0)
        {
            Handle_Update_State(gameWorld, deltaTime);
        }

        protected void End() => TransitionState = TransitionState.Ending;

        protected virtual void Handle_ResetState(GameState_Machine gameWorld)
        {

        }
        protected virtual void Handle_Begin_State(GameState_Machine gameWorld)
        {

        }
        protected virtual void Handle_End_State(GameState_Machine gameWorld)
        {

        }
        protected virtual void Handle_Update_State(GameState_Machine gameWorld, double deltaTime)
        {

        }
    }
}
