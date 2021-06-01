using MonkeyDungeon_Core.GameFeatures.GameStates;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects.Implemented
{
    public class StatusEffect_Dead : GameEntity_StatusEffect
    {
        public StatusEffect_Dead() 
            : base(StatusEffectType.Dead, -1)
        {
        }

        protected override void Handle_LoseOwner(GameEntity oldOwner)
        {
            oldOwner?.StatusEffect_Manager.Remove_StatusEffect(this);
        }

        protected override void Handle_NewOwner(GameEntity target)
        {
            target?.StatusEffect_Manager.Remove_All_StatusEffects();
            target?.Set_IncapacitatedState();
        }

        protected override void Handle_Combat_BeginTurn_StatusEffect(GameEntity_EntityField gameField)
        {
            //combat.Request_EndOfTurn();
            throw new NotImplementedException(); //TODO: resolve above.
        }
    }
}
