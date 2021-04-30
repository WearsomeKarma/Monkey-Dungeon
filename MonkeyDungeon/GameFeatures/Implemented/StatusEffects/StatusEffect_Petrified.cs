using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;

namespace MonkeyDungeon.GameFeatures.Implemented.StatusEffects
{
    public class StatusEffect_Petrified : StatusEffect
    {
        public StatusEffect_Petrified(int duration) 
            : base(StatusEffectType.Petrified, duration)
        {
        }

        protected override void HandleCombat_BeginTurn_StatusEffect(Combat_GameState combat)
        {
            combat.Request_EndOfTurn();
        }
    }
}
