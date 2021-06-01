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

        internal void Begin_Stage(Combat_Action action)
        {
            Handle_Stage(action);
        }
        protected abstract void Handle_Stage(Combat_Action action);
    }
}
