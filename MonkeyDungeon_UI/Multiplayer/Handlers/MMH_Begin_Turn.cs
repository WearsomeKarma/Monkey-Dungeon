using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Begin_Turn : Multiplayer_Message_UI_Handler
    {
        private UI_Combat_Layer UI_Combat_Layer { get; set; }

        public MMH_Begin_Turn(UI_Combat_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_BEGIN_TURN)
        {
            UI_Combat_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            UI_Combat_Layer.BeginTurn(recievedMessage.Local_Entity_ID);
        }
    }
}
