using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer
{
    public abstract class Multiplayer_Message_GameStateHandler : Multiplayer_Message_Handler
    {
        protected GameState_Machine GameState_Machine { get; private set; }
        protected GameState GameState { get; private set; }

        public Multiplayer_Message_GameStateHandler(GameState gameState, params string[] acceptedTypes)
            : base(acceptedTypes)
        {
            GameState_Machine = gameState.GameState_Machine;
            GameState = gameState;
        }
    }
}
