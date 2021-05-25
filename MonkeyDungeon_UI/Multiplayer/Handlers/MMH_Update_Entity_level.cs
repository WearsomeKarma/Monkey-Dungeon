using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Update_Entity_Level : Multiplayer_Message_UI_Handler
    {
        public MMH_Update_Entity_Level(SceneLayer sceneLayer) 
            : base(sceneLayer, "not-implemented")
        {
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            throw new NotImplementedException();
        }
    }
}
