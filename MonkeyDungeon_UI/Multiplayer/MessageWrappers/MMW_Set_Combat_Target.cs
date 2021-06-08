using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.MessageWrappers
{
    public class MMW_Set_Combat_Target : Multiplayer_Message_Wrapper
    {
        public MMW_Set_Combat_Target(GameEntity_ID target)
            : base (
                MD_VANILLA_MMH.MMH_SET_COMBAT_TARGET, 
                GameEntity_ID.ID_NULL,
                0,
                target
                )
        {}
    }
}