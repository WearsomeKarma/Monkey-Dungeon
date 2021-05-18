using MonkeyDungeon_Core.GameFeatures.Implemented.Entities.Enemies.Goblins;
using MonkeyDungeon_Core.GameFeatures.Implemented.Entities.PlayerClasses;
using MonkeyDungeon_Core.GameFeatures.Implemented.EntityControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Factory
    {
        private readonly GameState_Machine GameState_Machine;

        private readonly Dictionary<string, GameEntity> GameEntity_Catalog = new Dictionary<string, GameEntity>()
        {
            //Players
            { GameEntity.RACE_NAME_PLAYER, new GameEntity() }, //TODO: Fix this awful thing with unique_id
            { WarriorClass.CLASS_NAME, new WarriorClass(GameEntity.RACE_NAME_PLAYER, 1, new GameEntity_Controller_Player()) { Unique_ID = 0 } },
            { WizardClass.CLASS_NAME, new WizardClass(GameEntity.RACE_NAME_PLAYER, 1, new GameEntity_Controller_Player()) { Unique_ID = 1 } },
            { ArcherClass.CLASS_NAME, new ArcherClass(GameEntity.RACE_NAME_PLAYER, 1, new GameEntity_Controller_Player()) { Unique_ID = 2 } },
            { ClericClass.CLASS_NAME, new ClericClass(GameEntity.RACE_NAME_PLAYER, 1, new GameEntity_Controller_Player()) { Unique_ID = 3 } },
            { KnightClass.CLASS_NAME, new KnightClass(GameEntity.RACE_NAME_PLAYER, 1, new GameEntity_Controller_Player()) { Unique_ID = 4 } },
            { MonkClass.CLASS_NAME, new MonkClass(GameEntity.RACE_NAME_PLAYER, 1, new GameEntity_Controller_Player()) { Unique_ID = 5 } },

            //Merchants

            //Enemies
            { EC_Goblin.DEFAULT_RACE_NAME, new EC_Goblin(1) }
        };
        public string[] Get_Races()
        {
            List<string> ret = new List<string>();

            foreach (GameEntity entitiy in GameEntity_Catalog.Values)
                if (!ret.Contains(entitiy.Race))
                    ret.Add(entitiy.Race);

            return ret.ToArray();
        }

        internal GameEntity_Factory(GameState_Machine gameState_Machine)
        {
            GameState_Machine = gameState_Machine;
        }

        public void Add_Template(GameEntity gameEntity)
        {
            GameEntity_Catalog.Add(gameEntity.Name, gameEntity);
        }

        public GameEntity Create_NewEntity(int entityScene_ID, string name)
            => GameEntity_Catalog[name].Clone(entityScene_ID);
    }
}
