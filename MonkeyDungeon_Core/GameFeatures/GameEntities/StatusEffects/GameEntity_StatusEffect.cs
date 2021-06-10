
using MonkeyDungeon_Core.GameFeatures.GameStates.Combat;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects
{
    public enum StatusEffectType
    {
        Feared,
        Stunned,
        Petrified,
        Dead
    }

    public class GameEntity_StatusEffect
    {
        public readonly StatusEffectType StatusEffectType;
        public int TurnDuration { get; private set; }
        public int ElapsedDuration { get; set; }
        public GameEntity_ServerSide EffectedEntityServerSide { get; private set; }
        public bool Enabled { get; private set; }

        public GameEntity_StatusEffect(StatusEffectType type, int duration)
        {
            StatusEffectType = type;
            TurnDuration = duration;
        }

        internal void Toggle_ThisEffect(bool state)
        {
            ToggleThisEffect((state) ? 1 : -1);
        }

        internal void Toggle_ThisEffect()
        {
            ToggleThisEffect(0);
        }

        private void ToggleThisEffect(int state = 0)
        {
            Enabled = (state == 0) ? !Enabled : (state > 0);
            HandleToggled(Enabled);
        }

        protected virtual void HandleToggled(bool enabledState)
        {

        }

        internal void Remove_Owner(bool enabled = false) => Set_NewOwner(null, enabled);
        internal void Set_NewOwner(GameEntity_ServerSide target, bool enabled = true)
        {
            if (EffectedEntityServerSide != null)
                Handle_LoseOwner(EffectedEntityServerSide);
            EffectedEntityServerSide = target;
            Toggle_ThisEffect(enabled);
            Handle_NewOwner(target);
        }

        protected virtual void Handle_NewOwner(GameEntity_ServerSide newOwner) { }
        protected virtual void Handle_LoseOwner(GameEntity_ServerSide oldOwner) { }

        internal void Combat_BeginTurn_StatusEffect(GameEntity_ServerSide_Roster gameField)
        {
            Handle_Combat_BeginTurn_StatusEffect(gameField);
            ElapsedDuration++;
            if (TurnDuration > -1 && ElapsedDuration >= TurnDuration)
                EffectedEntityServerSide.StatusEffect_Manager.Remove_StatusEffect(this);
        }

        internal void Combat_EndTurn_StatusEffect(GameEntity_ServerSide_Roster gameField)
            => Handle_Combat_EndTurn_StatusEffect(gameField);
        internal void React_To_Cast(Combat_Action action)
            => Handle_React_To_Cast(action);

        internal double Get_Hit_Bonus(Combat_Action action)
            => Handle_Get_Hit_Bonus(action);


        internal double Get_Dodge_Bonus(Combat_Action action)
            => Handle_Get_Dodge_Bonus(action);

        internal void React_To_Pre_Resource_Offset(GameEntity_Attribute_Name resource, double finalizedOffset)
            => Handle_Pre_Resource_Offset(resource, finalizedOffset);

        internal void React_To_Post_Resource_Offset(GameEntity_Attribute_Name resource, double finalizedOffset)
            => Handle_Post_Resource_Offset(resource, finalizedOffset);

        internal Combat_Redirection_Chance React_To_Redirect_Chance
        (
            Combat_Action action,
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type assaulterPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
        )
            => Handle_Redirection_Chance
                (
                action, 
                assaultType,
                assaulterPositionType,
                targetPositionType,
                baseChance
                );
        
        protected virtual void Handle_Combat_BeginTurn_StatusEffect(GameEntity_ServerSide_Roster gameField) { }
        protected virtual void Handle_Combat_EndTurn_StatusEffect(GameEntity_ServerSide_Roster gameField) { }
        protected  virtual void Handle_React_To_Cast(Combat_Action action) { }
        protected virtual double Handle_Get_Hit_Bonus(Combat_Action action) => 0;
        protected virtual double Handle_Get_Dodge_Bonus(Combat_Action action) => 0;
        protected virtual void Handle_Pre_Resource_Offset(GameEntity_Attribute_Name resource, double finalizedOffset) { }
        protected virtual void Handle_Post_Resource_Offset(GameEntity_Attribute_Name resource, double finalizedOffset) { }

        protected virtual Combat_Redirection_Chance Handle_Redirection_Chance
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
