﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Prefabs;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Update_Entity_Resource : Multiplayer_Message_UI_Handler
    {
        private World_Layer World_Layer { get; set; }

        public MMH_Update_Entity_Resource(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_UPDATE_ENTITY_RESOURCE)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            GameEntity_ID id = recievedMessage.ENTITY_ID;
            float percentage = recievedMessage.FLOAT_VALUE;
            GameEntity_Attribute_Name resourceName = recievedMessage.ATTRIBUTE;
            
            GameEntity_ClientSide entity = World_Layer.Get_GameEntity(id);
            entity.Set_Resource_Percentage(resourceName, percentage);
        }
    }
}
