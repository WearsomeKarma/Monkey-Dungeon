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
            = Create__Base_Chance(GameEntity_Position_Swap_Type.Swap_Vertical, 0.7);

        /// <summary>
        /// 30% chance to miss.
        /// </summary>
        public static Combat_Redirection_Chance FRONT_RANGED_ONTO_MELEE
            = Create__Base_Chance(GameEntity_Position_Swap_Type.Swap_To_Null, 0.3);

        /// <summary>
        /// 30% chance to miss.
        /// </summary>
        public static Combat_Redirection_Chance REAR_MELEE_ONTO_FRONT
            = Create__Base_Chance(GameEntity_Position_Swap_Type.Swap_To_Null, 0.3);

        /// <summary>
        /// 70% chance to miss.
        /// </summary>
        public static Combat_Redirection_Chance REAR_MELEE_ONTO_REAR
            = Create__Base_Chance(GameEntity_Position_Swap_Type.Swap_To_Null, 0.7);
        
        /// <summary>
        /// 30% chance to redirect to the front.
        /// </summary>
        public static Combat_Redirection_Chance REAR_RANGED_ONTO_REAR
            = Create__Base_Chance(GameEntity_Position_Swap_Type.Swap_Vertical, 0.3);

        /// <summary>
        /// 100% to cause no redirection.
        /// </summary>
        public static Combat_Redirection_Chance NO_REDIRECT
            = Create__Base_Chance(GameEntity_Position_Swap_Type.No_Swap, 1);
        
        /// <summary>
        /// Use this to cause no modification to the redirection.
        /// </summary>
        public static Combat_Redirection_Chance NULL_REDIRECT
            = Create__Base_Chance(GameEntity_Position_Swap_Type.Swap_Is_Null, 1, GameEntity__Quantity_Modification_Type.Multiplicative);
        
        
        
        
        
        
        public GameEntity_Position_Swap_Type Redirection_Chance__Redirection_Type { get; private set; }

        /// <summary>
        /// True if this is a constant chance given by MD_VANILLA_COMBAT.
        /// </summary>
        public bool Redirection_Chance__Is_Base_Chance { get; internal set; }
        public readonly GameEntity__Quantity_Modification_Type MODIFICATION_TYPE;
        
        public Combat_Redirection_Chance
            (
            GameEntity_Position_Swap_Type redirectionChanceRedirectionType, 
            double chance, 
            GameEntity__Quantity_Modification_Type modificationType = GameEntity__Quantity_Modification_Type.Additive
            )
            : base 
                (
                GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME, 
                chance,
                MIN_CHANCE, 
                MAX_CHANCE
                )
        {
            Redirection_Chance__Redirection_Type = redirectionChanceRedirectionType;
            MODIFICATION_TYPE = modificationType;
        }

        public bool Determine__If_Redirection_Occurs__Redirection_Chance()
        {
            Random rand = new Random();

            return rand.NextDouble() * Quantity__Maximal_Value < Quantity__Value;
        }

        public sealed override void Modify__By_Quantity__Quantity(GameEntity_Quantity<GameEntity> quantity,
            GameEntity__Quantity_Modification_Type modificationType)
        {
            if (Redirection_Chance__Is_Base_Chance)
                return;
            
            Combat_Redirection_Chance quantityAsRedirection = quantity as Combat_Redirection_Chance;
            GameEntity_Position_Swap_Type quantitySwapType =
                quantityAsRedirection?.Redirection_Chance__Redirection_Type ??
                GameEntity_Position_Swap_Type.Swap_Is_Null;
            
            base.Modify__By_Quantity__Quantity(quantity, modificationType);

            if (quantityAsRedirection != null && quantitySwapType != GameEntity_Position_Swap_Type.Swap_Is_Null)
                Redirection_Chance__Redirection_Type = quantitySwapType;
        }



        private static Combat_Redirection_Chance Create__Base_Chance
            (
            GameEntity_Position_Swap_Type swapType,
            double chance,
            GameEntity__Quantity_Modification_Type modificationType = GameEntity__Quantity_Modification_Type.Mutate
            )
        {
            Combat_Redirection_Chance baseChance = new Combat_Redirection_Chance(swapType, chance, modificationType);
            baseChance.Redirection_Chance__Is_Base_Chance = true;

            return baseChance;
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
                case GameEntity_Position_Swap_Type.Swap_To_Null:
                    return GameEntity_Position.NULL_POSITION;
            }
        }

        public static explicit operator GameEntity_Position_Swap_Type(Combat_Redirection_Chance redirectionChance)
            => redirectionChance.Redirection_Chance__Redirection_Type;
        public static explicit operator double(Combat_Redirection_Chance redirectionChance)
            => redirectionChance.Quantity__Value;
        

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