using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_RosterEntry<T>
    {
        public T Entity { get; internal set; }
        public GameEntity_ID GameEntity_ID => Entity.GameEntity_ID;
        public GameEntity_Roster_Id Roster_ID { get; private set; }
        public Multiplayer_Relay_ID Multiplayer_Relay_ID => Entity.Multiplayer_Relay_ID;
        public GameEntity_Position World_Position { get; private set; }
        
        public bool Is_Ready { get; internal set; }
        public bool Is_Incapacitated { get; internal set; }
        public bool Is_Actable { get; internal set; }

        internal GameEntity_RosterEntry(GameEntity boundEntity, GameEntity_Roster_Id rosterID)
        {
            Entity = boundEntity;
            Roster_ID = rosterID;
            World_Position = GameEntity_Position.Get_Position_From_Type(GameEntity_Position_Type.FRONT_RIGHT, rosterID);

            Is_Ready = false;
            Is_Incapacitated = false;
            Is_Actable = false;
        }
    }
}