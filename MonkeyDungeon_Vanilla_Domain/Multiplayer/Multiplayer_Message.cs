using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public struct Multiplayer_Message
    {
        public static readonly GameEntity_Attribute_Name
            MM_MESSAGE_NULL = new GameEntity_Attribute_Name("MESSAGE_NULL"), 
            MM_MESSAGE_INVALID = new GameEntity_Attribute_Name("MESSAGE_INVALID"),
            MM_MESSAGE_FAIL = new GameEntity_Attribute_Name("MESSAGE_FAIL");

        public static readonly Multiplayer_Message MESSAGE_NULL = new Multiplayer_Message();

        public Multiplayer_Relay_ID Relay_ID { get; internal set; }
        public int Message_ID { get; internal set; }
        public bool Is_Unsent_Message => Relay_ID == Multiplayer_Relay_ID.ID_NULL;

        public readonly GameEntity_Attribute_Name MESSAGE_TYPE;

        private readonly int ID_INDEX;

        public GameEntity_ID Local_Entity_ID
            => GameEntity_ID.IDS[ID_INDEX];

        public readonly float FLOAT_VALUE;
        public readonly int INT_VALUE;
        public GameEntity_Attribute_Name ATTRIBUTE;

        public Multiplayer_Message(
            GameEntity_Attribute_Name messageType = null, 
            GameEntity_ID entityId = null, 
            float fval = 0, 
            int ival = 0,
            GameEntity_Attribute_Name sval = null
            )
        {
            Relay_ID = Multiplayer_Relay_ID.ID_NULL;
            Message_ID = -1; //TODO prim wrap

            MESSAGE_TYPE = messageType ?? MD_VANILLA_MMH.MMH_DEFAULT;
            ID_INDEX = entityId ?? GameEntity_ID.ID_ZERO;
            FLOAT_VALUE = fval;
            INT_VALUE = ival;
            ATTRIBUTE = sval ?? GameEntity_Attribute_Name.DEFAULT;
        }

        internal Multiplayer_Message(
            Multiplayer_Relay_ID relayId,
            GameEntity_Attribute_Name messageType = null,
            GameEntity_ID entityId = null, 
            float fval = 0, 
            int ival = 0,
            GameEntity_Attribute_Name sval = null
            )
        {
            Relay_ID = relayId;
            Message_ID = -1;//TODO: prim wrap

            MESSAGE_TYPE = messageType ?? MD_VANILLA_MMH.MMH_DEFAULT;
            ID_INDEX = entityId ?? GameEntity_ID.ID_ZERO;
            FLOAT_VALUE = fval;
            INT_VALUE = ival;
            ATTRIBUTE = sval ?? GameEntity_Attribute_Name.DEFAULT;
        }
        
        public bool Equals(Multiplayer_Message m)
        {
            return (
                MESSAGE_TYPE == m.MESSAGE_TYPE &&
                Local_Entity_ID == m.Local_Entity_ID &&
                FLOAT_VALUE == m.FLOAT_VALUE &&
                INT_VALUE == m.INT_VALUE &&
                ATTRIBUTE == m.ATTRIBUTE
                );
        }

        public override string ToString()
        {
            return String.Format(
                  "\tTYPE: {0}" +
                "\n\tENTITY_ID: {1}" +
                "\n\tINT_VALUE: {2}" +
                "\n\tFLOAT_VALUE: {3}" +
                "\n\tSTRING_VALUE: {4}",
                  MESSAGE_TYPE,
                  Local_Entity_ID,
                  INT_VALUE,
                  FLOAT_VALUE,
                  ATTRIBUTE
                );
        }

        public static bool operator ==(Multiplayer_Message m1, Multiplayer_Message m2)
            => m1.Equals(m2);

        public static bool operator !=(Multiplayer_Message m1, Multiplayer_Message m2)
            => !m1.Equals(m2);
    }
}
