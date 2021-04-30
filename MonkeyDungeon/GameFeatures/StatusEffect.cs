using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public enum StatusEffectType
    {
        Feared,
        Stunned,
        Petrified,
        Dead
    }

    public class StatusEffect
    {
        public readonly StatusEffectType StatusEffectType;
        public int TurnDuration { get; private set; }
        public int ElapsedDuration { get; set; }
        public EntityComponent EffectedEntity { get; private set; }
        public bool Enabled { get; private set; }

        public StatusEffect(StatusEffectType type, int duration)
        {
            StatusEffectType = type;
            TurnDuration = duration;
        }

        internal void MakeOwnerLess()
        {
            EffectedEntity = null;
            ToggleThisEffect(false);
        }

        internal void ToggleThisEffect(bool state)
        {
            ToggleThisEffect((state) ? 1 : -1);
        }

        internal void ToggleThisEffect()
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

        internal void GiveThisEffect_NewOwner(EntityComponent target, bool enabled = true)
        {
            EntityComponent oldOwner = EffectedEntity;
            EffectedEntity = target;
            if (enabled != Enabled)
                ToggleThisEffect(enabled);
            HandleNewOwner(oldOwner);
        }

        protected virtual void HandleNewOwner(EntityComponent oldOwner)
        {

        }

        internal void Combat_BeginTurn_StatusEffect(Combat_GameState combat)
        {
            HandleCombat_BeginTurn_StatusEffect(combat);
            ElapsedDuration++;
            if (TurnDuration > -1 && ElapsedDuration >= TurnDuration)
                EffectedEntity.Remove_StatusEffect(this);
        }

        internal void Combat_EndTurn_StatusEffect(Combat_GameState combat)
        {
            HandleCombat_EndTurn_StatusEffect(combat);
        }

        protected virtual void HandleCombat_BeginTurn_StatusEffect(Combat_GameState combat) { }
        protected virtual void HandleCombat_EndTurn_StatusEffect(Combat_GameState combat) { }
    }
}
