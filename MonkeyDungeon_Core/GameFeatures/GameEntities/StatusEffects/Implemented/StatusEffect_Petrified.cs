using MonkeyDungeon_Core.GameFeatures.GameStates;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects.Implemented
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
