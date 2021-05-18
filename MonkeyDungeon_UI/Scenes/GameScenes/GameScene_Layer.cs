using isometricgame.GameEngine;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Scenes.GameScenes
{
    public class GameScene_Layer : SceneLayer
    {
        public static readonly int WORLD_LAYER_INDEX = 2;
        public static readonly int UI_GENERIC_LAYER_INDEX = 1;
        public static readonly int UI_CLIENT_LAYER_INDEX = 0;

        public EventScheduler EventScheduler { get; private set; }

        public GameScene_Layer(GameScene parentScene, int layer) 
            : base(parentScene, layer)
        {
            EventScheduler = parentScene.EventScheduler;
        }
    }
}
