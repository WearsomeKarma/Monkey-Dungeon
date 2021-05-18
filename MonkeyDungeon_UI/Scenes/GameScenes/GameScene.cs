using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Prefabs.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Scenes.GameScenes
{
    public class GameScene : Scene
    {
        internal UI_PauseMenu_Layer UI_Base_Layer { get; set; }
        internal UI_Combat_Layer UI_Combat_Layer { get; set; }
        internal UI_Shopping_Layer UI_Shopping_Layer { get; set; }
        internal GameScene_Layer[] UI_StateBased_Layers { get; set; }
        internal World_Layer World_Layer { get; set; }

        internal EventScheduler EventScheduler { get; private set; }

        internal MonkeyDungeon_Game_Client MonkeyDungeon_Game_UI { get; private set; }

        internal GameScene(MonkeyDungeon_Game_Client game) 
            : base(game)
        {
            MonkeyDungeon_Game_UI = game;
            EventScheduler = game.UI_EventScheduler;

            AddLayers(
                World_Layer = new World_Layer(this),
                UI_Combat_Layer = new UI_Combat_Layer(this),
                UI_Shopping_Layer = new UI_Shopping_Layer(this),
                UI_Base_Layer = new UI_PauseMenu_Layer(this)
                );
            //EnableOnlyLayer<World_Layer>();
            //EnableLayers<UI_PauseMenu_Layer>();

            UI_StateBased_Layers = new GameScene_Layer[] { UI_Combat_Layer, UI_Shopping_Layer };
        }
    }
}
