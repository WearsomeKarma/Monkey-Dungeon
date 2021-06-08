using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer
{
    public abstract class Multiplayer_Message_CombatState_Handler : Multiplayer_Message_GameState_Handler
    {
        protected Combat_GameState Combat { get; private set; }

        public Multiplayer_Message_CombatState_Handler(Combat_GameState combat,
            params GameEntity_Attribute_Name[] acceptedTypes)
        : base(combat, acceptedTypes)
        {
            Combat = combat;
        }

        protected bool IsValid_Message(Multiplayer_Message message)
        {
            bool ret = Combat.Entity_ID_Of_Current_Turn.Relay_ID == message.Relay_ID;

            if (!ret)
                Handle_Invalid_Message(message);
            
            return ret;
        }
    }
}