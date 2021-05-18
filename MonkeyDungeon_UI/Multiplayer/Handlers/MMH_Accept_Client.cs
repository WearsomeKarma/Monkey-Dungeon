using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Accept_Client : Multiplayer_Message_UI_Handler
    {
        private MonkeyDungeon_Game_Client MD_GC { get; set; }

        public MMH_Accept_Client(MonkeyDungeon_Game_Client md_gc) 
            : base(null, MD_VANILLA_MMH.MMH_ACCEPT_CLIENT)
        {
            MD_GC = md_gc;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            MD_GC.SceneManagementService.SetScene("gameScene");
        }
    }
}
