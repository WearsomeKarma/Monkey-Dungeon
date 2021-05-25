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
    public class MMH_Set_Traveling_State : Multiplayer_Message_UI_Handler
    {
        World_Layer World_Layer { get; set; }

        public MMH_Set_Traveling_State(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_SET_TRAVELING_STATE)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            //0 is not traveling
            bool state = recievedMessage.INT_VALUE != 0;

            World_Layer.IsTraveling = state;
        }
    }
}
