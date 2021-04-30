using isometricgame.GameEngine;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.GameScenes
{
    public class UI_Shopping_Layer : UI_StateBased_Layer
    {
        public UI_Shopping_Layer(Scene parentScene) 
            : base(parentScene, typeof(Shopping_GameState))
        {
        }
    }
}
