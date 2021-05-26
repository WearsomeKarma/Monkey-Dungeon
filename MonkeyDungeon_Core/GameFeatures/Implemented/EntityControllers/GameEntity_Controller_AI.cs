using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.Implemented.Abilities;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.EntityControllers
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
            ca.Owner_OfCombatAction = Entity;
            ca.Target_ID = targetId;
            ca.Set_Ability(MD_VANILLA_ABILITYNAMES.ABILITY_PUNCH);

            return ca;
        }

        public override GameEntity_Controller Clone()
        {
            return new GameEntity_Controller_AI();
        }
    }
}
