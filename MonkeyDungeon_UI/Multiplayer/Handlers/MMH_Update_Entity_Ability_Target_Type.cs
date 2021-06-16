using System;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Update_Entity_Ability_Target_Type : Multiplayer_Message_UI_Handler
    {
        private World_Layer World_Layer { get; set; }
        
        public MMH_Update_Entity_Ability_Target_Type(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_UPDATE_ENTITY_ABILITY_TARGET_TYPE)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            GameEntity_ID entityId = recievedMessage.ENTITY_ID;
            
            GameEntity_Attribute_Name_Ability abilityName
                = GameEntity_Attribute_Name.Cast<GameEntity_Attribute_Name_Ability>(recievedMessage.ATTRIBUTE, GameEntity_Attribute_Type.ABILITY_NAMES);
            
            Combat_Target_Type targetType = (Combat_Target_Type) recievedMessage.INT_VALUE;

            World_Layer.Get_GameEntity(entityId).Set_Ability_Target_Type(abilityName, targetType);
        }
    }
}