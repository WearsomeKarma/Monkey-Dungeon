using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using OpenTK;

namespace MonkeyDungeon_UI.UI_Events.Implemented
{
    public class UI_Panning_Event : UI_GameEvent
    {
        protected GameObject GameObject { get; set; }
        internal Vector3 Inital_Position { get; set; }
        internal Vector3 Target_Position { get; set; }

        internal UI_Panning_Event(EventScheduler eventScheduler, string eventTag, GameObject gameObject, Vector3 initalPos, double duration = 1) 
            : base(eventScheduler, eventTag, duration)
        {
            GameObject = gameObject;
            Inital_Position = initalPos;
        }

        protected override void Callback_DeltaTime(Timer timer)
        {
            float delta = (float)timer.Frame_DeltaTime;
            float duration = (float)Duration;

            Pan_From_Position(GameObject, Inital_Position, Target_Position, delta, duration);
        }

        protected override void Callback_Elapsed()
        {
            GameObject.Position = Inital_Position;
        }
    }
}
