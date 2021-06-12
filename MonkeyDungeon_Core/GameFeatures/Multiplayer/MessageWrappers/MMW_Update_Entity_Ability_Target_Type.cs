using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Update_Entity_Ability_Target_Type : Multiplayer_Message_Wrapper
    {
        public MMW_Update_Entity_Ability_Target_Type(GameEntity_ID entityId, Combat_Target_Type targetType,
            GameEntity_Attribute_Name abilityName)
            : base(MD_VANILLA_MMH.MMH_UPDATE_ENTITY_ABILITY_TARGET_TYPE, entityId, 0, (int) targetType, abilityName)
        { }
    }
}