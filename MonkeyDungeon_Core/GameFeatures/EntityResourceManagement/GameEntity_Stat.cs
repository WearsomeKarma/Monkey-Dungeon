using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.EntityResourceManagement
{
    public class GameEntity_Stat : GameEntity_Resource
    {
        public GameEntity_Stat(string statName, double baseValue, double maxProgresionRate, double max =-1) 
            : base(statName, baseValue, (max < 0 ? baseValue : max), 0, 0, maxProgresionRate, 0)
        {
        }

        public new GameEntity_Stat Clone()
        {
            return new GameEntity_Stat(
                Resource_Name,
                Get_BaseValue(),
                Max_Value.Scaling_Rate,
                Max_Value.Value
                );
        }
    }
}
