using System;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public class Combat_Redirection_Chance : GameEntity_Quantity<GameEntity>
    {
        public static readonly double MAX_CHANCE = 1;
        public static readonly double MIN_CHANCE = 0;
        
        /// <summary>
        /// 70% chance to redirect to the front.
        /// </summary>
        public static Combat_Redirection_Chance FRONT_MELEE_ONTO_REAR
            = new Combat_Redirection_Chance(GameEntity_Position_Swap_Type.Swap_Vertical, 0.7);

        /// <summary>
        /// 30% chance to miss.
        /// </summary>
        public static Combat_Redirection_Chance FRONT_RANGED_ONTO_MELEE
            = new Combat_Redirection_Chance(GameEntity_Position_Swap_Type.Swap_Null, 0.3);

        /// <summary>
        /// 30% chance to miss.
        /// </summary>
        public static Combat_Redirection_Chance REAR_MELEE_ONTO_FRONT
            = new Combat_Redirection_Chance(GameEntity_Position_Swap_Type.Swap_Null, 0.3);

        /// <summary>
        /// 70% chance to miss.
        /// </summary>
        public static Combat_Redirection_Chance REAR_MELEE_ONTO_REAR
            = new Combat_Redirection_Chance(GameEntity_Position_Swap_Type.Swap_Null, 0.7);
        
        /// <summary>
        /// 30% chance to redirect to the front.
        /// </summary>
        public static Combat_Redirection_Chance REAR_RANGED_ONTO_REAR
            = new Combat_Redirection_Chance(GameEntity_Position_Swap_Type.Swap_Vertical, 0.3);

        /// <summary>
        /// 100% to cause no redirection.
        /// </summary>
        public static Combat_Redirection_Chance NO_REDIRECT
            = new Combat_Redirection_Chance(GameEntity_Position_Swap_Type.No_Swap, 1);
        
        
        
        
        
        
        
        public GameEntity_Position_Swap_Type Redirection_Chance__Redirection_Type { get; private set; }

        /// <summary>
        /// True if this is a constant chance given by MD_VANILLA_COMBAT.
        /// </summary>
        public readonly bool IS_BASE_CHANCE;
        public readonly GameEntity__Quantity_Modification_Type MODIFICATION_TYPE;
        
        public Combat_Redirection_Chance(GameEntity_Position_Swap_Type redirectionChanceRedirectionType, double chance, GameEntity__Quantity_Modification_Type modificationType = GameEntity__Quantity_Modification_Type.Additive)
            : base (GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME, MIN_CHANCE, MAX_CHANCE, chance)
        {
            Redirection_Chance__Redirection_Type = redirectionChanceRedirectionType;
            MODIFICATION_TYPE = modificationType;
        }

        internal Combat_Redirection_Chance(GameEntity_Position_Swap_Type redirectChanceRedirectionType, double chance)
            : base (GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME, MIN_CHANCE, MAX_CHANCE, chance)
        {
            Redirection_Chance__Redirection_Type = redirectChanceRedirectionType;
            IS_BASE_CHANCE = true;
            MODIFICATION_TYPE = GameEntity__Quantity_Modification_Type.None;
        }

        public void Combine__Redirection_Chance(Combat_Redirection_Chance chance)
        {
            Combat_Redirection_Chance combinedChance = Combine(this, chance);

            Redirection_Chance__Redirection_Type = chance.Redirection_Chance__Redirection_Type;
            Set__Value__Quantity(combinedChance.Value);
        }

        public bool Determine__If_Redirection_Occurs__Redirection_Chance()
        {
            Random rand = new Random();

            return rand.NextDouble() * Max_Quantity < Value;
        }
        
        
        
        public static GameEntity_Position Redirect(GameEntity_Position position, GameEntity_Position_Swap_Type redirectType)
        {
            GameEntity_Position newPos;

            switch (redirectType)
            {
                default:
                    return position;
                case GameEntity_Position_Swap_Type.Swap_Diagonal:
                    newPos = position.Get_Horizontal_Swap();
                    newPos = newPos.Get_Vertical_Swap();
                    return newPos;
                case GameEntity_Position_Swap_Type.Swap_Horizontal:
                    return newPos = position.Get_Horizontal_Swap();
                case GameEntity_Position_Swap_Type.Swap_Vertical:
                    return newPos = position.Get_Vertical_Swap();
                case GameEntity_Position_Swap_Type.Swap_Null:
                    return GameEntity_Position.NULL_POSITION;
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
                case GameEntity__Quantity_Modification_Type.Additive:
                    newChance = MathHelper.Clampd(chanceOne.Value + chanceTwo.Value, MIN_CHANCE, MAX_CHANCE);
                    break;
                case GameEntity__Quantity_Modification_Type.Multiplicative:
                    newChance = MathHelper.Clampd(chanceOne.Value * chanceTwo.Value, MIN_CHANCE, MAX_CHANCE);
                    break;
                default:
                    return chanceOne;
            }

            return new Combat_Redirection_Chance(chanceTwo.Redirection_Chance__Redirection_Type, newChance, chanceOne.MODIFICATION_TYPE);
        }
        
        public static explicit operator GameEntity_Position_Swap_Type(Combat_Redirection_Chance redirectionChance)
            => redirectionChance.Redirection_Chance__Redirection_Type;
        public static explicit operator double(Combat_Redirection_Chance redirectionChance)
            => redirectionChance.Value;
        

        public static Combat_Redirection_Chance Base_Redirection_Chance
            (
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type assaulterPositionType,
            GameEntity_Position_Type targetPositionType
            )
        {
            switch (assaulterPositionType)
            {
                case GameEntity_Position_Type.FRONT_LEFT:
                case GameEntity_Position_Type.FRONT_RIGHT:
                    return From_Front_Redirection_Chance(assaultType, targetPositionType);
                default:
                    return From_Rear_Redirection_Chance(assaultType, targetPositionType);
            }
        }

        private static Combat_Redirection_Chance From_Front_Redirection_Chance
            (
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type targetPositionType
            )
        {
            if (assaultType == Combat_Assault_Type.Melee)
            {
                switch (targetPositionType)
                {
                    case GameEntity_Position_Type.REAR_LEFT:
                    case GameEntity_Position_Type.REAR_RIGHT:
                        return FRONT_MELEE_ONTO_REAR;
                    default:
                        return NO_REDIRECT;
                }
            }

            switch (targetPositionType)
            {
                case GameEntity_Position_Type.REAR_LEFT:
                case GameEntity_Position_Type.REAR_RIGHT:
                    return REAR_RANGED_ONTO_REAR;
                default:
                    return NO_REDIRECT;
            }
        }

        private static Combat_Redirection_Chance From_Rear_Redirection_Chance
            (
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type targetPositionType
            )
        {
            if (assaultType == Combat_Assault_Type.Melee)
            {
                switch (targetPositionType)
                {
                    case GameEntity_Position_Type.FRONT_LEFT:
                    case GameEntity_Position_Type.FRONT_RIGHT:
                        return REAR_MELEE_ONTO_FRONT;
                    default:
                        return REAR_MELEE_ONTO_REAR;
                }
            }

            switch (targetPositionType)
            {
                case GameEntity_Position_Type.REAR_LEFT:
                case GameEntity_Position_Type.REAR_RIGHT:
                    return REAR_RANGED_ONTO_REAR;
                default:
                    return NO_REDIRECT;
            }
        }
    }
}