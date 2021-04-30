using MonkeyDungeon.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.Exceptions
{
    public class EntityStat_NotFound_Exception : Exception
    {
        public readonly EntityComponent Entity;

        public EntityStat_NotFound_Exception(EntityComponent entity, string statName)
            : base(String.Format(
                "Requested stat was not found: {0}\n Did you misspell the stat name?",
                (statName.Length > 10 ? statName.Substring(0,10) + "..." : statName)                
                ))
        {
            Entity = entity;
        }
    }
}
