using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Declare_Entity_Description : Multiplayer_Message_UI_Handler
    {
        private World_Layer World_Layer { get; set; }

        public MMH_Declare_Entity_Description(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_DECLARE_ENTITY_DESCRIPTION)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            GameEntity_ID id = recievedMessage.Local_Entity_ID;
            GameEntity_Attribute_Name @class = recievedMessage.ATTRIBUTE;

            World_Layer.Set_Description(id, @class);
        }
    }
}
