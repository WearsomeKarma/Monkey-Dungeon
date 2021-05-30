using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.MessageWrappers
{
    public class MMW_Set_Entity : Multiplayer_Message_Wrapper
    {
        public MMW_Set_Entity(GameEntity_ID sceneId, GameEntity_Attribute_Name factoryTag) 
            : base(MD_VANILLA_MMH.MMH_SET_ENTITY, sceneId, 0, 0, factoryTag)
        {
        }
    }
}
