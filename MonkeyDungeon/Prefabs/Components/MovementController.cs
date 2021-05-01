using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Tools;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Prefabs.Components
{
    public class MovementController : EventComponent
    {
        private Vector3 resetPosition;
        private Vector3 targetPosition;
        private Vector3 targetOffset;
        private Vector3 resetOffset;
        private double rate;
        public MovementController(Vector3 targetPosition, double time)
            : base(time)
        {
            this.targetPosition = targetPosition;
            EventTimer.Reset(time);
            rate = time / 3;
        }

        protected override void Handle_NewParent()
        {
            Set_InitalPosition(ParentObject.Position);
        }

        internal void Set_InitalPosition(Vector3 pos)
        {
            resetPosition = pos;
            Update_Position();
        }
        
        internal void Set_TargetPosition(Vector3 pos)
        {
            targetPosition = pos;
            Update_Position();
        }
        
        private void Update_Position()
        {
            targetOffset = targetPosition - resetPosition;
            resetOffset = resetPosition - targetPosition;
        }

        internal void _reset_Movement(double newLimit)
            => ResetComponent(newLimit);
        protected override void ResetComponent(double newLimit)
        {
            ResetPosition();
            rate = newLimit / 3;
        }

        internal void _progress_Movement(Timer timer)
            => PerformFrame(timer);
        protected override void PerformFrame(Timer timer)
        {
            Vector3 offset = new Vector3();
            if (timer.TimeElapsed < timer.TimeLimit / 3)
                offset = targetOffset * (float)(1/rate * timer.Frame_DeltaTime);
            else if (timer.TimeElapsed > timer.TimeLimit * 2 / 3)
                offset = resetOffset * (float)(1/rate * timer.Frame_DeltaTime);
            else
                return;
            ParentObject.Position += offset;
        }

        internal void _finish_Movement()
            => FinishComponent();
        protected override void FinishComponent()
        {
            ResetPosition();
        }

        private void ResetPosition()
        {
            if (ParentObject != null)
                ParentObject.Position = resetPosition;
        }
    }
}
