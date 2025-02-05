﻿using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_UI.Multiplayer
{
    public abstract class Multiplayer_Message_UI_Handler : Multiplayer_Message_Handler
    {
        protected SceneLayer SceneLayer { get; private set; }

        public Multiplayer_Message_UI_Handler(SceneLayer sceneLayer, params GameEntity_Attribute_Name[] acceptedTypes)
            : base(acceptedTypes)
        {
            SceneLayer = sceneLayer;
        }
    }
}
