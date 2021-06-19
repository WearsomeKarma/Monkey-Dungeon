using System;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat
{
    public class Combat_Survey_Damage<T> : GameEntity_Survey_Quantity<T, GameEntity_Damage<T>> where T : GameEntity
    {
        public Combat_Survey_Damage(Func<GameEntity_Damage<T>> defaultQuantitizer) 
            : base(defaultQuantitizer)
        {
        }

        public void Set__Damage__Survey_Damage(GameEntity_Position position, GameEntity_Damage<T> damage)
        {
            FIELD[position] = damage;
        }
    }
}