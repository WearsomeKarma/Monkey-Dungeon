using MonkeyDungeon_Core.GameFeatures.GameComponents.Entities;
using MonkeyDungeon_Core.GameFeatures.GameComponents.Controllers;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_ServerSide_Factory
    {
        private readonly Game_StateMachine GameState_Machine;

        private readonly Dictionary<GameEntity_Attribute_Name, GameEntity_ServerSide> GameEntity_Catalog = new Dictionary<GameEntity_Attribute_Name, GameEntity_ServerSide>()
        {
            //Players
            { MD_VANILLA_RACE_NAMES.RACE_MONKEY, new GameEntity_ServerSide() }, //TODO: Fix this awful thing with unique_id
            { MD_VANILLA_RACE_NAMES.CLASS_WARRIOR, new WarriorClass(MD_VANILLA_RACE_NAMES.RACE_MONKEY, 1, new GameEntityServerSideControllerPlayer()) { Unique_ID = 0 } },
            { MD_VANILLA_RACE_NAMES.CLASS_WIZARD, new WizardClass(MD_VANILLA_RACE_NAMES.RACE_MONKEY, 1, new GameEntityServerSideControllerPlayer()) { Unique_ID = 1 } },
            { MD_VANILLA_RACE_NAMES.CLASS_ARCHER, new ArcherClass(MD_VANILLA_RACE_NAMES.RACE_MONKEY, 1, new GameEntityServerSideControllerPlayer()) { Unique_ID = 2 } },
            { MD_VANILLA_RACE_NAMES.CLASS_CLERIC, new ClericClass(MD_VANILLA_RACE_NAMES.RACE_MONKEY, 1, new GameEntityServerSideControllerPlayer()) { Unique_ID = 3 } },
            { MD_VANILLA_RACE_NAMES.CLASS_KNIGHT, new KnightClass(MD_VANILLA_RACE_NAMES.RACE_MONKEY, 1, new GameEntityServerSideControllerPlayer()) { Unique_ID = 4 } },
            { MD_VANILLA_RACE_NAMES.CLASS_MONK, new MonkClass(MD_VANILLA_RACE_NAMES.RACE_MONKEY, 1, new GameEntityServerSideControllerPlayer()) { Unique_ID = 5 } },

            //Merchants

            //Enemies
            { MD_VANILLA_RACE_NAMES.RACE_GOBLIN, new EC_Goblin(1) }
        };
        public string[] Get_Races()
        {
            List<string> ret = new List<string>();

            foreach (GameEntity_ServerSide entitiy in GameEntity_Catalog.Values)
                if (!ret.Contains(entitiy.GameEntity__Race))
                    ret.Add(entitiy.GameEntity__Race);

            return ret.ToArray();
        }

        internal GameEntity_ServerSide_Factory(Game_StateMachine gameState_Machine)
        {
            GameState_Machine = gameState_Machine;
        }

        public void Add_Template(GameEntity_ServerSide gameEntityServerSide)
        {
            GameEntity_Catalog.Add(gameEntityServerSide.GameEntity__Race, gameEntityServerSide);
        }

        public GameEntity_ServerSide Create_NewEntity(GameEntity_ID entityScene_ID, Multiplayer_Relay_ID relayId, GameEntity_Position position,  GameEntity_Attribute_Name race)
        {
            GameEntity_ServerSide entityServerSide = GameEntity_Catalog[race].Clone__GameEntity(entityScene_ID);
            entityServerSide.Set_Position(position);
            return entityServerSide;
        }
    }
}
