using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats
{
    public class GameEntity_Stat_Manager
    {
        private readonly GameEntity Entity;

        private readonly List<GameEntity_Stat> Stats        = new List<GameEntity_Stat>();
        public GameEntity_Stat[] Get_Stats                  () => Stats.ToArray();
        public T Get_Stat<T>                                (GameEntity_Attribute_Name statName=null) where T : GameEntity_Stat { foreach (T stat in Stats) return stat; return null; }
        public GameEntity_Stat Get_Stat                     (GameEntity_Attribute_Name statName) => Get_Stat<GameEntity_Stat>(statName);
        public void Add_Stat                                (GameEntity_Stat stat) { Stats.Add(stat); stat.Attach_To_Entity(Entity); }

        public GameEntity_Stat_Manager(GameEntity managedEntity, List<GameEntity_Stat> stats = null)
        {
            Entity = managedEntity;

            if (stats != null)
                foreach (GameEntity_Stat stat in stats)
                    Add_Stat(stat);
        }
    }
}
