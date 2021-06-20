using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_StatusEffect<T> : GameEntity_Attribute<T> where T : GameEntity
    {
        public int StatusEffect_TurnDuration { get; private set; }
        public int StatusEffect_Elapsed_TurnDuration { get; set; }
        public bool Is__StatusEffect_Enabled { get; private set; }

        public GameEntity_StatusEffect(GameEntity_Attribute_Name statusEffectTag, int duration)
            : base
                (
                statusEffectTag
                )
        {
            StatusEffect_TurnDuration = duration;
        }
        protected virtual void Handle__Toggled__StatusEffect(bool state)
        {
            Is__StatusEffect_Enabled = state;
            Handle__Toggled__StatusEffect(state);
        }

        protected void Begin__Combat_Turn__StatusEffect()
        {
            Handle_Begin__Combat_Turn__StatusEffect();
            StatusEffect_Elapsed_TurnDuration++;
            if (StatusEffect_TurnDuration > -1 && StatusEffect_Elapsed_TurnDuration >= StatusEffect_TurnDuration)
                throw new NotImplementedException(); //TODO: Trello [6/19/2021] prim wrap duration
        }
        
        protected virtual void Handle_Begin__Combat_Turn__StatusEffect() { }
        protected virtual void Handle_Conclude__Combat_Turn__StatusEffect() { }
        protected  virtual void Handle_React_To__Cast__StatusEffect() { }
        protected virtual double Handle_Get__Hit_Bonus__StatusEffect() => 0;
        protected virtual double Handle_Get__Dodge_Bonus__StatusEffect() => 0;
        protected virtual void Handle_React_To__Pre_Resource_Offset__StatusEffect(GameEntity_Attribute_Name resource, double finalizedOffset) { }
        protected virtual void Handle_React_To__Post_Resource_Offset__StatusEffect(GameEntity_Attribute_Name resource, double finalizedOffset) { }

        protected virtual Combat_Redirection_Chance Handle_React_To__Redirection_Chance__StatusEffect
        (
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type assaulterPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
        )
            => Combat_Redirection_Chance.NO_REDIRECT;
    }
}
