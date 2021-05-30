using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.EntityControllers
{
    public class GameEntity_Controller_AI : GameEntity_Controller
    {
        public GameEntity_Controller_AI()
            : base(true)
        { }

        protected override Combat_Action Handle_CombatAction_Request(Combat_GameState combat)
        {
            Random rand = new Random();
            GameEntity[] players = combat.ConsciousPlayers;
            int targetId = players[rand.Next(players.Length)].Scene_GameObject_ID;

            //TODO: make combat ref GameScene
            Combat_Action ca = new Combat_Action();
            ca.Action_Owner = Entity;
            ca.Target.Add_Target(targetId);
            ca.Set_Ability(MD_VANILLA_ABILITYNAMES.ABILITY_PUNCH);

            return ca;
        }

        public override GameEntity_Controller Clone()
        {
            return new GameEntity_Controller_AI();
        }
    }
}
