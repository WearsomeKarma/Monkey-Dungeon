using isometricgame.GameEngine;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Scenes.GameScenes
{
    public class GameScene_Layer : SceneLayer
    {
        public static readonly int WORLD_LAYER_INDEX = 2;
        public static readonly int UI_GENERIC_LAYER_INDEX = 1;
        public static readonly int UI_CLIENT_LAYER_INDEX = 0;

        protected GameScene GameScene { get; set; }
        protected MonkeyDungeon_Game_Client MonkeyDungeon_Game_Client { get; set; }
        
        public EventScheduler EventScheduler { get; private set; }

        public GameScene_Layer(GameScene parentScene, int layer) 
            : base(parentScene, layer)
        {
            GameScene = parentScene;
            MonkeyDungeon_Game_Client = GameScene.MonkeyDungeon_Game_UI;
            
            EventScheduler = parentScene.EventScheduler;
        }

        protected void Relay_Message(Multiplayer_Message msg)
        {
            MonkeyDungeon_Game_Client.Client_RecieverEndpoint_UI.Queue_Message(msg);
        }

        protected void Add_Handler_Expectation(params Multiplayer_Message_Handler[] mmhs)
        {
            foreach(Multiplayer_Message_Handler mmh in mmhs)
                MonkeyDungeon_Game_Client.Expectation_Context.Register_Handler(mmh);
        }
    }
}
