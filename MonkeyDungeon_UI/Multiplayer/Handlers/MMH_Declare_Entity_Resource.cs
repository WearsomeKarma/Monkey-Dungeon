using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Prefabs;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Declare_Entity_Resource : Multiplayer_Message_UI_Handler
    {
        World_Layer World_Layer { get; set; }
        public object UI_GameEntity_Description { get; private set; }

        public MMH_Declare_Entity_Resource(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_DECLARE_ENTITY_RESOURCE)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            GameEntity_Attribute_Name resourceName = recievedMessage.ATTRIBUTE;

            GameEntity_ClientSide entity = World_Layer.Get_GameEntity(recievedMessage.ENTITY_ID);

            //I hate this code and TODO: change it.
            entity.Add_Resource(GameEntity_Attribute_Name.Cast<GameEntity_Attribute_Name_Resource>(resourceName, GameEntity_Attribute_Type.RESOURCE_NAMES));
        }
    }
}
