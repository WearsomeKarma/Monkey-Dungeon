namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Position
    {
        public static GameEntity_Position NULL_POSITION = new GameEntity_Position(-1);
        public static GameEntity_Position FRONT_RIGHT = new GameEntity_Position(0);
        public static GameEntity_Position FRONT_LEFT = new GameEntity_Position(1);
        public static GameEntity_Position REAR_RIGHT = new GameEntity_Position(2);
        public static GameEntity_Position REAR_LEFT = new GameEntity_Position(3);

        public static GameEntity_Position[] POSITIONS = new GameEntity_Position[]
        {
            FRONT_RIGHT,
            FRONT_LEFT,
            REAR_RIGHT,
            REAR_LEFT
        };
        
        public int World_Position { get; private set; }
        
        private int Modulo_Position => World_Position / 2; //0 or 1
        private int Modulo_Position_Low => (Modulo_Position) * MD_PARTY.MAX_PARTY_SIZE_HALF;
        private int Modulo_Position_High => (Modulo_Position + 1) * MD_PARTY.MAX_PARTY_SIZE_HALF;

        public int Horizontal_Adjacent_Position => (Modulo_Position_Low) + ((World_Position + 1) % MD_PARTY.MAX_PARTY_SIZE_HALF);
        public int Vertical_Adjacent_Position => (World_Position + MD_PARTY.MAX_PARTY_SIZE_3_2) % MD_PARTY.MAX_PARTY_SIZE;
        
        internal GameEntity_Position(int initalPosition)
        {
            World_Position = (int) initalPosition;
        }

        public GameEntity_Position Get_Horizontal_Swap()
        {
            if (this == NULL_POSITION)
                return this;
            return POSITIONS[Horizontal_Adjacent_Position];
        }

        public GameEntity_Position Get_Vertical_Swap()
        {
            if (this == NULL_POSITION)
                return this;
            return POSITIONS[Vertical_Adjacent_Position];
        }

        public override string ToString()
        {
            return ((GameEntity_Position_Type) World_Position).ToString();
        }

        public static explicit operator GameEntity_Position(GameEntity_Position_Type type)
            => POSITIONS[(int) type];

        public static explicit operator GameEntity_Position_Type(GameEntity_Position position)
            => (GameEntity_Position_Type) (position.World_Position);

        public static explicit operator GameEntity_Position(GameEntity_ID id)
            => POSITIONS[(id > -1) ? id % MD_PARTY.MAX_PARTY_SIZE : 0];
    }
}