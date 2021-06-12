using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Prefabs;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.MessageWrappers
{
    public class MMW_Combat_Set_Selected_Ability : Multiplayer_Message_Wrapper
    {
        public MMW_Combat_Set_Selected_Ability(
            GameEntity_ClientSide_Ability ability
            ) 
            : base(
                  MD_VANILLA_MMH.MMH_COMBAT_SET_SELECTED_ABILITY, 
                  GameEntity_ID.ID_ZERO, 
                  0, 
                  0,
                  ability.Ability_Name
                  )
        {
        }
    }
}
