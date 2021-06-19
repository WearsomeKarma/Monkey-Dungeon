using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.StatusEffects
{
    public sealed class GameEntity_StatusEffect_Manager
    {
        private readonly GameEntity_ServerSide ATTACHED_ENTITY;

        //TODO: filter by detremental
        private readonly List<GameEntity_ServerSide_StatusEffect> STATUSEFFECTS = new List<GameEntity_ServerSide_StatusEffect>();
        public GameEntity_ServerSide_StatusEffect[] Get__StatusEffects          () => STATUSEFFECTS.ToArray();
        public T Get__StatusEffect__GameEntity                                   <T>(GameEntity_Attribute_Name name) where T : GameEntity_ServerSide_StatusEffect { foreach(T statusEffect in STATUSEFFECTS) { if (statusEffect.Attribute_Name == name) return statusEffect; } return null; }
        public void Add__StatusEffect__GameEntity                                (GameEntity_ServerSide_StatusEffect effect) { effect.Attach_To__Entity__ServerSide_StatusEffect(ATTACHED_ENTITY); STATUSEFFECTS.Add(effect);  }
        public void Remove__StatusEffect__GameEntity                             (GameEntity_ServerSide_StatusEffect effect) { if (!STATUSEFFECTS.Contains(effect)) return; STATUSEFFECTS.Remove(effect); effect.Detach_From__Entity__ServerSide_StatusEffect(); }
        public void Remove_All__StatusEffects__GameEntity                        () { foreach (GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS.ToList()) Remove__StatusEffect__GameEntity(effect); }
        public void Remove_All__Except<T>              () where T : GameEntity_ServerSide_StatusEffect { foreach (GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS.ToList()) if (effect is T) Remove__StatusEffect__GameEntity(effect); }
        public void Disable__StatusEffects<T>                        () where T : GameEntity_ServerSide_StatusEffect { foreach (GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS.ToList()) if (effect is T) effect.Toggle__StatusEffect(false); }
        public void Disable__StatusEffect                            (GameEntity_ServerSide_StatusEffect effect) { foreach (GameEntity_ServerSide_StatusEffect subEffect in STATUSEFFECTS.ToList()) if (subEffect == effect) effect.Toggle__StatusEffect(false); }

        internal GameEntity_StatusEffect_Manager(GameEntity_ServerSide managedAttachedEntity)
        {
            ATTACHED_ENTITY = managedAttachedEntity;
        }

        internal void Combat_BeginTurn__StatusEffect_Manager()
        {
            foreach (GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS)
                effect.Combat_BeginTurn__StatusEffect();
        }

        internal void React_To_Cast()
        {
            foreach (GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS)
            {
                effect.React_To_Cast__StatusEffect();
            }
        }

        internal double Get_Hit_Bonuses__StatusEffect_Manager()
        {
            double ret = 0;

            foreach (GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS)
                ret += effect.Get_Hit_Bonus__StatusEffect();
            
            return ret;
        }

        internal double Get_Dodge_Bonuses__StatusEffect_Manager()
        {
            double ret = 0;

            foreach (GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS)
                ret += effect.Get_Dodge_Bonus__StatusEffect();
            
            return ret;
        }

        internal void React_To_Pre_Resource_Offset(GameEntity_Attribute_Name resource, double finalizedOffset)
        {
            foreach (GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS)
                effect.React_To_Pre_Resource_Offset__StatusEffect(resource, finalizedOffset);
        }

        internal void React_To_Post_Resource_Offset(GameEntity_Attribute_Name resource, double finalizedOffset)
        {
            foreach(GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS)
                effect.React_To_Post_Resource_Offset__Status_Effect(resource, finalizedOffset);
        }

        internal Combat_Redirection_Chance[] React_To_Redirect_Chance
            (
            Combat_Assault_Type assaultType, 
            GameEntity_Position_Type assaulterPositionType, 
            GameEntity_Position_Type targetPositionType, 
            Combat_Redirection_Chance baseChance
            )
        {
            List<Combat_Redirection_Chance> chancesFromStatusEffects = new List<Combat_Redirection_Chance>();
            Combat_Redirection_Chance chance;
            
            foreach (GameEntity_ServerSide_StatusEffect effect in STATUSEFFECTS)
            {
                chance = effect.React_To_Redirect_Chance__StatusEffect
                (
                    assaultType,
                    assaulterPositionType,
                    targetPositionType,
                    baseChance
                );
                
                if (chance != Combat_Redirection_Chance.NO_REDIRECT)
                    chancesFromStatusEffects.Add(chance);
            }

            return chancesFromStatusEffects.ToArray();
        }
    }
}
