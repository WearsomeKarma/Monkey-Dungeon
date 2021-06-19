using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities.Implemented;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.Controllers
{
    public class GameEntityServerSideControllerAi : GameEntity_ServerSide_Controller
    {
        public GameEntityServerSideControllerAi()
            : base(true)
        { }

        protected override void Handle_Get__Combat_Action__Controller()
        {
            Random rand = new Random();
            GameEntity_ServerSide[] players = GameEntity_Roster.Get_Entities(GameEntity_Team_ID.TEAM_ONE_ID, true);
            GameEntity_Position targetId = players[rand.Next(players.Length)].GameEntity_Position;

            GameEntity_ServerSide_Ability ability =
                Attached_GameEntity.Get__Ability__GameEntity<GameEntity_ServerSide_Ability>(MD_VANILLA_ABILITY_NAMES.ABILITY_PUNCH);
            
            GameEntity_Controller_ServerSide_Action.Set_Ability(ability);
            Combat_Setup__Add_Target(targetId);
        }

        public override GameEntity_ServerSide_Controller Clone__Controller()
        {
            return new GameEntityServerSideControllerAi();
        }
    }
}
