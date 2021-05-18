using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.StatusEffects
{
    public class StatusEffect_Petrified : GameEntity_StatusEffect
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
