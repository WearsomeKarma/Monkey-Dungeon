using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Invoke_UI_Event : Multiplayer_Message_UI_Handler
    {
        private readonly GameScene_Layer GAMESCENE_LAYER;

        public MMH_Invoke_UI_Event(GameScene_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_INVOKE_UI_EVENT)
        {
            GAMESCENE_LAYER = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            GAMESCENE_LAYER.EventScheduler.Invoke_Event(recievedMessage.STRING_VALUE);
        }
    }
}
