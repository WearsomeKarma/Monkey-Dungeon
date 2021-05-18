using isometricgame.GameEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.UI_Events
{
    public class UI_GameEvent
    {
        private readonly TimedCallback Callback;

        internal UI_GameEvent(EventScheduler eventScheduler, string eventTag, double duration=1)
        {
            Callback = new TimedCallback(eventTag, duration, Callback_DeltaTime, Callback_Elapsed, Callback_Reset, eventScheduler);
        }

        protected virtual void Callback_DeltaTime(Timer timer) { }
        protected virtual void Callback_Elapsed() { }
        protected virtual void Callback_Reset(double newDuration) { }

        public static implicit operator TimedCallback(UI_GameEvent e) => e.Callback;
    }
}
