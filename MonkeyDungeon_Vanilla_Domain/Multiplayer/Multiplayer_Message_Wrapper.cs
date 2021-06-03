using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public class Multiplayer_Message_Wrapper
    {
        public readonly Multiplayer_Message MESSAGE;

        public Multiplayer_Message_Wrapper(
            GameEntity_Attribute_Name messageType = null,
            GameEntity_ID entityId = null,
            float fval=0,
            int ival=0,
            GameEntity_Attribute_Name sval = null
            )
        {
            MESSAGE = new Multiplayer_Message(
                messageType ?? GameEntity_Attribute_Name.DEFAULT,
                entityId ?? GameEntity_ID.ID_ZERO,
                fval,
                ival,
                sval ?? GameEntity_Attribute_Name.DEFAULT
                );
        }

        public static implicit operator Multiplayer_Message(Multiplayer_Message_Wrapper mmw)
            => mmw.MESSAGE;
    }
}
