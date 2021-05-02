using isometricgame.GameEngine.Tools;
using MonkeyDungeon.Prefabs.Components;
using MonkeyDungeon.Prefabs.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Events.Implemented
{
    public class MeleeEvent : GameEvent
    {
        public static readonly string EVENT_TAG = "event_meleeAttack";
        public static readonly Vector3 POSITION_MELEE = new Vector3(20, 20, 0);

        private MovementController AllySide_Entity { get; set; }
        private MovementController EnemySide_Entity { get; set; }

        public MeleeEvent(double eventDuration = 1) 
            : base(eventDuration)
        {
        }

        public void Set_Combattants(
            MovementController allySide,
            MovementController enemySide
            )
        {
            AllySide_Entity = allySide;
            EnemySide_Entity = enemySide;

            AllySide_Entity.Set_TargetPosition(-POSITION_MELEE);
            EnemySide_Entity.Set_TargetPosition(POSITION_MELEE);
        }

        protected override void ResetEvent(double newTime)
        {
            AllySide_Entity._reset_Movement(newTime);
            EnemySide_Entity._reset_Movement(newTime);
        }

        protected override void Progress_Event(Timer timer)
        {
            AllySide_Entity._progress_Movement(timer);
            EnemySide_Entity._progress_Movement(timer);
        }

        protected override void FinishEvent()
        {
            AllySide_Entity._finish_Movement();
            EnemySide_Entity._finish_Movement();
        }
    }
}
