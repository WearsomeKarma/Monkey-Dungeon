using MonkeyDungeon_Core.GameFeatures.GameEntities.Entities;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Controllers;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Factory
    {
        private readonly GameState_Machine GameState_Machine;

        private readonly Dictionary<GameEntity_Attribute_Name, GameEntity> GameEntity_Catalog = new Dictionary<GameEntity_Attribute_Name, GameEntity>()
        {
            //Players
            { MD_VANILLA_RACES.RACE_MONKEY, new GameEntity() }, //TODO: Fix this awful thing with unique_id
            { MD_VANILLA_RACES.CLASS_WARRIOR, new WarriorClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 0 } },
            { MD_VANILLA_RACES.CLASS_WIZARD, new WizardClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 1 } },
            { MD_VANILLA_RACES.CLASS_ARCHER, new ArcherClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 2 } },
            { MD_VANILLA_RACES.CLASS_CLERIC, new ClericClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 3 } },
            { MD_VANILLA_RACES.CLASS_KNIGHT, new KnightClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 4 } },
            { MD_VANILLA_RACES.CLASS_MONK, new MonkClass(MD_VANILLA_RACES.RACE_MONKEY, 1, new GameEntity_Controller_Player()) { Unique_ID = 5 } },

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
            GameEntity_Catalog.Add(gameEntity.Race, gameEntity);
        }

        public GameEntity Create_NewEntity(GameEntity_ID entityScene_ID, Multiplayer_Relay_ID relayId, GameEntity_Attribute_Name race)
        {
            GameEntity entity = GameEntity_Catalog[race].Clone(entityScene_ID);
            return entity;
        }
    }
}
