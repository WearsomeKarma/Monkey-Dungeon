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
    public class MMH_Set_Party_UI_Descriptions : Multiplayer_Message_UI_Handler
    {
        private World_Layer World_Layer { get; set; }

        public MMH_Set_Party_UI_Descriptions(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_SET_PARTY_UI_DESCRIPTIONS)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int isPlayers = recievedMessage.INT_VALUE;
            string[] classes = recievedMessage.STRING_VALUE.Split(' ');

            World_Layer.Set_Descriptions(isPlayers, classes);
        }
    }
}
