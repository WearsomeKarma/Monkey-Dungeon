using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity
    {
        public static readonly GameEntity NULL_ENTITY = new GameEntity();
        
        public GameEntity_ID GameEntity_ID { get; protected set; }
        public GameEntity_Team_ID GameEntity_Team_ID => GameEntity_ID.Team_Id;
        public GameEntity_Position GameEntity_Position { get; protected set; }
        public Multiplayer_Relay_ID Multiplayer_Relay_ID => GameEntity_ID.Relay_ID;
        
        public GameEntity_Attribute_Name_Race GameEntity_Race { get; protected set; }
        
        public int GameEntity_Cosmetic_ID { get; protected set; }
        
        public bool IsIncapacitated { get; protected set; }
        public bool IsReady { get; protected set; }
        
        public GameEntity
        (
            GameEntity_ID gameEntityID = null,
            GameEntity_Position position = null,

            GameEntity_Attribute_Name_Race race = null,

            int gameEntityCosmeticId = 0,

            bool isIncapacitated = false,
            bool isReady = false
        )
        {
            GameEntity_ID        = gameEntityID  ??  GameEntity_ID.ID_NULL;
            GameEntity_Position  = position      ??  GameEntity_Position.ID_NULL;
 
            GameEntity_Race      = race          ??  MD_VANILLA_RACES.RACE_MONKEY;

            GameEntity_Cosmetic_ID = gameEntityCosmeticId;

            IsIncapacitated = isIncapacitated;
            IsReady = isReady;
        }

        public static bool Validate(GameEntity entity)
            => entity != null || entity != NULL_ENTITY;
    }
}