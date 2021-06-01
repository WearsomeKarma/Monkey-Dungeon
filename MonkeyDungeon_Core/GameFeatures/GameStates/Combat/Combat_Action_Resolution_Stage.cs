using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat
{
    public abstract class Combat_Action_Resolution_Stage
    {
        private Combat_GameState Combat { get; set; }
        protected GameEntity_EntityField Entity_Field => Combat.Game_Field;
        protected GameEntity Get_Entity(GameEntity_ID id)
            => Entity_Field.Get_Entity(id);

        protected Combat_Action_Resolver Resolver { get; set; }
        internal void Bind_To_Resolver(Combat_Action_Resolver resolver, Combat_GameState combat)
        {
            Resolver = resolver;
            Combat = combat;
        }

        protected GameEntity Get_Entity(Combat_Action action)
            => Get_Entity(action.Action_Owner);

        protected GameEntity_Ability Get_Ability(Combat_Action action)
            => Get_Entity(action).Ability_Manager.Get_Ability<GameEntity_Ability>(action.Selected_Ability);
        
        internal void Begin_Stage(Combat_Action action)
        {
            Handle_Stage(action);
        }
        protected abstract void Handle_Stage(Combat_Action action);
    }
}
