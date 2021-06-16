using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameStates;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Controllers
{
    public class GameEntity_Controller_Player : GameEntity_Controller
    {
        protected override void Handle_Get__Combat_Action__Controller()
        {
            
        }

        public override GameEntity_Controller Clone__Controller()
        {
            return new GameEntity_Controller_Player();
        }
    }
}
