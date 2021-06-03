using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_RosterEntry
    {
        public GameEntity Game_Entity { get; internal set; }
        public GameEntity_ID GameEntity_ID => Game_Entity.GameEntity_ID;
        public Multiplayer_Relay_ID Multiplayer_Relay_ID => Game_Entity.Multiplayer_Relay_ID;
        public GameEntity_Position World_Position { get; private set; }
        
        public bool Is_Ready { get; internal set; }
        public bool Is_Incapacitated { get; internal set; }
        public bool Is_Actable { get; internal set; }

        internal GameEntity_RosterEntry(GameEntity boundEntity)
        {
            Game_Entity = boundEntity;
            World_Position = (GameEntity_Position) boundEntity.GameEntity_ID;

            Is_Ready = false;
            Is_Incapacitated = false;
            Is_Actable = false;
        }
    }
}