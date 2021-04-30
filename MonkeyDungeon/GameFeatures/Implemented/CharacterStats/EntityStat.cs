using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.CharacterStats
{
    public static class ENTITY_STATS
    {
        // Stats
        public static readonly string STRENGTH = "Strength";
        public static readonly string AGILITY = "Agility";
        public static readonly string SMARTYPANTS = "Smartypants";
        public static readonly string STINKINESS = "Stinkiness";
    }

    public class EntityStat : EntityResource
    {
        public EntityStat(string statName, float baseValue, float maxProgresionRate, float max=-1) 
            : base(statName, baseValue, (max < 0 ? baseValue : max), 0, 0, maxProgresionRate, 0)
        {
        }

        public new EntityStat Clone()
        {
            return new EntityStat(
                Resource_Name,
                Get_BaseValue(),
                Max_Value.ScalingRate,
                Max_Value.Value
                );
        }
    }
}
