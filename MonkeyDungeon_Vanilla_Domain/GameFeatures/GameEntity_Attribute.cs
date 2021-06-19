
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Attribute<T> where T : GameEntity
    {
        public readonly GameEntity_Attribute_Name Attribute_Name;

        protected T Attached_Entity { get; private set; }

        protected GameEntity_Attribute(GameEntity_Attribute_Name attributeName)
        {
            Attribute_Name = attributeName;
        }

        protected void Attach_To__Entity__Attribute(T newEntityServerSide)
        {
            if (Attached_Entity != null)
                Detach_From__Entity__Attribute();

            Attached_Entity = newEntityServerSide;
            Handle_Attach_To_Entity(Attached_Entity);
        }

        protected void Detach_From__Entity__Attribute()
        {
            Handle_Detach_From_Entity(Attached_Entity);
            Attached_Entity = null;
        }

        protected virtual void Handle_Attach_To_Entity(T newEntityServerSide) { }
        protected virtual void Handle_Detach_From_Entity(T oldEntityServerSide) { }
    }
}
