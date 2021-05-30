using MonkeyDungeon_Core.GameFeatures.GameStates;
using System.Collections.Generic;
using System.Linq;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects
{
    public class GameEntity_StatusEffect_Manager
    {
        private readonly GameEntity Entity;

        //TODO: filter by detremental
        private List<GameEntity_StatusEffect> StatusEffects         = new List<GameEntity_StatusEffect>();
        public GameEntity_StatusEffect[] Get_StatusEffects          () => StatusEffects.ToArray();
        public void Add_StatusEffect                                (GameEntity_StatusEffect effect) { effect.Set_NewOwner(Entity); StatusEffects.Add(effect);  }
        public void Remove_StatusEffect                             (GameEntity_StatusEffect effect) { if (!StatusEffects.Contains(effect)) return; StatusEffects.Remove(effect); effect.Remove_Owner(); }
        public void Remove_All_StatusEffects                        () { foreach (GameEntity_StatusEffect effect in StatusEffects.ToList()) Remove_StatusEffect(effect); }
        public void Remove_All_StatusEffects_Except<T>              () where T : GameEntity_StatusEffect { foreach (GameEntity_StatusEffect effect in StatusEffects.ToList()) if (effect is T) Remove_StatusEffect(effect); }
        public void Disable_StatusEffects<T>                        () where T : GameEntity_StatusEffect { foreach (GameEntity_StatusEffect effect in StatusEffects.ToList()) if (effect is T) effect.Toggle_ThisEffect(false); }
        public void Disable_StatusEffect                            (GameEntity_StatusEffect effect) { foreach (GameEntity_StatusEffect subEffect in StatusEffects.ToList()) if (subEffect == effect) effect.Toggle_ThisEffect(false); }

        public GameEntity_StatusEffect_Manager(GameEntity managedEntity)
        {
            Entity = managedEntity;
        }

        internal void Combat_BeginTurn(Combat_GameState combat)
        {
            foreach (GameEntity_StatusEffect effect in StatusEffects)
                effect.Combat_BeginTurn_StatusEffect(combat);
        }
    }
}
