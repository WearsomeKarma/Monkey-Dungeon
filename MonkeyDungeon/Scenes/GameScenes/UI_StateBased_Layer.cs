using isometricgame.GameEngine;
using isometricgame.GameEngine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.GameScenes
{
    public class UI_StateBased_Layer : SceneLayer
    {
        public Type GameStateHandler_Type { get; private set; }

        public UI_StateBased_Layer(Scene parentScene, Type gameStateHandler_Type) 
            : base(parentScene, 1)
        {
            GameStateHandler_Type = gameStateHandler_Type;
        }
    }
}
