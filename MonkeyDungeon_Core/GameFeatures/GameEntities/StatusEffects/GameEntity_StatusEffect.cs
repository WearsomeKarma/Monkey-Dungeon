
using MonkeyDungeon_Core.GameFeatures.GameStates.Combat;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects
{
    public class GameEntity_StatusEffect : GameEntity_Attribute
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

        internal void Toggle__StatusEffect(bool state)
        {
            Is__StatusEffect_Enabled = state;
            Handle_Toggled__StatusEffect(state);
        }

        protected virtual void Handle_Toggled__StatusEffect(bool enabledState)
        {

        }

        internal void Combat_BeginTurn__StatusEffect()
        {
            Handle_Combat_BeginTurn__StatusEffect();
            StatusEffect_Elapsed_TurnDuration++;
            if (StatusEffect_TurnDuration > -1 && StatusEffect_Elapsed_TurnDuration >= StatusEffect_TurnDuration)
                Parent_EntityServerSide.Remove__GameEntity_StatusEffect(this);
        }

        internal void Combat_EndTurn__StatusEffect()
            => Handle_Combat_EndTurn__StatusEffect();

        internal void React_To_Cast__StatusEffect(Combat_Action action)
            => Handle_React_To_Cast__StatusEffect(action);

        internal double Get_Hit_Bonus__StatusEffect(Combat_Action action)
            => Handle_Get_Hit_Bonus__StatusEffect(action);


        internal double Get_Dodge_Bonus__StatusEffect(Combat_Action action)
            => Handle_Get_Dodge_Bonus__StatusEffect(action);

        internal void React_To_Pre_Resource_Offset__StatusEffect(GameEntity_Attribute_Name resource, double finalizedOffset)
            => Handle_Pre_Resource_Offset__StatusEffect(resource, finalizedOffset);

        internal void React_To_Post_Resource_Offset__Status_Effect(GameEntity_Attribute_Name resource, double finalizedOffset)
            => Handle_Post_Resource_Offset__StatusEffect(resource, finalizedOffset);

        internal Combat_Redirection_Chance React_To_Redirect_Chance__StatusEffect
        (
            Combat_Action action,
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type assaulterPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
        )
            => Handle_Redirection_Chance__StatusEffect
                (
                action, 
                assaultType,
                assaulterPositionType,
                targetPositionType,
                baseChance
                );
        
        protected virtual void Handle_Combat_BeginTurn__StatusEffect() { }
        protected virtual void Handle_Combat_EndTurn__StatusEffect() { }
        protected  virtual void Handle_React_To_Cast__StatusEffect(Combat_Action action) { }
        protected virtual double Handle_Get_Hit_Bonus__StatusEffect(Combat_Action action) => 0;
        protected virtual double Handle_Get_Dodge_Bonus__StatusEffect(Combat_Action action) => 0;
        protected virtual void Handle_Pre_Resource_Offset__StatusEffect(GameEntity_Attribute_Name resource, double finalizedOffset) { }
        protected virtual void Handle_Post_Resource_Offset__StatusEffect(GameEntity_Attribute_Name resource, double finalizedOffset) { }

        protected virtual Combat_Redirection_Chance Handle_Redirection_Chance__StatusEffect
        (
            Combat_Action action,
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type assaulterPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
        )
            => MD_VANILLA_COMBAT.NO_REDIRECT;
    }
}
