using MonkeyDungeon_Vanilla_Domain.GameFeatures;

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

        internal void Attach_To__Entity__ServerSide_StatusEffect(GameEntity_ServerSide entity)
            => Attach_To__Entity__Attribute(entity);
        internal void Detach_From__Entity__ServerSide_StatusEffect()
            => Detach_From__Entity__Attribute();
    }
}