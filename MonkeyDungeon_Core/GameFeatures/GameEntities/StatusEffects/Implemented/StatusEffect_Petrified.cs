using MonkeyDungeon_Core.GameFeatures.GameStates;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects.Implemented
{
    public class StatusEffect_Petrified : GameEntity_StatusEffect
    {
        public StatusEffect_Petrified(int duration) 
            : base(StatusEffectType.Petrified, duration)
        {
        }

        protected override void Handle_Combat_BeginTurn_StatusEffect(GameEntity_EntityField gameField)
        {
            //combat.Request_EndOfTurn();
            throw new NotImplementedException(); //TODO: resovle above.
        }
    }
}
