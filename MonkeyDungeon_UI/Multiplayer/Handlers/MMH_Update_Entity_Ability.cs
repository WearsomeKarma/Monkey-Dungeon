using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Prefabs;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Update_Entity_Ability : Multiplayer_Message_UI_Handler
    {
        private World_Layer World_Layer { get; set; }

        public MMH_Update_Entity_Ability(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_UPDATE_ENTITY_ABILITY)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            GameEntity_ID entity_id = recievedMessage.ENTITY_ID;
            GameEntity_Ability_Index abilityIndex = GameEntity_Ability_Index.INDICES[recievedMessage.INT_VALUE];
            GameEntity_Attribute_Name abilityName = recievedMessage.ATTRIBUTE;

            GameEntity_ClientSide entity = World_Layer.Get_GameEntity(entity_id);
            entity.Set_Ability(abilityIndex, GameEntity_Attribute_Name.Cast<GameEntity_Attribute_Name_Ability>(abilityName, GameEntity_Attribute_Type.ABILITY_NAMES));
        }
    }
}
