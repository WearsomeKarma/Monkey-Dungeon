using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Controllers
{
    public class GameEntity_Controller_AI : GameEntity_Controller
    {
        public GameEntity_Controller_AI()
            : base(true)
        { }

        protected override Combat_Action Handle_CombatAction_Request(GameEntity_EntityField gameField)
        {
            Random rand = new Random();
            GameEntity[] players = null;// gameField.ConsciousPlayers;
            throw new NotImplementedException(); //TODO: fix above.
            GameEntity_ID targetId = players[rand.Next(players.Length)].Scene_GameObject_ID;

            //TODO: make combat ref GameScene
            Combat_Action ca = new Combat_Action();
            ca.Action_Owner = Entity.Scene_GameObject_ID;
            ca.Target.Add_Target(targetId);
            ca.Selected_Ability = MD_VANILLA_ABILITYNAMES.ABILITY_PUNCH;

            return ca;
        }

        public override GameEntity_Controller Clone()
        {
            return new GameEntity_Controller_AI();
        }
    }
}
