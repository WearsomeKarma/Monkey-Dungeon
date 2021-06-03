
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Attribute
    {
        public readonly GameEntity_Attribute_Name ATTRIBUTE_NAME;

        internal GameEntity Internal_Parent { get; private set; }
        protected GameEntity Parent_Entity => Internal_Parent;

        public GameEntity_Attribute(GameEntity_Attribute_Name attributeName)
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
