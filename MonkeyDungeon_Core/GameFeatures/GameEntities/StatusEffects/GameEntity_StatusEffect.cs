
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
        public GameEntity EffectedEntity { get; private set; }
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
        internal void Set_NewOwner(GameEntity target, bool enabled = true)
        {
            if (EffectedEntity != null)
                Handle_LoseOwner(EffectedEntity);
            EffectedEntity = target;
            Toggle_ThisEffect(enabled);
            Handle_NewOwner(target);
        }

        protected virtual void Handle_NewOwner(GameEntity newOwner) { }
        protected virtual void Handle_LoseOwner(GameEntity oldOwner) { }

        internal void Combat_BeginTurn_StatusEffect(GameEntity_EntityField gameField)
        {
            HandleCombat_BeginTurn_StatusEffect(gameField);
            ElapsedDuration++;
            if (TurnDuration > -1 && ElapsedDuration >= TurnDuration)
                EffectedEntity.StatusEffect_Manager.Remove_StatusEffect(this);
        }

        internal void Combat_EndTurn_StatusEffect(GameEntity_EntityField gameField)
        {
            HandleCombat_EndTurn_StatusEffect(gameField);
        }

        protected virtual void HandleCombat_BeginTurn_StatusEffect(GameEntity_EntityField gameField) { }
        protected virtual void HandleCombat_EndTurn_StatusEffect(GameEntity_EntityField gameField) { }
    }
}
