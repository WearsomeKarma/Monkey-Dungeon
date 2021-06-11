using System.Diagnostics;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public static class MD_VANILLA_COMBAT
    {
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