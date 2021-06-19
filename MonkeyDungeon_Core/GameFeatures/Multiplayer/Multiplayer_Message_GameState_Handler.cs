using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer
{
    public abstract class Multiplayer_Message_GameState_Handler : Multiplayer_Message_Handler
    {
        protected Game_StateMachine GameState_Machine { get; private set; }
        protected GameState GameState { get; private set; }

        public Multiplayer_Message_GameState_Handler(GameState gameState, params GameEntity_Attribute_Name[] acceptedTypes)
            : base(acceptedTypes)
        {
            GameState_Machine = gameState.GameState_Machine;
            GameState = gameState;
        }
    }
}
