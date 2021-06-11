using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Prefabs;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Update_Ability_Point : Multiplayer_Message_UI_Handler
    { 
        World_Layer World_Layer { get; set; }

        public MMH_Update_Ability_Point(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_UPDATE_ABILITY_POINT)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int abilityPointCount = recievedMessage.INT_VALUE;

            GameEntity_ClientSide entity = World_Layer.Get_GameEntity(recievedMessage.Local_Entity_ID);

            entity.Ability_Points.Resource_Percentage = abilityPointCount;
        }
    }
}
