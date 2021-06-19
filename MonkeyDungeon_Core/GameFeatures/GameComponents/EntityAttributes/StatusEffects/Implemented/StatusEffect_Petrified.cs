using MonkeyDungeon_Core.GameFeatures.GameStates;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.StatusEffects.Implemented
{
    public class StatusEffect_Petrified : GameEntity_ServerSide_StatusEffect
    {
        public StatusEffect_Petrified(int duration) 
            : base(MD_VANILLA_STATUSEFFECT_NAMES.STATUSEFFECT_PETRIFIED, duration)
        {
        }

        protected override void Handle_Combat_BeginTurn__StatusEffect()
        {
            //combat.Request_EndOfTurn();
            throw new NotImplementedException(); //TODO: resolve above.
        }
    }
}
