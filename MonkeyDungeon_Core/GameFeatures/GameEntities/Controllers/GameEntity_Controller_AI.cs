using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Controllers
{
    public class GameEntity_Controller_AI : GameEntity_Controller
    {
        public GameEntity_Controller_AI()
            : base(true)
        { }

        protected override Combat_Action Handle_CombatAction_Request(GameEntity_ServerSide_Roster gameField)
        {
            Random rand = new Random();
            GameEntity_ServerSide[] players = gameField.Get_Entities(GameEntity_Team_ID.TEAM_ONE_ID, true);
            GameEntity_Position targetId = players[rand.Next(players.Length)].GameEntity_Position;

            //TODO: make combat ref GameScene
            Combat_Action ca = new Combat_Action();
            ca.Action_Owner = EntityServerSide.GameEntity_ID;
            ca.Target.Add_Target(targetId);
            ca.Selected_Ability = MD_VANILLA_ABILITY_NAMES.ABILITY_PUNCH;

            return ca;
        }

        public override GameEntity_Controller Clone()
        {
            return new GameEntity_Controller_AI();
        }
    }
}
