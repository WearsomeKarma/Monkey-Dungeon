using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Description
    {
        public GameEntity_ID GameEntity_ID { get; internal set; }
        public GameEntity_Roster_Id GameEntity_Roster_ID { get; internal set; }
        public Multiplayer_Relay_ID Multiplayer_Relay_ID { get; internal set; }
        
        public GameEntity_Attribute_Name GameEntity_Race { get; internal set; }
        
        /// <summary>
        /// TODO: prim wrap and internal set.
        /// </summary>
        public int GameEntity_Cosmetic_ID { get; set; }
    }
}