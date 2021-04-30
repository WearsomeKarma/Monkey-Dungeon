using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;

namespace MonkeyDungeon.GameFeatures.Implemented.StatusEffects
{
    public class StatusEffect_Dead : StatusEffect
    {
        public StatusEffect_Dead() 
            : base(StatusEffectType.Dead, -1)
        {
        }

        protected override void HandleNewOwner(EntityComponent target)
        {
            target?.Remove_All_StatusEffects();
        }

        protected override void HandleCombat_BeginTurn_StatusEffect(Combat_GameState combat)
        {
            combat.Request_EndOfTurn();
        }
    }
}
