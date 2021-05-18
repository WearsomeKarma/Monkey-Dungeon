using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.EntityResourceManagement
{
    public class GameEntity_Resistance_Manager
    {
        private readonly GameEntity Entity;

        private readonly List<GameEntity_Resistance> Resistances                            = new List<GameEntity_Resistance>();
        public GameEntity_Resistance[] Get_Resistances()                                     => Resistances.ToArray();
        public GameEntity_Resistance Get_Resistance(DamageType typeOfDamage_Resisting)      { foreach (GameEntity_Resistance resistance in Resistances) if (resistance.TypeOfDamage_Resisting == typeOfDamage_Resisting) return resistance; return null; }
        public float Get_Resistance_Value(DamageType typeOfDamage_Resisting)                { GameEntity_Resistance r = Get_Resistance(typeOfDamage_Resisting); return (r == null) ? 1 : r.ResistancePercentage; }
        public void Add_Resistance(GameEntity_Resistance resistance)                        { Resistances.Add(resistance); }

        public GameEntity_Resistance_Manager(GameEntity managedEntity, List<GameEntity_Resistance> resistances = null)
        {
            Entity = managedEntity;

            if (resistances != null)
                foreach (GameEntity_Resistance resistance in resistances)
                    Add_Resistance(resistance);
        }
    }
}
