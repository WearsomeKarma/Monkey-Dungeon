using MonkeyDungeon.GameFeatures.Implemented.Entities.Enemies.Goblins;
using MonkeyDungeon.GameFeatures.Implemented.Entities.PlayerClasses;
using MonkeyDungeon.GameFeatures.Implemented.EntityControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class GameEntity_Factory
    {
        private readonly GameState_Machine GameState_Machine;

        private readonly Dictionary<string, GameEntity> GameEntity_Catalog = new Dictionary<string, GameEntity>()
        {
            //Players
            { GameEntity.RACE_NAME_PLAYER, new GameEntity() },
            { WarriorClass.CLASS_NAME, new WarriorClass("player", 1, new GameEntity_Controller_Player()) },
            { WizardClass.CLASS_NAME, new WizardClass("player", 1, new GameEntity_Controller_Player()) },
            { ArcherClass.CLASS_NAME, new ArcherClass("player", 1, new GameEntity_Controller_Player()) },
            { ClericClass.CLASS_NAME, new ClericClass("player", 1, new GameEntity_Controller_Player()) },
            { KnightClass.CLASS_NAME, new KnightClass("player", 1, new GameEntity_Controller_Player()) },
            { MonkClass.CLASS_NAME, new MonkClass("player", 1, new GameEntity_Controller_Player()) },

            //Merchants

            //Enemies
            { EC_Goblin.DEFAULT_RACE_NAME, new EC_Goblin(1) }
        };

        internal GameEntity_Factory(GameState_Machine gameState_Machine)
        {
            GameState_Machine = gameState_Machine;
        }

        public void Add_Template(GameEntity gameEntity)
        {
            GameEntity_Catalog.Add(gameEntity.Name, gameEntity);
        }

        public GameEntity Create_NewEntity(string name)
            => GameEntity_Catalog[name].Clone();
    }
}
