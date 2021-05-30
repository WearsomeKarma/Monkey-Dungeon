using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Dismiss_Entity : Multiplayer_Message_Wrapper
    {
        public MMW_Dismiss_Entity(GameEntity_ID entityId = null)
            : base(MD_VANILLA_MMH.MMH_DISMISS_ENTITY, entityId ?? GameEntity_ID.ID_ZERO)
        {
        }
    }
}
