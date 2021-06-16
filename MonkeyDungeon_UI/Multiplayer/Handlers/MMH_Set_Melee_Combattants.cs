using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.Handlers
{
    public class MMH_Set_Melee_Combattants : Multiplayer_Message_UI_Handler
    {
        private World_Layer World_Layer { get; set; }

        public MMH_Set_Melee_Combattants(World_Layer sceneLayer) 
            : base(sceneLayer, MD_VANILLA_MMH.MMH_SET_MELEE_COMBATTANTS)
        {
            World_Layer = sceneLayer;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            GameEntity_ID ally = recievedMessage.ENTITY_ID;
            GameEntity_ID enemy = GameEntity_ID.IDS[recievedMessage.INT_VALUE];

            World_Layer.UI_MeleeEvent.Ally_Id = ally;
            World_Layer.UI_MeleeEvent.Enemy_Id = enemy;
        }
    }
}
