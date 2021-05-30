using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats
{
    public class GameEntity_Stat : Quantity
    {
        public GameEntity Parent_Entity { get; set; }

        public event Action<double> Quantity_Changed;

        public readonly string Stat_Name;

        public GameEntity_Stat(string statName, double minQuantity, double maxQuantity, double? initalValue = null)
            : base(minQuantity, maxQuantity)
        {
            Stat_Name = statName;

            if(initalValue != null)
                Set_Value((double)initalValue);
        }

        public GameEntity_Stat Clone()
        {
            return new GameEntity_Stat(Stat_Name, Value, Min_Quantity, Max_Quantity);
        }

        internal void Attach_To_Entity(GameEntity newEntity)
        {
            if (Parent_Entity != null)
                Detatch_From_Entity();

            Parent_Entity = newEntity;

            Handle_Attach_To_Entity(Parent_Entity);
        }

        internal void Detatch_From_Entity()
        {
            Handle_Detach_From_Entity(Parent_Entity);
            Parent_Entity = null;
        }

        protected override void Handle_Post_Offset_Value(double newValue)
        {
            Quantity_Changed?.Invoke(Value);
        }

        protected override void Handle_Post_Set_Value(double newMax)
        {
            Quantity_Changed?.Invoke(this);
        }

        protected virtual void Handle_Attach_To_Entity(GameEntity newEntity) { }
        protected virtual void Handle_Detach_From_Entity(GameEntity oldEntity) { }
    }
}
