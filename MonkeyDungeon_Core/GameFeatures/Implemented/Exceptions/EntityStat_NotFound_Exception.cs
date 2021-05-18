using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.Exceptions
{
    public class EntityStat_NotFound_Exception : Exception
    {
        public readonly GameEntity Entity;

        public EntityStat_NotFound_Exception(GameEntity entity, string statName)
            : base(String.Format(
                "Requested stat was not found: {0}\n Did you misspell the stat name?",
                (statName.Length > 10 ? statName.Substring(0,10) + "..." : statName)                
                ))
        {
            Entity = entity;
        }
    }
}
