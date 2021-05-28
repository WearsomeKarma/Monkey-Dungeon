using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public struct Multiplayer_Message
    {
        public static readonly string 
            MM_MESSAGE_NULL = "MESSAGE_NULL", 
            MM_MESSAGE_INVALID = "MESSAGE_INVALID",
            MM_MESSAGE_FAIL = "MESSAGE_FAIL";

        public static readonly Multiplayer_Message MESSAGE_NULL = new Multiplayer_Message();
        public static int ID_NULL = -1;

        public int Relay_ID { get; internal set; }
        public int Message_ID { get; internal set; }
        public bool Is_Unsent_Message => Relay_ID == ID_NULL;

        public readonly string MESSAGE_TYPE;

        public readonly int ENTITY_ID;

        public readonly float FLOAT_VALUE;
        public readonly int INT_VALUE;
        public readonly string STRING_VALUE;

        public Multiplayer_Message(
            string messageType = "MESSAGE_NULL", 
            int entityId = 0, 
            float fval = 0, 
            int ival = 0, 
            string sval = ""
            )
        {
            Relay_ID = ID_NULL;
            Message_ID = ID_NULL;

            MESSAGE_TYPE = messageType ?? MD_VANILLA_MMH.MMH_DEFAULT;
            ENTITY_ID = entityId;
            FLOAT_VALUE = fval;
            INT_VALUE = ival;
            STRING_VALUE = sval;
        }

        internal Multiplayer_Message(
            int relayId, 
            string messageType = "MESSAGE_NULL", 
            int entityId = 0, 
            float fval = 0, 
            int ival = 0, 
            string sval = ""
            )
        {
            Relay_ID = relayId;
            Message_ID = ID_NULL;

            MESSAGE_TYPE = messageType ?? MD_VANILLA_MMH.MMH_DEFAULT;
            ENTITY_ID = entityId;
            FLOAT_VALUE = fval;
            INT_VALUE = ival;
            STRING_VALUE = sval;
        }
        
        public bool Equals(Multiplayer_Message m)
        {
            return (
                MESSAGE_TYPE == m.MESSAGE_TYPE &&
                ENTITY_ID == m.ENTITY_ID &&
                FLOAT_VALUE == m.FLOAT_VALUE &&
                INT_VALUE == m.INT_VALUE &&
                STRING_VALUE == m.STRING_VALUE
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
                  ENTITY_ID,
                  INT_VALUE,
                  FLOAT_VALUE,
                  STRING_VALUE
                );
        }

        public static bool operator ==(Multiplayer_Message m1, Multiplayer_Message m2)
            => m1.Equals(m2);

        public static bool operator !=(Multiplayer_Message m1, Multiplayer_Message m2)
            => !m1.Equals(m2);
    }
}
