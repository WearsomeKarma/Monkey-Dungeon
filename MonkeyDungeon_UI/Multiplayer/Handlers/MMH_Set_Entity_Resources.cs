using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Prefabs.UI.EntityData;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Set_MD_VANILLA_RESOURCES : Multiplayer_Message_UI_Handler
    {
        World_Layer World_Layer { get; set; }
        public object UI_GameEntity_Description { get; private set; }

        public MMH_Set_MD_VANILLA_RESOURCES(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_SET_MD_VANILLA_RESOURCES)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            string[] resourceNames = recievedMessage.STRING_VALUE.Split(' ');

            UI_GameEntity_Descriptor entity = World_Layer.Get_Description_From_Id(recievedMessage.ENTITY_ID);

            entity.Set_Resources(resourceNames);
        }
    }
}
