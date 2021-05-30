using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Declare_Entity_Resource : Multiplayer_Message_Wrapper
    {
        public MMW_Declare_Entity_Resource(GameEntity_ID entityId, GameEntity_Attribute_Name resourceName) 
            : base(MD_VANILLA_MMH.MMH_DECLARE_ENTITY_RESOURCE, entityId, 0, 0, resourceName)
        {
        }
    }
}
