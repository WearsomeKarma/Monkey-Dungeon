using System;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.Controllers
{
    public class GameEntityServerSideControllerAi : GameEntity_ServerSide_Controller
    {
        public GameEntityServerSideControllerAi()
            : base(true)
        {
        }

        protected override void Handle_Get__Combat_Action__Controller()
        {
            var rand = new Random();
            var players = GameEntity_Roster.Get_Entities(GameEntity_Team_ID.TEAM_ONE_ID, true);
            var targetId = players[rand.Next(players.Length)].GameEntity__Position;

            var ability =
                Attached_Entity.Get__Ability__GameEntity<GameEntity_ServerSide_Ability>(MD_VANILLA_ABILITY_NAMES
                    .ABILITY_PUNCH);

            GameEntity_Controller_ServerSide_Action.Set_Ability(ability);
            Combat_Setup__Add_Target__ServerSide_Controller(targetId);
        }

        public override GameEntity_ServerSide_Controller Clone__Controller()
        {
            return new GameEntityServerSideControllerAi();
        }
    }
}