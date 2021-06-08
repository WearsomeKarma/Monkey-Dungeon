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
    public class MMH_Update_Entity_UniqueID : Multiplayer_Message_UI_Handler
    {
        World_Layer World_Layer { get; set; }

        public MMH_Update_Entity_UniqueID(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_UPDATE_ENTITY_UNIQUEID)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int entity_id = recievedMessage.Local_Entity_ID;
            int unique_id = recievedMessage.INT_VALUE;

            World_Layer.Set_Unique_ID(entity_id, (uint)unique_id);
        }
    }
}
