using System;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_ID
    {
        public static readonly GameEntity_ID ID_NULL    = new GameEntity_ID(-1);
        public static readonly GameEntity_ID ID_ZERO    = new GameEntity_ID(0);
        public static readonly GameEntity_ID ID_ONE     = new GameEntity_ID(1);
        public static readonly GameEntity_ID ID_TWO     = new GameEntity_ID(2);
        public static readonly GameEntity_ID ID_THREE   = new GameEntity_ID(3);
        public static readonly GameEntity_ID ID_FOUR    = new GameEntity_ID(4);
        public static readonly GameEntity_ID ID_FIVE    = new GameEntity_ID(5);
        public static readonly GameEntity_ID ID_SIX     = new GameEntity_ID(6);
        public static readonly GameEntity_ID ID_SEVEN   = new GameEntity_ID(7);

        public static readonly GameEntity_ID[] IDS = new GameEntity_ID[]
        {
            ID_ZERO,
            ID_ONE,
            ID_TWO,
            ID_THREE,
            ID_FOUR,
            ID_FIVE,
            ID_SIX,
            ID_SEVEN
        };

        public readonly int ID;
        public GameEntity_Roster_Id Roster_ID => GameEntity_Roster_Id.ROSTER_IDS[ID / MD_PARTY.MAX_PARTY_SIZE];
        public Multiplayer_Relay_ID Relay_ID { get; internal set; }
        public bool IsRelay_Bound => Relay_ID != Multiplayer_Relay_ID.NULL_ID;

        internal GameEntity_ID(int id, Multiplayer_Relay_ID rid = null)
        {
            ID = id;
            Relay_ID = rid ?? Multiplayer_Relay_ID.NULL_ID;
        }

        public override string ToString()
        {
            return String.Format("{0}  R:{1}", ID, Relay_ID);
        }

        public static implicit operator int(GameEntity_ID gameEntity_ID)
            => gameEntity_ID?.ID ?? ID_NULL.ID;
    }
}
