namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public class Combat_Redirection_Chance
    {
        public static readonly double MAX_CHANCE = 1;
        public static readonly double MIN_CHANCE = 0;
        
        public readonly Combat_Redirect_Type REDIRECTION;
        public readonly double CHANCE;

        /// <summary>
        /// True if this is a constant chance given by MD_VANILLA_COMBAT.
        /// </summary>
        public readonly bool IS_BASE_CHANCE;
        public readonly Quantity_Modification_Type MODIFICATION_TYPE;
        
        public Combat_Redirection_Chance(Combat_Redirect_Type redirection, double chance, Quantity_Modification_Type modificationType = Quantity_Modification_Type.Additive)
        {
            REDIRECTION = redirection;
            CHANCE = MathHelper.Clampd(chance, MIN_CHANCE, MAX_CHANCE);
            MODIFICATION_TYPE = modificationType;
        }

        internal Combat_Redirection_Chance(Combat_Redirect_Type redirectType, double chance)
        {
            REDIRECTION = redirectType;
            CHANCE = MathHelper.Clampd(chance, MIN_CHANCE, MAX_CHANCE);
            IS_BASE_CHANCE = true;
            MODIFICATION_TYPE = Quantity_Modification_Type.None;
        }

        public static GameEntity_Position Redirect(GameEntity_Position position, Combat_Redirect_Type redirectType)
        {
            GameEntity_Position newPos;

            switch (redirectType)
            {
                default:
                    return position;
                case Combat_Redirect_Type.Redirect_Diagonal:
                    newPos = position.Get_Horizontal_Swap();
                    newPos = newPos.Get_Vertical_Swap();
                    return newPos;
                case Combat_Redirect_Type.Redirect_Horizontal:
                    return newPos = position.Get_Horizontal_Swap();
                case Combat_Redirect_Type.Redirect_Vertical:
                    return newPos = position.Get_Vertical_Swap();
                case Combat_Redirect_Type.Redirect_Null:
                    return GameEntity_Position.__NULL____POSITION;
            }
        }
        
        /// <summary>
        /// Non commutative. Returns a new Combat_Redirection_Chance as:
        /// chanceTwo.REDIRECTION, combined-chance, chanceOne.MODIFICATION_TYPE.
        /// If chanceTwo is none, returns chanceOne.
        /// </summary>
        /// <param name="chanceOne"></param>
        /// <param name="chanceTwo"></param>
        /// <returns></returns>
        public static Combat_Redirection_Chance Combine
            (
            Combat_Redirection_Chance chanceOne,
            Combat_Redirection_Chance chanceTwo
            )
        {
            double newChance;
            switch (chanceTwo.MODIFICATION_TYPE)
            {
                case Quantity_Modification_Type.Additive:
                    newChance = MathHelper.Clampd(chanceOne.CHANCE + chanceTwo.CHANCE, MIN_CHANCE, MAX_CHANCE);
                    break;
                case Quantity_Modification_Type.Multiplicative:
                    newChance = MathHelper.Clampd(chanceOne.CHANCE * chanceTwo.CHANCE, MIN_CHANCE, MAX_CHANCE);
                    break;
                default:
                    return chanceOne;
            }

            return new Combat_Redirection_Chance(chanceTwo.REDIRECTION, newChance, chanceOne.MODIFICATION_TYPE);
        }
        
        public static explicit operator Combat_Redirect_Type(Combat_Redirection_Chance redirectionChance)
            => redirectionChance.REDIRECTION;
        public static explicit operator double(Combat_Redirection_Chance redirectionChance)
            => redirectionChance.CHANCE;
    }
}