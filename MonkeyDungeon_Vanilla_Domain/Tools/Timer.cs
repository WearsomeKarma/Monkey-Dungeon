using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Tools
{
    public class Timer
    {
        public bool IsFinished => TimeElapsed >= Duration;
        public bool Active { get; private set; }
        public void Set(double duration=-1)
        {
            Duration = (duration > 0) ? duration : defaultTime;
            TimeElapsed = 0;
            Active = true;
        }

        public double TimeElapsed { get; private set; }
        public double Duration { get; set; }
        private double defaultTime;

        public Timer(double defaultTime)
        {
            this.defaultTime = defaultTime;
        }

        public void Progress_DeltaTime(double deltaTime)
        {
            if (Active)
                TimeElapsed += deltaTime;
            if (IsFinished)
                Active = false;
        }
    }
}
