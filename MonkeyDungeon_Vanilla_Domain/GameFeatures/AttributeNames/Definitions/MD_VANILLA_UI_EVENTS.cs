
namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions
{
    public static class MD_VANILLA_UI_EVENTS
    {
        public static readonly GameEntity_Attribute_Name_UI_Event UI_EVENT_ANNOUNCEMENT = new GameEntity_Attribute_Name_UI_Event("UI_Event_Announcement");
        public static readonly GameEntity_Attribute_Name_UI_Event UI_EVENT_MELEE = new GameEntity_Attribute_Name_UI_Event("UI_Event_Melee");
        public static readonly GameEntity_Attribute_Name_UI_Event UI_EVENT_RANGED_ATTACK = new GameEntity_Attribute_Name_UI_Event("UI_Event_Ranged_Attack");
        
        public static readonly GameEntity_Attribute_Name_UI_Event[] STRINGS = new GameEntity_Attribute_Name_UI_Event[]
        {
            UI_EVENT_ANNOUNCEMENT,
            UI_EVENT_MELEE,
            UI_EVENT_RANGED_ATTACK
        };
    }
}
