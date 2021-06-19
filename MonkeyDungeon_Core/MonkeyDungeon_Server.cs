using MonkeyDungeon_Core.GameFeatures;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.Multiplayer; 

namespace MonkeyDungeon_Core
{
    public class MonkeyDungeon_Server : Multiplayer_Relay_Manager
    {
        public Game_StateMachine GameState_Machine { get; private set; }

        public MonkeyDungeon_Server(
            Multiplayer_Relay localRelay
            )
            :base(localRelay)
        {
            GameState_Machine = new Game_StateMachine(
                this,
                new GameState[]
                {
                    new GameState_Traveling(),
                    new GameState_Combat(),
                    new GameState_Checkpoint(),
                    new GameState_GameOver()
                }
                );
        }

        public void On_Update_Frame(double deltaTime)
        {
            Check_Relays();

            GameState_Machine.CheckFor_GameState_Transition(deltaTime);

            Flush_Relays();
        }
    }
}
