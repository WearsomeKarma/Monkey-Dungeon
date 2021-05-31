using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using OpenTK;

namespace MonkeyDungeon_UI.UI_Events.Implemented
{
    public class UI_Ranged_Attack : UI_Panning_Event
    {
        private Particle Ranged_Particle { get; set; }

        internal UI_Ranged_Attack(EventScheduler eventScheduler, Particle rangedParticle, Vector3 initalPos, double duration = 1) 
            : base(eventScheduler, MD_VANILLA_UI.UI_EVENT_RANGED_ATTACK, rangedParticle, initalPos, duration)
        {
            Ranged_Particle = rangedParticle;
        }

        protected override void Callback_Reset(double newDuration)
        {
            Ranged_Particle.Position = Inital_Position;
            Ranged_Particle.Toggle_Sprite(true);
        }

        protected override void Callback_Elapsed()
        {
            Ranged_Particle.Toggle_Sprite(false);
        }
    }
}
