namespace MonkeyDungeon_Core.GameFeatures.GameComponents.Controllers
{
    public class GameEntityServerSideControllerPlayer : GameEntity_ServerSide_Controller
    {
        protected override void Handle_Get__Combat_Action__Controller()
        {
        }

        public override GameEntity_ServerSide_Controller Clone__Controller()
        {
            return new GameEntityServerSideControllerPlayer();
        }
    }
}