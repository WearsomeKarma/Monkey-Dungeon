using isometricgame.GameEngine;
using isometricgame.GameEngine.Systems;
using MonkeyDungeon.Components;
using MonkeyDungeon.Components.Implemented.Enemies.Goblins;
using MonkeyDungeon.Components.Implemented.PlayerClasses;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.Prefabs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameSystems
{
    public class EntityComponentFactory : GameSystem
    {
        private List<EntityComponent> entityComponents;
        public List<EntityComponent> EntityComponents { get => entityComponents.ToList(); }

        public EntityComponentFactory(Game game) 
            : base(game)
        {
            entityComponents = new List<EntityComponent>()
            {
                //player classes
                new ArcherClass("player", 1, null),
                new ClericClass("player", 1, null),
                new KnightClass("player", 1, null),
                new MonkClass("player", 1, null),
                new WarriorClass("player", 1, null),
                new WizardClass("player", 1, null),

                //NPCs
                new EC_Goblin(1)
            };
        }

        public T GetNew_EntityComponent<T>() where T : EntityComponent
        {
            foreach (EntityComponent ec in entityComponents)
                if (ec is T)
                    return (T)ec.Clone();
            return null;
        }

        public T GetNew_EntityComponent<T>(string ec_typeName) where T : EntityComponent
        {
            foreach (EntityComponent ec in entityComponents)
                if (ec is T && ec.TypeName == ec_typeName)
                    return (T)ec.Clone();
            return null;
        }

        public EntityComponent New_EntityComponent(string ec_typeName, EntityController actingEntity = null)
        {
            EntityComponent ec = GetNew_EntityComponent<EntityComponent>(ec_typeName);
            if (actingEntity != null)
                ec.Set_ActingEntity(actingEntity);
            return ec;
        }
    }
}
