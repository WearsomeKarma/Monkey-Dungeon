
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Attribute
    {
        public readonly GameEntity_Attribute_Name Attribute_Name;

        internal GameEntity_ServerSide Internal_Parent { get; private set; }
        protected GameEntity_ServerSide Parent_EntityServerSide => Internal_Parent;

        public GameEntity_Attribute(GameEntity_Attribute_Name attributeName)
        {
            Attribute_Name = attributeName;
        }

        internal void Attach_To_Entity(GameEntity_ServerSide newEntityServerSide)
        {
            if (Internal_Parent != null)
                Detach_From_Entity();

            Internal_Parent = newEntityServerSide;
            Handle_Attach_To_Entity(Internal_Parent);
        }

        internal void Detach_From_Entity()
        {
            Handle_Detach_From_Entity(Internal_Parent);
            Internal_Parent = null;
        }

        protected virtual void Handle_Attach_To_Entity(GameEntity_ServerSide newEntityServerSide) { }
        protected virtual void Handle_Detach_From_Entity(GameEntity_ServerSide oldEntityServerSide) { }
    }
}
