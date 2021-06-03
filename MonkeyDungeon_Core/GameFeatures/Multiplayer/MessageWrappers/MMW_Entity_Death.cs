using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Entity_Death : Multiplayer_Message_Wrapper
    {
        public MMW_Entity_Death(GameEntity_ID entityId = null)
            : base(MD_VANILLA_MMH.MMH_ENTITY_DEATH, entityId ?? GameEntity_ID.ID_ZERO)
        {
        }
    }
}
