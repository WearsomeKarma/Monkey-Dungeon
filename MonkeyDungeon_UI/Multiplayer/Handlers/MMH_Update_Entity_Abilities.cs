using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Update_Entity_Abilities : Multiplayer_Message_UI_Handler
    {
        private World_Layer World_Layer { get; set; }

        public MMH_Update_Entity_Abilities(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_UPDATE_ENTITY_ABILITIES)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int entity_id = recievedMessage.ENTITY_ID;
            string[] abilities = recievedMessage.STRING_VALUE.Split(',');

            UI_GameEntity_Descriptor entity = World_Layer.Get_Description_From_Id(entity_id);
            entity.Set_Abilities(abilities);
        }
    }
}
