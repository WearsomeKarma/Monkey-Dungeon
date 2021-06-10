using MonkeyDungeon_Core.GameFeatures.GameEntities;
using System;

namespace MonkeyDungeon_Core.GameFeatures.Exceptions
{
    public class EntityStat_NotFound_Exception : Exception
    {
        public readonly GameEntity_ServerSide EntityServerSide;

        public EntityStat_NotFound_Exception(GameEntity_ServerSide entityServerSide, string statName)
            : base(String.Format(
                "Requested stat was not found: {0}\n Did you misspell the stat name?",
                (statName.Length > 10 ? statName.Substring(0,10) + "..." : statName)                
                ))
        {
            EntityServerSide = entityServerSide;
        }
    }
}
