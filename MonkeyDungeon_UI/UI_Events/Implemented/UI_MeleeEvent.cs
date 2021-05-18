using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Tools;
using OpenTK;

namespace MonkeyDungeon_UI.UI_Events.Implemented
{
    public class UI_MeleeEvent : UI_GameEvent
    {
        public static readonly string EVENT_TAG = "ui_event_MeleeAttack";

        internal UI_MeleeEvent(EventScheduler eventScheduler, double duration, Vector3 allySide, Vector3 enemySide)
            : base(eventScheduler, "event_melee", duration)
        {

        }



        protected override void Callback_Reset(double newDuration)
        {
            base.Callback_Reset(newDuration);
        }

        protected override void Callback_DeltaTime(Timer timer)
        {
            base.Callback_DeltaTime(timer);
        }

        protected override void Callback_Elapsed()
        {
            base.Callback_Elapsed();
        }
    }
}
