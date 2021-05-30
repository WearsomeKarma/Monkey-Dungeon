using MonkeyDungeon_Core.GameFeatures.GameEntities.Entities;
using MonkeyDungeon_Core.GameFeatures.GameEntities.EntityControllers;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System.Collections.Generic;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Factory
    {
        private readonly GameState_Machine GameState_Machine;

        private readonly Dictionary<string, GameEntity> GameEntity_Catalog = new Dictionary<string, GameEntity>()
        {
            //Players
            { MD_VANILLA_RACES.RACE_MONKEY, new GameEntity() }, //TODO: Fix this awful thing with unique_id
            { WarriorClass.CLASS_NAME, new WarriorClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 0 } },
            { WizardClass.CLASS_NAME, new WizardClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 1 } },
            { ArcherClass.CLASS_NAME, new ArcherClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 2 } },
            { ClericClass.CLASS_NAME, new ClericClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 3 } },
            { KnightClass.CLASS_NAME, new KnightClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 4 } },
            { MonkClass.CLASS_NAME, new MonkClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 5 } },

            //Merchants

            //Enemies
            { MD_VANILLA_RACES.RACE_GOBLIN, new EC_Goblin(1) }
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

        public GameEntity Create_NewEntity(GameEntity_ID entityScene_ID, int relayId, string race)
        {
            GameEntity entity = GameEntity_Catalog[race].Clone();

            entity.Scene_GameObject_ID = entityScene_ID;
            entity.Relay_ID_Of_Owner = relayId;

            return entity;
        }
    }
}
