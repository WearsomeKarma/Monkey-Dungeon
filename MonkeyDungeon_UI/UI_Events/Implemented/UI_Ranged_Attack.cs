using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using OpenTK;

namespace MonkeyDungeon_UI.UI_Events.Implemented
{
    public class UI_Ranged_Attack : UI_Panning_Event
    {
        private UI_ParticleObject Ranged_UiParticleObject { get; set; }

        internal UI_Ranged_Attack(EventScheduler eventScheduler, UI_ParticleObject rangedUiParticleObject, Vector3 initalPos, double duration = 1) 
            : base(eventScheduler, MD_VANILLA_UI_EVENT_NAMES.UI_EVENT_RANGED_ATTACK, rangedUiParticleObject, initalPos, duration)
        {
            Ranged_UiParticleObject = rangedUiParticleObject;
        }

        protected override void Callback_Reset(double newDuration)
        {
            Ranged_UiParticleObject.Position = Inital_Position;
            Ranged_UiParticleObject.Toggle_Sprite(true);
        }

        protected override void Callback_Elapsed()
        {
            Ranged_UiParticleObject.Toggle_Sprite(false);
        }
    }
}
