using MonkeyDungeon_Core.GameFeatures.GameStates;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.StatusEffects.Implemented
{
    public class StatusEffect_Dead : GameEntity_ServerSide_StatusEffect
    {
        public StatusEffect_Dead() 
            : base(MD_VANILLA_STATUSEFFECT_NAMES.STATUSEFFECT_DEAD, -1)
        {
        }

        protected override void Handle_Attach_To_Entity(GameEntity_ServerSide newEntityServerSide)
        {
            newEntityServerSide.Remove_All__StatusEffects__GameEntity();
            newEntityServerSide.Set_Incapacitated_State(true);
        }

        protected override void Handle_Detach_From_Entity(GameEntity_ServerSide oldEntityServerSide)
        {
            oldEntityServerSide.Remove__GameEntity_StatusEffect(this);
        }

        protected override void Handle_Combat_BeginTurn__StatusEffect()
        {
            //combat.Request_EndOfTurn();
            throw new NotImplementedException(); //TODO: resolve above.
        }
    }
}
