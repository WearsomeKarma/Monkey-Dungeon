using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Introduce_Entity : Multiplayer_Message_Wrapper
    {
        public MMW_Introduce_Entity(GameEntity_ID entityId) 
            : base(MD_VANILLA_MMH.MMH_INTRODUCE_ENTITY, entityId)
        {
        }
    }
}
