using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public static class MD_VANILLA_MMH
    {
        public static readonly string[] MMH_STRINGS = new string[] 
        {
            //DEFAULT
            MMH_DEFAULT,

            //GAME STATE EVENTS
            MMH_ACCEPT_CLIENT,
            MMH_SET_PARTY_UI_DESCRIPTIONS,

            MMH_BEGIN_TURN,

            MMH_SET_ENTITY,
            MMH_SET_ENTITY_READY,
            MMH_UPDATE_ENTITY_RESOURCE,
            MMH_UPDATE_ENTITY_ABILITIES,
            MMH_UPDATE_ENTITY_UNIQUEID,
            //UI CLIENT EVENTS
            MMH_INVOKE_UI_EVENT
        };

        /// <summary>
        /// A context that is typically ignored. The empty context. Sequential messages following this
        /// context will be IGNORED!
        /// </summary>
        public static readonly string MMH_DEFAULT = "MMH_Null";

        /// <summary>
        /// Inform the client they are accepted into the game, and that 
        /// they may transition into GameScene.
        /// </summary>
        public static readonly string MMH_ACCEPT_CLIENT = "MMH_Accept_Client";

        /// <summary>
        /// Context for beginning the game. INT: Party Size. STRING: FactoryTags seperated by spaces.
        /// </summary>
        public static readonly string MMH_SET_PARTY_UI_DESCRIPTIONS = "MMH_Set_Party_UI_Descriptions";

        /// <summary>
        /// Context for declaring the start of a new turn. INT: Entity_ID
        /// </summary>
        public static readonly string MMH_BEGIN_TURN = "MMH_Begin_Turn";

        /// <summary>
        /// Context for setting an entity. INT: Entity_ID, STRING: FactoryTag
        /// </summary>
        public static readonly string MMH_SET_ENTITY = "MMH_Set_Entity";
        /// <summary>
        /// Context for flagging an entity of a team being ready to compete. 
        /// INT: Status {0: ready | !0: not ready}
        /// </summary>
        public static readonly string MMH_SET_ENTITY_READY = "MMH_Set_Entity_Ready";
        /// <summary>
        /// Context for updating to the client side - the percentage of an entity's resource.
        /// ENTITY_ID,
        /// FLOAT: percentage of resource,
        /// STRING: name of resource.
        /// </summary>
        public static readonly string MMH_UPDATE_ENTITY_RESOURCE = "MMH_Update_Entity_Resource";
        /// <summary>
        /// Context for updating to the client side - the abilities of an entity.
        /// ENTITY_ID,
        /// STRING: all the abilities sperated by a comma.
        /// </summary>
        public static readonly string MMH_UPDATE_ENTITY_ABILITIES = "MMH_Update_Entity_Abilities";
        /// <summary>
        /// Context for updating to the client side - the uid of an entity. 
        /// Important for visual appearance.
        /// ENTITY_ID,
        /// INT: (uint) uid.
        /// </summary>
        public static readonly string MMH_UPDATE_ENTITY_UNIQUEID = "MMH_Update_Entity_UniqueID";

        /// <summary>
        /// Context for initiating a UI event on the client side. STRING: EventTag
        /// </summary>
        public static readonly string MMH_INVOKE_UI_EVENT = "MMH_Invoke_UI_Event";
    }
}
