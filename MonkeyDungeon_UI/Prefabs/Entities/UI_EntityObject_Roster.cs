using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_UI.Prefabs.Entities
{
    public class UI_EntityObject_Roster : GameEntity_Survey<UI_EntityObject>
    {
        //TODO: contemplate giving a non-null default value?
        internal UI_EntityObject_Roster() 
            : base(null)
        {
        }

        public UI_EntityObject Get_Entity(GameEntity_Position position)
            => Get_Entry_From_Position(position);

        public UI_EntityObject[] Get_Entities()
            => Get_Reduced_Field();

        public void Set_Entity(GameEntity_Position position, UI_EntityObject ui_entityObject)
            => Set_Entry_By_Position(position, ui_entityObject);
        
        public void Swap_UI_Elements(GameEntity_Position position, GameEntity_Position_Swap_Type swapType)
            => Swap_Entries(position, swapType);
    }
}