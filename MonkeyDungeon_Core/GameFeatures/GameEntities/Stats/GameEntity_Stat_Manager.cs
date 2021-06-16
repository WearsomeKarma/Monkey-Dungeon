﻿using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats
{
    public sealed class GameEntity_Stat_Manager
    {
        private readonly GameEntity_ServerSide ATTACHED_ENTITY;

        private readonly List<GameEntity_Stat> Stats        = new List<GameEntity_Stat>();
        public GameEntity_Stat[] Get__Stats                  () => Stats.ToArray();
        public T Get__Stat<T>                                (GameEntity_Attribute_Name statName=null) where T : GameEntity_Stat { foreach (T stat in Stats) return stat; return null; }
        public GameEntity_Stat Get__Stat                     (GameEntity_Attribute_Name statName) => Get__Stat<GameEntity_Stat>(statName);
        public void Add__Stat                                (GameEntity_Stat stat) { Stats.Add(stat); stat.Attach_To_Entity(ATTACHED_ENTITY); }

        internal GameEntity_Stat_Manager(GameEntity_ServerSide managedAttachedEntity, List<GameEntity_Stat> stats = null)
        {
            ATTACHED_ENTITY = managedAttachedEntity;

            if (stats != null)
                foreach (GameEntity_Stat stat in stats)
                    Add__Stat(stat);
        }
    }
}
