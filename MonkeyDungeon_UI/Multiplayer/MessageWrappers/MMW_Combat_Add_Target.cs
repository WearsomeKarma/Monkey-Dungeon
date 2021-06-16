using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.MessageWrappers
{
    public class MMW_Combat_Add_Target : Multiplayer_Message_Wrapper
    {
        public MMW_Combat_Add_Target(GameEntity_Position target)
            : base (
                MD_VANILLA_MMH.MMH_COMBAT_ADD_TARGET, 
                GameEntity_ID.ID_NULL,
                0,
                (int)target
                )
        {}
    }
}