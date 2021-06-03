using isometricgame.GameEngine;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using OpenTK;

namespace MonkeyDungeon_UI.UI_Events.Implemented
{
    public class UI_Announcement_Event : UI_Panning_Event
    {
        internal UI_Announcement_Event(EventScheduler eventScheduler, GameObject gameObject, Vector3 initalPos, Vector3 targetPos, double duration = 1) 
            : base(eventScheduler, MD_VANILLA_UI.UI_EVENT_ANNOUNCEMENT, gameObject, initalPos, duration)
        {
            Target_Position = targetPos;
        }

        protected override void Callback_DeltaTime(Timer timer)
        {
            float delta = (float)timer.Frame_DeltaTime;
            float duration = (float)Duration;
            
            if (timer.TimeElapsed < (Duration / 3))
            {
                Pan_From_Position(GameObject, Inital_Position, Target_Position, delta, duration / 3);
                return;
            }
            else if (timer.TimeElapsed < (Duration * 2 / 3))
                return;
            
            Pan_From_Position(GameObject, Target_Position, Inital_Position, delta, duration / 3);
        }
    }
}
