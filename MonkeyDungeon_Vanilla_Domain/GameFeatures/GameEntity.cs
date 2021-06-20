using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity
    {
        public static readonly GameEntity NULL_ENTITY = new GameEntity();
        
        public GameEntity_ID GameEntity__ID { get; protected set; }
        public GameEntity_Team_ID GameEntity__Team_ID => GameEntity__ID.Team_Id;
        public GameEntity_Position GameEntity__Position { get; protected set; }
        public Multiplayer_Relay_ID GameEntity__Multiplayer_Relay_ID => GameEntity__ID.Relay_ID;
        
        public GameEntity_Attribute_Name_Race GameEntity__Race { get; protected set; }
        
        public uint GameEntity__Cosmetic_ID { get; protected set; }
        
        public bool GameEntity__Is_Incapacitated { get; protected set; }
        public bool GameEntity__Is_Ready { get; protected set; }
        public bool GameEntity__Is_Dismissed { get; protected set; }
        public bool GameEntity__Is_Not_Present => GameEntity__Is_Incapacitated || GameEntity__Is_Dismissed;
        
        public GameEntity
        (
            GameEntity_ID gameEntityID = null,
            GameEntity_Position position = null,

            GameEntity_Attribute_Name_Race race = null,

            uint gameEntityCosmeticId = 0,

            bool gameEntityIsIncapacitated = false,
            bool gameEntityIsReady = false
        )
        {
            GameEntity__ID        = gameEntityID  ??  GameEntity_ID.ID_NULL;
            GameEntity__Position  = position      ??  GameEntity_Position.NULL_POSITION;
 
            GameEntity__Race      = race          ??  MD_VANILLA_RACE_NAMES.RACE_MONKEY;

            GameEntity__Cosmetic_ID = gameEntityCosmeticId;

            GameEntity__Is_Incapacitated = gameEntityIsIncapacitated;
            GameEntity__Is_Ready = gameEntityIsReady;
        }

        public static bool Validate(GameEntity entity)
            => entity != null && entity != NULL_ENTITY;
    }
}