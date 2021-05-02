using isometricgame.GameEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Events
{
    public class GameEvent
    {
        internal TimedCallback EventTimer { get; set; }

        public GameEvent(double eventDuration = 1)
        {
            EventTimer = new TimedCallback(
                new Timer(eventDuration),
                Progress_Event,
                FinishEvent,
                ResetEvent
                );
        }

        protected virtual void ResetEvent(double newTime) { }
        protected virtual void Progress_Event(Timer timer) { }
        protected virtual void FinishEvent() { }

        public static implicit operator TimedCallback(GameEvent e) => e.EventTimer;
    }
}
