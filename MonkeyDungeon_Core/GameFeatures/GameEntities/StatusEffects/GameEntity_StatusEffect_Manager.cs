using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects
{
    public class GameEntity_StatusEffect_Manager
    {
        private readonly GameEntity_ServerSide _entityServerSide;

        //TODO: filter by detremental
        private List<GameEntity_StatusEffect> StatusEffects         = new List<GameEntity_StatusEffect>();
        public GameEntity_StatusEffect[] Get_StatusEffects          () => StatusEffects.ToArray();
        public void Add_StatusEffect                                (GameEntity_StatusEffect effect) { effect.Set_NewOwner(_entityServerSide); StatusEffects.Add(effect);  }
        public void Remove_StatusEffect                             (GameEntity_StatusEffect effect) { if (!StatusEffects.Contains(effect)) return; StatusEffects.Remove(effect); effect.Remove_Owner(); }
        public void Remove_All_StatusEffects                        () { foreach (GameEntity_StatusEffect effect in StatusEffects.ToList()) Remove_StatusEffect(effect); }
        public void Remove_All_StatusEffects_Except<T>              () where T : GameEntity_StatusEffect { foreach (GameEntity_StatusEffect effect in StatusEffects.ToList()) if (effect is T) Remove_StatusEffect(effect); }
        public void Disable_StatusEffects<T>                        () where T : GameEntity_StatusEffect { foreach (GameEntity_StatusEffect effect in StatusEffects.ToList()) if (effect is T) effect.Toggle_ThisEffect(false); }
        public void Disable_StatusEffect                            (GameEntity_StatusEffect effect) { foreach (GameEntity_StatusEffect subEffect in StatusEffects.ToList()) if (subEffect == effect) effect.Toggle_ThisEffect(false); }

        public GameEntity_StatusEffect_Manager(GameEntity_ServerSide managedEntityServerSide)
        {
            _entityServerSide = managedEntityServerSide;
        }

        internal void Combat_BeginTurn(GameEntity_ServerSide_Roster gameField)
        {
            foreach (GameEntity_StatusEffect effect in StatusEffects)
                effect.Combat_BeginTurn_StatusEffect(gameField);
        }

        internal void React_To_Cast(Combat_Action action)
        {
            foreach (GameEntity_StatusEffect effect in StatusEffects)
            {
                effect.React_To_Cast(action);
            }
        }

        internal double Get_Hit_Bonuses(Combat_Action action)
        {
            double ret = 0;

            foreach (GameEntity_StatusEffect effect in StatusEffects)
                ret += effect.Get_Hit_Bonus(action);
            
            return ret;
        }

        internal double Get_Dodge_Bonuses(Combat_Action action)
        {
            double ret = 0;

            foreach (GameEntity_StatusEffect effect in StatusEffects)
                ret += effect.Get_Dodge_Bonus(action);
            
            return ret;
        }

        internal void React_To_Pre_Resource_Offset(GameEntity_Attribute_Name resource, double finalizedOffset)
        {
            foreach (GameEntity_StatusEffect effect in StatusEffects)
                effect.React_To_Pre_Resource_Offset(resource, finalizedOffset);
        }

        internal void React_To_Post_Resource_Offset(GameEntity_Attribute_Name resource, double finalizedOffset)
        {
            foreach(GameEntity_StatusEffect effect in StatusEffects)
                effect.React_To_Post_Resource_Offset(resource, finalizedOffset);
        }

        internal Combat_Redirection_Chance[] React_To_Redirect_Chance
            (
            Combat_Action action, 
            Combat_Assault_Type assaultType, 
            GameEntity_Position_Type assaulterPositionType, 
            GameEntity_Position_Type targetPositionType, 
            Combat_Redirection_Chance baseChance
            )
        {
            List<Combat_Redirection_Chance> chancesFromStatusEffects = new List<Combat_Redirection_Chance>();
            Combat_Redirection_Chance chance;
            
            foreach (GameEntity_StatusEffect effect in StatusEffects)
            {
                chance = effect.React_To_Redirect_Chance
                (
                    action,
                    assaultType,
                    assaulterPositionType,
                    targetPositionType,
                    baseChance
                );
                
                if (chance != MD_VANILLA_COMBAT.NO_REDIRECT)
                    chancesFromStatusEffects.Add(chance);
            }

            return chancesFromStatusEffects.ToArray();
        }
    }
}
