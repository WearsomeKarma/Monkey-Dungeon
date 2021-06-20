using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer
{
    public abstract class Multiplayer_Message_CombatState_Handler : Multiplayer_Message_GameState_Handler
    {
        protected GameState_Combat GameStateCombat { get; private set; }

        public Multiplayer_Message_CombatState_Handler(GameState_Combat gameStateCombat,
            params GameEntity_Attribute_Name[] acceptedTypes)
        : base(gameStateCombat, acceptedTypes)
        {
            GameStateCombat = gameStateCombat;
        }

        protected bool IsValid_Message(Multiplayer_Message message)
        {
            bool ret = GameStateCombat.Combat__Entity_Relay_Id__Of_Current_Turn== message.Relay_ID;

            if (!ret)
                Handle_Invalid_Message(message);
            
            return ret;
        }
    }
}