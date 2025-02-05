﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public static class MD_VANILLA_MMH
    {
        /// <summary>
        /// A context that is typically ignored. The empty context. Sequential messages following this
        /// context will be IGNORED!
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_DEFAULT 
            = new GameEntity_Attribute_Name("MMH_Null");

        /// <summary>
        /// Inform the client they are accepted into the game, and that 
        /// they may transition into GameScene.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_ACCEPT_CLIENT 
            = new GameEntity_Attribute_Name("MMH_Accept_Client");

        /// <summary>
        /// Context for beginning the game. INT: Party Size. STRING: FactoryTags seperated by spaces.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_DECLARE_ENTITY_DESCRIPTION 
            = new GameEntity_Attribute_Name("MMH_Declare_Entity_Description");

        /// <summary>
        /// Context for sending an announcement to the client. 
        /// Invokes MD_VANILLA_UI.UI_EVENT_ANNOUNCEMENT on client side.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_ANNOUNCEMENT 
            = new GameEntity_Attribute_Name("MMH_Announce");

        /// <summary>
        /// Context for declaring the start of a new turn. INT: Entity_ID
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_BEGIN_TURN 
            = new GameEntity_Attribute_Name("MMH_Begin_Turn");

        /// <summary>
        /// Context for setting an entity. INT: Entity_ID, STRING: FactoryTag
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_SET_ENTITY 
            = new GameEntity_Attribute_Name("MMH_Set_Entity");
        /// <summary>
        /// Context for flagging an entity of a team being ready to compete. 
        /// INT: Status {0: ready | !0: not ready}
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_SET_ENTITY_READY 
            = new GameEntity_Attribute_Name("MMH_Set_Entity_Ready");
        /// <summary>
        /// Context for updating to the server - client is requesting the end of their turn.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_REQUEST_ENDTURN 
            = new GameEntity_Attribute_Name("MMH_Request_EndTurn");

        public static readonly GameEntity_Attribute_Name MMH_INTRODUCE_ENTITY 
            = new GameEntity_Attribute_Name("MMH_Introduce_Entity");
        /// <summary>
        /// Context for updating to the client side - the dismissal of an entity.
        /// This will hide the entity on the client side. Good for NPC death or player disconnect.
        /// ENTITY_ID.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_DISMISS_ENTITY 
            = new GameEntity_Attribute_Name("MMH_Dismiss_Entity");
        /// <summary>
        /// Context for updating to the client side - that an entity died.
        /// ENTITY_ID.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_ENTITY_DEATH 
            = new GameEntity_Attribute_Name("MMH_Entity_Death");
        /// <summary>
        /// Context for updating to the client side - the names of the resources used.
        /// ENTITY_ID,
        /// STRING: seperated by a space, the names of the resources.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_DECLARE_ENTITY_RESOURCE 
            = new GameEntity_Attribute_Name("MMH_Set_MD_VANILLA_RESOURCES");
        /// <summary>
        /// Context for updating to the client side - the percentage of an entity's resource.
        /// ENTITY_ID,
        /// FLOAT: percentage of resource,
        /// STRING: name of resource.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_UPDATE_ENTITY_RESOURCE 
            = new GameEntity_Attribute_Name("MMH_Update_Entity_Resource");
        /// <summary>
        /// Context for updating to the client side - the abilities of an entity.
        /// ENTITY_ID,
        /// STRING: all the abilities sperated by a comma.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_UPDATE_ENTITY_ABILITY 
            = new GameEntity_Attribute_Name("MMH_Update_Entity_Ability");

        public static readonly GameEntity_Attribute_Name MMH_UPDATE_ENTITY_ABILITY_TARGET_TYPE
            = new GameEntity_Attribute_Name("MMH_Update_Entity_Ability_Target_Type");
        /// <summary>
        /// Context for updating to the client side - the uid of an entity. 
        /// Important for visual appearance.
        /// ENTITY_ID,
        /// INT: (uint) uid.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_UPDATE_ENTITY_UNIQUEID 
            = new GameEntity_Attribute_Name("MMH_Update_Entity_UniqueID");
        /// <summary>
        /// Context for updating the the client side - the ability point count of an entity.
        /// ENTITY_ID,
        /// INT: ability point count.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_UPDATE_ABILITY_POINT 
            = new GameEntity_Attribute_Name("MMH_Update_Ability_Point");

        /// <summary>
        /// Context for relaying to client side - if the party is traveling or not.
        /// INT: 0 is not traveling, any other value is.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_SET_TRAVELING_STATE 
            = new GameEntity_Attribute_Name("MMH_Set_Traveling_State");

        /// <summary>
        /// Context for relaying to server side - the client's combat action.
        /// ENTITY_ID: the sender of the message.
        /// INT: the target.
        /// STRING: the ability.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_COMBAT_SET_SELECTED_ABILITY 
            = new GameEntity_Attribute_Name("MMH_Combat_Set_Selected_Ability");

        public static readonly GameEntity_Attribute_Name MMH_COMBAT_ADD_TARGET
            = new GameEntity_Attribute_Name("MMH_Combat_Add_Target");
        /// <summary>
        /// Context for relaying to client side - the entities involved in the melee event.
        /// ENTITY_ID: the ally side entity.
        /// INT: the enemy side entity.
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_SET_MELEE_COMBATTANTS 
            = new GameEntity_Attribute_Name("MMH_Set_Melee_Combattants");

        public static readonly GameEntity_Attribute_Name MMH_SET_RANGED_PARTICLE 
            = new GameEntity_Attribute_Name("MMH_Set_Ranged_Particle");

        /// <summary>
        /// Context for initiating a UI event on the client side. STRING: EventTag
        /// </summary>
        public static readonly GameEntity_Attribute_Name MMH_INVOKE_UI_EVENT 
            = new GameEntity_Attribute_Name("MMH_Invoke_UI_Event");


        public static readonly GameEntity_Attribute_Name[] MMH_STRINGS 
            = new GameEntity_Attribute_Name[]
        {
            //DEFAULT
            MMH_DEFAULT,

            //GAME STATE EVENTS
            MMH_ACCEPT_CLIENT,
            MMH_DECLARE_ENTITY_DESCRIPTION,

            MMH_ANNOUNCEMENT,

            MMH_BEGIN_TURN,

            //client
            MMH_SET_ENTITY,
            MMH_SET_ENTITY_READY,
            MMH_REQUEST_ENDTURN,

            //server
            MMH_INTRODUCE_ENTITY,
            MMH_DISMISS_ENTITY,
            MMH_ENTITY_DEATH,
            MMH_DECLARE_ENTITY_RESOURCE,
            MMH_UPDATE_ENTITY_RESOURCE,
            MMH_UPDATE_ENTITY_ABILITY,
            MMH_UPDATE_ENTITY_ABILITY_TARGET_TYPE,
            MMH_UPDATE_ENTITY_UNIQUEID,

            //state
            MMH_SET_TRAVELING_STATE,

            //COMBAT
            MMH_COMBAT_SET_SELECTED_ABILITY,
            MMH_SET_MELEE_COMBATTANTS,
            MMH_SET_RANGED_PARTICLE,
            //UI CLIENT EVENTS
            MMH_INVOKE_UI_EVENT
        };
    }
}
