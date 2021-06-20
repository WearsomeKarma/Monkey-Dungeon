using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.StatusEffects
{
    public class GameEntity_ServerSide_StatusEffect : GameEntity_StatusEffect<GameEntity_ServerSide>
    {
        public GameEntity_ServerSide_StatusEffect
            (
            GameEntity_Attribute_Name statusEffectTag, 
            int duration
            ) 
            : base
                (
                statusEffectTag, 
                duration
                )
        {
        }

        internal void Toggle__StatusEffect__ServerSide_StatusEffect(bool state)
            => Handle__Toggled__StatusEffect(state);
        
        internal void Begin__Combat_Turn__ServerSide_StatusEffect()
            => Begin__Combat_Turn__StatusEffect();

        internal void React_To__Cast__ServerSide_StatusEffect()
            => Handle_React_To__Cast__StatusEffect();

        internal double Get__Hit_Bonus__ServerSide_StatusEffect()
            => Handle_Get__Hit_Bonus__StatusEffect();

        internal Combat_Redirection_Chance React_To__Redirect_Chance__ServerSide_StatusEffect
            (
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type assaulterPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
            )
            => Handle_React_To__Redirection_Chance__StatusEffect
            (
                assaultType,
                assaulterPositionType,
                targetPositionType,
                baseChance
            );
        
        internal double Get__Dodge_Bonus__ServerSide_StatusEffect()
            => Handle_Get__Dodge_Bonus__StatusEffect();

        internal void React_To__Pre_Resource_Offset__ServerSide_StatusEffect
            (
            GameEntity_Attribute_Name affectedResource,
            double quantity
            )
            => Handle_React_To__Pre_Resource_Offset__StatusEffect(affectedResource, quantity);

        internal void React_To__Post_Resource_Offset__ServerSide_Status_Effect
            (
            GameEntity_Attribute_Name affectedResource,
            double quantity
            )
            => Handle_React_To__Post_Resource_Offset__StatusEffect(affectedResource, quantity);
        
        internal void Attach_To__Entity__ServerSide_StatusEffect(GameEntity_ServerSide entity)
            => Attach_To__Entity__Attribute(entity);
        internal void Detach_From__Entity__ServerSide_StatusEffect()
            => Detach_From__Entity__Attribute();
    }
}