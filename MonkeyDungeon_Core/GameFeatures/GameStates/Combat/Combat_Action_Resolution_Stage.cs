using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat
{
    public abstract class Combat_Action_Resolution_Stage
    {
        protected Combat_GameState Combat { get; set; }
        protected Combat_Action_Resolver Resolver { get; set; }
        internal void Bind_To_Resolver(Combat_Action_Resolver resolver, Combat_GameState combat)
        {
            Resolver = resolver;
            Combat = combat;
        }

        internal void Begin_Stage(Combat_Action action)
        {
            Handle_Begin_Stage(action);
        }
        protected abstract void Handle_Begin_Stage(Combat_Action action);

        internal void End_Stage(Combat_Action action)
        {
            Handle_End_Stage(action);
        }
        protected abstract void Handle_End_Stage(Combat_Action action);
    }
}
