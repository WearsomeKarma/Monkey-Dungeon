﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Entity_Death : Multiplayer_Message_UI_Handler
    {
        World_Layer World_Layer { get; set; }

        public MMH_Entity_Death(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_ENTITY_DEATH)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            World_Layer.Get_GameEntity(recievedMessage.ENTITY_ID).Set_Incapacitated_Status(true);
        }
    }
}
