using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities
{
    public class GameEntity_Attribute
    {
        public readonly string ATTRIBUTE_NAME;

        internal GameEntity Internal_Parent { get; private set; }
        protected GameEntity Parent_Entity => Internal_Parent;

        public GameEntity_Attribute(string attributeName)
        {
            ATTRIBUTE_NAME = attributeName;
        }

        internal void Attach_To_Entity(GameEntity newEntity)
        {
            if (Internal_Parent != null)
                Detach_From_Entity();

            Internal_Parent = newEntity;
            Handle_Attach_To_Entity(Internal_Parent);
        }

        internal void Detach_From_Entity()
        {
            Handle_Detach_From_Entity(Internal_Parent);
            Internal_Parent = null;
        }

        protected virtual void Handle_Attach_To_Entity(GameEntity newEntity) { }
        protected virtual void Handle_Detach_From_Entity(GameEntity oldEntity) { }
    }
}
