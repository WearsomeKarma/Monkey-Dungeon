using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats
{
    public sealed class GameEntity_Stat_Manager
    {
        private readonly GameEntity_ServerSide ATTACHED_ENTITY;

        private readonly List<GameEntity_ServerSide_Stat> Stats        = new List<GameEntity_ServerSide_Stat>();
        public GameEntity_ServerSide_Stat[] Get__Stats                  () => Stats.ToArray();
        public T Get__Stat<T>                                (GameEntity_Attribute_Name statName=null) where T : GameEntity_ServerSide_Stat { foreach (T stat in Stats) return stat; return null; }
        public GameEntity_ServerSide_Stat Get__Stat                     (GameEntity_Attribute_Name statName) => Get__Stat<GameEntity_ServerSide_Stat>(statName);
        public void Add__Stat                                (GameEntity_ServerSide_Stat stat) { Stats.Add(stat); stat.Attach_To__Entity__ServerSide_Stat(ATTACHED_ENTITY); }

        internal GameEntity_Stat_Manager(GameEntity_ServerSide managedAttachedEntity, List<GameEntity_ServerSide_Stat> stats = null)
        {
            ATTACHED_ENTITY = managedAttachedEntity;

            if (stats != null)
                foreach (GameEntity_ServerSide_Stat stat in stats)
                    Add__Stat(stat);
        }
    }
}
