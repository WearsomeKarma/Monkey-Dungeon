
namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public class Multiplayer_Relay_ID
    {
        public static readonly Multiplayer_Relay_ID ID_NULL = new Multiplayer_Relay_ID(-1);

        public readonly int ID;

        internal Multiplayer_Relay_ID(int id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return ID.ToString();
        }

        public static implicit operator int(Multiplayer_Relay_ID id)
            => id.ID;
    }
}
